using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Threading
{
    public class RunLatestScheduler : TaskScheduler,  IDisposable
    {
        public RunLatestScheduler(bool waitOnDispose = true)
        {
            _waitOnDispose = waitOnDispose;
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

            AsyncEx.Run(() =>
            {
                _thread.Join();
                _taskReceived.Dispose();
            }, async: !_waitOnDispose);
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
        private volatile bool _isDisposed;
        private readonly bool _waitOnDispose;

        private void ExecutionLoop()
        {
            while (true)
            {
                _taskReceived.WaitOne();
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
