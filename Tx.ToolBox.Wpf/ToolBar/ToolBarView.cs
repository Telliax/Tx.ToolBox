using System.Windows;

namespace Tx.ToolBox.Wpf.ToolBar
{
    public class ToolBarView : System.Windows.Controls.ToolBar
    {
        static ToolBarView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolBarView), new FrameworkPropertyMetadata(typeof(ToolBarView)));
        }
    }
}