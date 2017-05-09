using System;
using System.Windows.Input;

namespace Tx.ToolBox.UI.Mvvm
{
    public class Command : ICommand
    {
        public Command(Action action, Func<bool> canExecute = null)
            : this(action == null ? (Action<object>)null : _ => action(),
                   canExecute == null ? (Func<object, bool>)null : _ => canExecute())
        {
        }

        public Command(Action<object> action, Func<object, bool> canExecute = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = canExecute;

            if (_canExecute != null) return;
            _canExecute = _ => true;
            UseCommandManager = false;
        }

        public bool UseCommandManager { get; set; } = true;

        public void RefreshCanExecute()
        {
            UserCanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (UseCommandManager)
                {
                    CommandManager.RequerySuggested += value;
                }
                UserCanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                UserCanExecuteChanged -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
        }

        private readonly Action<object> _action;
        private readonly Func<object, bool> _canExecute;
        private event EventHandler UserCanExecuteChanged;
    }
}
