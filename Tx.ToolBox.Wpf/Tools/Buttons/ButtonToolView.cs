using System.Windows;

namespace Tx.ToolBox.Wpf.Tools.Buttons
{
    public class ButtonToolView : System.Windows.Controls.Button
    {
        static ButtonToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonToolView), new FrameworkPropertyMetadata(typeof(ButtonToolView)));
        }
    }
}