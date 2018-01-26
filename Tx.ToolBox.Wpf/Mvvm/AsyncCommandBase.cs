using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public abstract class AsyncCommandBase : CommandBase
    {
        protected AsyncCommandBase()
        {
            _cts = new CancellationTokenSource();
            CancelCommand = new Command(_cts.Cancel, () => IsExecuting && !_cts.IsCancellationRequested) { UseCommandManager = false };
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
            using (_cts = new CancellationTokenSource())
            {
                IsExecuting = true;
                await OnExecute(parameter, _cts.Token).ConfigureAwait(true);
                IsExecuting = false;
            }
        }

        protected abstract Task OnExecute(object parameter, CancellationToken token);
        protected abstract bool OnCanExecute(object parameter);

        private bool _isExecuting;
        private CancellationTokenSource _cts;
    }
}