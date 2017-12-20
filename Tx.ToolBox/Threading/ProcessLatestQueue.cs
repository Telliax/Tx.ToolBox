using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Threading
{
    public class ProcessLatestQueue : IActionQueue, IDisposable
    {
        public Task<bool> Post(Action<CancellationToken> action)
        {
            lock (_lock)
            {
                if (_disposed) return Task.FromResult(false);

                _nextJob?.Drop();
                _nextJob = new Job(action);

                WakeUp();

                return _nextJob.Task;
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                _cts.Cancel();
                try
                {
                    _taskQueue.Wait();
                }
                catch (TaskCanceledException)
                {
                }
                _cts.Dispose();
                _disposed = true;
            }
        }

        private Task _taskQueue = Task.CompletedTask;
        private Job _nextJob;
        private readonly object _lock = new object();
        private bool _disposed;
        private bool _isWorking;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private void WakeUp()
        {
            if (_isWorking) return;

            _isWorking = true;
            _taskQueue = _taskQueue.ContinueWith(t =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    Job job;
                    lock (_lock)
                    {
                        if (_nextJob == null)
                        {
                            _isWorking = false;
                            return;
                        }
                        job = _nextJob;
                        _nextJob = null;
                    }
                    job.Execute(_cts.Token);
                }
            }, TaskScheduler.Default);
        }

        private class Job
        {
            public Job(Action<CancellationToken> action)
            {
                _action = action;
                _result = new TaskCompletionSource<bool>();
            }

            public void Execute(CancellationToken token)
            {
                _action(token);
                _result.SetResult(true);
            }

            public void Drop()
            {
                _result.SetResult(false);
            }

            public Task<bool> Task => _result.Task;

            private readonly Action<CancellationToken> _action;
            private readonly TaskCompletionSource<bool> _result;
        }
    }
}
