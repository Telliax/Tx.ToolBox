using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    public static class EnumEx
    {
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
