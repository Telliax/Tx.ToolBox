using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Tx.ToolBox.UI.Converters
{
    public abstract class StringConverterBase<TSource> : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DependencyProperty.UnsetValue;

            var collection = value as IEnumerable<TSource>;
            if (collection != null)
            {
                return new CollectionAdapter(collection.Select(ToString));
            }

            return ToString((TSource)value);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is CollectionAdapter)
            {
                throw new NotSupportedException("Two-way or one-way-to-source convertions are not supported for collections. Override ItemTemplate instead.");
            }
            return FromString((string) value);
        }

        protected abstract string ToString(TSource source);
        protected virtual TSource FromString(string target)
        {
            throw new NotImplementedException();
        }

        private class CollectionAdapter : List<string>, INotifyCollectionChanged
        {
            public CollectionAdapter(IEnumerable<string> collection)
            {
                AddRange(collection);
            }

            public event NotifyCollectionChangedEventHandler CollectionChanged
            {
                add {  }
                remove {  }
            }
        }
    }
}
