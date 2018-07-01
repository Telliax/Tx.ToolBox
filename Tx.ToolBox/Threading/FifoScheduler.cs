using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Threading
{
    public class FifoScheduler : TaskScheduler, IDisposable
    {
        public FifoScheduler(int maxQueueCapacity, bool waitOnDispose = true) 
        {
            _waitOnDispose = waitOnDispose;
            _tasks = new BlockingCollection<Task>(maxQueueCapacity);
            _thread = new Thread(ExecutionLoop);
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
            _thread.Start();
        }

        public override int MaximumConcurrencyLevel => 1;

        public void Dispose()
        {
            if (_token.IsCancellationRequested) return;

            _cts.Cancel();

            AsyncEx.Run(() =>
            {
                _thread.Join();
                _tasks.Dispose();
                _cts.Dispose();
            }, async: !_waitOnDispose);
        }

        protected override void QueueTask(Task task)
        {
            _tasks.Add(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            return false;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks.ToArray();
        }

        private readonly BlockingCollection<Task> _tasks;
        private readonly Thread _thread;
        private readonly CancellationTokenSource _cts;
        private CancellationToken _token;
        private readonly bool _waitOnDispose;

        private void ExecutionLoop()
        {
            while (true)
            {
                try
                {
                    if (_token.IsCancellationRequested) return;
                    var task = _tasks.Take(_token);
                    TryExecuteTask(task);
                }
                catch (OperationCanceledException)
                {
                }
            }
        }
    }
}
