using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public class AsyncCommand<TParameter> : AsyncCommandBase
    {
        public AsyncCommand(Func<TParameter, CancellationToken, Task> asyncAction, Func<TParameter, bool> canExecute = null)
        {
            _action = asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));
            _canExecute = canExecute ?? (p => true);
            UseCommandManager = canExecute != null;
        }

        protected override Task OnExecute(object parameter, CancellationToken token)
        {
            return _action((TParameter)parameter, token);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return _canExecute((TParameter)parameter);
        }

        private readonly Func<TParameter, CancellationToken, Task> _action;
        private readonly Func<TParameter, bool> _canExecute;
    }
}
