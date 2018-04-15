using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Tools.Text
{
    public class LabelToolView : TextBlock
    {
        static LabelToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LabelToolView), new FrameworkPropertyMetadata(typeof(LabelToolView)));
        }
    }
}
