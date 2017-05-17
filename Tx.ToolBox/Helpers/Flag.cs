using System;
using System.Threading;

namespace Tx.ToolBox.Helpers
{
    /// <summary>
    /// A simple bool falg that is meant to be used in `using` blocks, so you do not forget to reset it.
    /// Works correctly even if using blocks are nested.
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Sets the flag value to true.
        /// </summary>
        /// <returns>Sets flag value to false when disposed.</returns>
        public IDisposable Set()
        {
            Interlocked.Increment(ref _setCount);
            return DisposableEx.AsDisposable(() => Interlocked.Decrement(ref _setCount));
        }
        /// <summary>
        /// Current state of the flag.
        /// </summary>
        public bool IsSet { get { return _setCount > 0; } }

        private int _setCount;
    }
}
