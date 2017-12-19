using System.Windows;

namespace Tx.ToolBox.Wpf.Converters
{
    public class BoolToVisibilityConverter : BoolConverterBase<Visibility>
    {
        public BoolToVisibilityConverter() 
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
