using System;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public class Command<TParameter> : CommandBase
    {
        public Command(Action<TParameter> action, Func<TParameter, bool> canExecute = null)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
            _canExecute = canExecute ?? (p => true);
            UseCommandManager = canExecute != null;
        }

        public static implicit operator Command<TParameter>(Action<TParameter> action)
        {
            return new Command<TParameter>(action);
        }

        public override bool CanExecute(object parameter)
        {
            return _canExecute((TParameter)parameter);
        }

        public override void Execute(object parameter)
        {
            _action((TParameter)parameter);
        }

        private readonly Action<TParameter> _action;
        private readonly Func<TParameter, bool> _canExecute;
    }
}
