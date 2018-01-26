using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public abstract class AsyncCommandBase : CommandBase, IDisposable
    {
        protected AsyncCommandBase()
        {
            CancelCommand = new Command(Cancel, () => IsExecuting && !_cts.IsCancellationRequested) { UseCommandManager = false };
            CreateNewToken();
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                Cancel();
                _cts.Dispose();
                _disposed = true;
            }
        }

        public Command CancelCommand { get; }

        public bool IsExecuting
        {
            get => _isExecuting;
            private set
            {
                SetField(ref _isExecuting, value);
                RefreshCanExecute();
                CancelCommand.RefreshCanExecute();
            } 
        }

        public sealed override bool CanExecute(object parameter)
        {
            return !IsExecuting && OnCanExecute(parameter);
        }

        public sealed override async void Execute(object parameter)
        {
            if (_token.IsCancellationRequested)
            {
                lock (_lock)
                {
                    if (_disposed) return;
                    var old = _cts;
                    CreateNewToken();
                    old.Dispose();
                }
            }
            IsExecuting = true;
            await OnExecute(parameter, _token).ConfigureAwait(true);
            IsExecuting = false;
        }

        protected abstract Task OnExecute(object parameter, CancellationToken token);
        protected abstract bool OnCanExecute(object parameter);

        private bool _isExecuting;
        private CancellationTokenSource _cts;
        private CancellationToken _token;
        private readonly object _lock = new object();
        private bool _disposed;

        private void Cancel()
        {
            if (!IsExecuting) return;
            _cts.Cancel();
        }

        private void CreateNewToken()
        {
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
        }
    }
}