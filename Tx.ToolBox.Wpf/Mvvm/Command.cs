using System;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public class Command : CommandBase
    {
        public Command(Action action, Func<bool> canExecute = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = canExecute ?? (() => true);
            UseCommandManager = canExecute != null;
        }

        public static implicit operator Command(Action action)
        {
            return new Command(action);
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute();
        }

        public override void Execute(object parameter)
        {
            _action();
        }

        private readonly Action _action;
        private readonly Func<bool> _canExecute;
    }
}
