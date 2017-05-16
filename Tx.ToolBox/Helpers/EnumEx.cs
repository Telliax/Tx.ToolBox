using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    /// <summary>
    /// A bunch of helper methods realted to enums
    /// </summary>
    public static class EnumEx
    {
        /// <summary>
        /// Pretty way to get all enum values.
        /// </summary>
        /// <typeparam name="T">Type of enum.</typeparam>
        /// <returns>Array of enum values.</returns>
        public static T[] GetValues<T>()
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
