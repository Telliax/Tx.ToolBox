using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tx.ToolBox.UI.Converters
{
    public class BoolToVisibilityConverter : BoolConverterBase<Visibility>
    {
        public BoolToVisibilityConverter() 
            : base(Visibility.Visible, Visibility.Collapsed)
        {
        }
    }
}
