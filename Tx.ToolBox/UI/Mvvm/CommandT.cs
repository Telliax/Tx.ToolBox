using System;

namespace Tx.ToolBox.UI.Mvvm
{
    public class Command<TParameter> : Command
    {
        public Command(Action<TParameter> action, Func<TParameter, bool> canExecute = null)
            : base(action == null ? (Action<object>)null : p => action((TParameter)p), 
                   canExecute == null ? (Func<object, bool>)null : p => canExecute((TParameter)p))
        {
        }
    }
}
