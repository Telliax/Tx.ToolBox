using System;
using System.Windows.Input;

namespace Tx.ToolBox.Wpf.Mvvm
{
    public abstract class CommandBase : ViewModel, ICommand
    {
        public bool UseCommandManager { get; set; }

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

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        private event EventHandler UserCanExecuteChanged;
    }
}