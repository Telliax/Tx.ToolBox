using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tx.ToolBox.Wpf.Converters;

namespace Tx.ToolBox.Wpf.SampleApp.Samples
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
