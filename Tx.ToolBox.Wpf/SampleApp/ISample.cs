using System.Windows;
using Castle.Windsor;

namespace Tx.ToolBox.Wpf.SampleApp
{
    public interface ISample
    {
        string Name { get; }
        string Description { get; }
        FrameworkElement ResolveView();
        void Load(IWindsorContainer appContainer);
        void Unload();
    }
}
