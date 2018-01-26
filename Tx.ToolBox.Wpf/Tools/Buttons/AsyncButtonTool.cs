using System;
using System.Threading;
using System.Threading.Tasks;
using Tx.ToolBox.Wpf.Mvvm;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    [Template(typeof(AsyncButtonToolView))]
    public abstract class AsyncButtonTool : ButtonToolBase, IDisposable
    {
        public AsyncButtonTool()
        {
            Command = new AsyncCommand(ExecuteAsync, CanExecute);
        }

        public AsyncCommand Command { get; }

        public void Dispose()
        {
            Command.Dispose();
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }

        protected abstract Task ExecuteAsync(CancellationToken token);
    }
}
