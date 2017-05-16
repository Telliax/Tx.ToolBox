using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    /// <summary>
    /// A simple bool falg that is meant to be used in `using` blocks, so you do not forget to reset it.
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Sets the flag value to true.
        /// </summary>
        /// <returns>Sets flag value to false when disposed.</returns>
        public IDisposable Set()
        {
            IsSet = true;
            return new Action(() => IsSet = false).AsDisposable();
        }
        /// <summary>
        /// Current state of the flag.
        /// </summary>
        public bool IsSet { get; private set; }
    }
}
