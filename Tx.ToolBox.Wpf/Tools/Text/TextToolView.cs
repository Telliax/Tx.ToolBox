using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Tools.Text
{
    public class TextToolView : TextBox
    {
        static TextToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TextToolView), new FrameworkPropertyMetadata(typeof(TextToolView)));
        }
    }
}
