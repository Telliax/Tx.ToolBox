using System.Windows;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    public class AsyncButtonToolView : ButtonToolView
    {
        static AsyncButtonToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AsyncButtonToolView), new FrameworkPropertyMetadata(typeof(AsyncButtonToolView)));
        }
    }
}
