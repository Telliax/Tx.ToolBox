using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    [Template(typeof(ButtonToolView))]
    public abstract class ButtonTool : ButtonToolBase
    {
        protected ButtonTool()
        {
            Command = new Command(Execute, CanExecute);
        }

        public Command Command { get; } 

        protected abstract void Execute();
    }
}
