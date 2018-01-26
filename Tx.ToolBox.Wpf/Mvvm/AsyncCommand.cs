using System;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public class AsyncCommand : AsyncCommandBase
    {
        public AsyncCommand(Func<CancellationToken, Task> asyncAction, Func<bool> canExecute = null)
        {
            _action = asyncAction ?? throw new ArgumentNullException(nameof(asyncAction));
            _canExecute = canExecute ?? (() => true);
            UseCommandManager = canExecute != null;
        }

        protected override Task OnExecute(object parameter, CancellationToken token)
        {
            return _action(token);
        }

        protected override bool OnCanExecute(object parameter)
        {
            return _canExecute();
        }

        private readonly Func<CancellationToken, Task> _action;
        private readonly Func<bool> _canExecute;
    }
}
