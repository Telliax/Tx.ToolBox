using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Threading
{
    public class RunLatestScheduler : TaskScheduler,  IDisposable
    {
        public RunLatestScheduler()
        {
            _thread = new Thread(ExecutionLoop);
            _thread.Start();
        }

        public override int MaximumConcurrencyLevel => 1;

        public void Dispose()
        {
            if (_isDisposed) return;

            lock (_lock)
            {
                _isDisposed = true;
                _taskReceived.Set();
            }
            _thread.Join();
            _taskReceived.Dispose();
        }

        protected override void QueueTask(Task task)
        {
            lock (_lock)
            {
                _nextTask = task;
                _taskReceived.Set();
            }
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            lock (_lock)
            {
                if (_nextTask == null) yield break;

                yield return _nextTask;
            }
        }

        private readonly Thread _thread;
        private Task _nextTask;
        private readonly object _lock = new object();
        private readonly ManualResetEvent _taskReceived = new ManualResetEvent(false);
        private volatile bool _isDisposed = false;

        private void ExecutionLoop()
        {
            while (_taskReceived.WaitOne())
            {
                Task task;
                lock (_lock)
                {
                    if (_isDisposed) return;
                    task = _nextTask;
                    _nextTask = null;
                    _taskReceived.Reset();
                }
                TryExecuteTask(task);
            }
        }
    }
}
