using System;
using System.Threading;
using System.Threading.Tasks;
using Tx.ToolBox.Wpf.Templates;
using Tx.ToolBox.Wpf.ToolBar.Tools.Button;

namespace Tx.ToolBox.Wpf.ToolBar.Tools.AsyncButton
{
    [Template(typeof(AsyncButtonToolView))]
    public abstract class AsyncButtonTool : ButtonTool, IDisposable
    {
        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { SetField(ref _isExecuting, value); }
        }

        public void Dispose()
        {
            lock (_disposeLock)
            {
                if (_disposed) return;
                if (_isExecuting)
                {
                    _cts.Cancel();
                    _completedEvent.WaitOne();
                }
                _completedEvent.Dispose();
                OnDispose();
                _disposed = true;
            }
        }

        protected sealed override async void Execute()
        {
            lock (_disposeLock)
            {
                if (_disposed) return;
                _completedEvent.Reset();
                IsExecuting = true;
            }
            try
            {
                using (_cts = new CancellationTokenSource())
                {
                    await ExecuteAsync(_cts.Token);
                }
            }
            finally 
            {
                _completedEvent.Set();
                IsExecuting = false;
            }
        }

        protected override bool CanExecute()
        {
            return !_isExecuting;
        }

        protected virtual void OnDispose()
        {
        }

        protected abstract Task ExecuteAsync(CancellationToken token);

        private bool _isExecuting;
        private CancellationTokenSource _cts ;
        private readonly ManualResetEvent _completedEvent = new ManualResetEvent(false);
        private readonly object _disposeLock = new object();
        private bool _disposed;
    }
}
