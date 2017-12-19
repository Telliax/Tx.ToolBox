using System;
using System.Collections.Generic;
using System.Linq;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Wpf.Converters
{
    public abstract class EnumConverterBase<TEnum> : StringConverterBase<TEnum>
    {
        protected EnumConverterBase(Func<TEnum, string> converter)
        {
            if (!typeof(TEnum).IsEnum) throw new NotSupportedException("Only enums are supported!");
            if (converter == null) throw new ArgumentNullException(nameof(converter));

            var values = EnumEx.GetValues<TEnum>();
            _forwardMap = values.ToDictionary(x => x, converter);
            _backwardMap = values.ToDictionary(converter, x => x);
        }

        protected override string ToString(TEnum source)
        {
            return _forwardMap[source];
        }

        protected override TEnum FromString(string target)
        {
            return _backwardMap[target];
        }

        private readonly Dictionary<TEnum, string> _forwardMap;
        private readonly Dictionary<string, TEnum> _backwardMap;
    }
}
