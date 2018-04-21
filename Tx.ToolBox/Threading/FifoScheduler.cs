using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Threading
{
    class FifoScheduler : TaskScheduler, IDisposable
    {
        public FifoScheduler(int maxQueueCapacity) 
        {
            _tasks = new BlockingCollection<Task>(maxQueueCapacity);
            _thread = new Thread(ExecutionLoop);
            _thread.Start();
        }

        public override int MaximumConcurrencyLevel => 1;

        public void Dispose()
        {
            if (!_thread.IsAlive) return;

            _cts.Cancel();
            _thread.Join();
            _tasks.Dispose();
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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private void ExecutionLoop()
        {
            while (true)
            {
                var task = _tasks.Take(_cts.Token);
                if (_cts.IsCancellationRequested) return;
                TryExecuteTask(task);
            }
        }
    }
}
