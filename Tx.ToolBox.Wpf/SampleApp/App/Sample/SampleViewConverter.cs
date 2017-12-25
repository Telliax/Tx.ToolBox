using System;
using System.Globalization;
using Tx.ToolBox.Wpf.Converters;
using Tx.ToolBox.Wpf.SampleApp.App.List;

namespace Tx.ToolBox.Wpf.SampleApp.App.Sample
{
    class SampleViewConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var sample = value as SampleViewModel;
            return sample?.Sample.ResolveView();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
