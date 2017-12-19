using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace Tx.ToolBox.Wpf.Converters
{
    public abstract class MarkupExtensionBase : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public abstract class ConverterBase : MarkupExtensionBase, IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }

    public abstract class MultiConverterBase : MarkupExtensionBase, IMultiValueConverter
    {
        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);
        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
