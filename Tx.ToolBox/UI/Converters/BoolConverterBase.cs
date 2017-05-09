using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tx.ToolBox.UI.Converters
{
    public abstract class BoolConverterBase<TTarget> : ConverterBase
    {
        protected BoolConverterBase(TTarget trueValue, TTarget falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public TTarget True { get; set; }
        public TTarget False { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;
            return (bool) value ? True : False;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return EqualityComparer<TTarget>.Default.Equals((TTarget)value, True);
        }
    }
}
