using System.Windows;
using System.Windows.Controls;

namespace Tx.ToolBox.Wpf.Tools.Popup
{
    public class ComboBoxToolView : ComboBox
    {
        static ComboBoxToolView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ComboBoxToolView), new FrameworkPropertyMetadata(typeof(ComboBoxToolView)));
        }
    }
}
