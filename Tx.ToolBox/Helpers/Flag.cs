using System;
using System.Threading;

namespace Tx.ToolBox.Helpers
{
    /// <summary>
    /// A simple bool flag that is meant to be used in `using` blocks, so you do not forget to reset it.
    /// Works correctly even if using blocks are nested.
    /// </summary>
    public class Flag
    {
        /// <summary>
        /// Temporary sets the flag value to true.
        /// </summary>
        /// <returns>Sets flag value back to false when disposed (unless it was set more than once).</returns>
        public IDisposable SetTemporary()
        {
            Interlocked.Increment(ref _setCount);
            int unset = 0;
            return DisposableEx.AsDisposable(() => 
            {
                if (Interlocked.CompareExchange(ref unset, 1, 0) == 1) return;
                Interlocked.Decrement(ref _setCount);
            });
        }

        /// <summary>
        /// Permanently, sets the flag value to true.
        /// </summary>
        public void SetPermanently()
        {
            Interlocked.Increment(ref _setCount);
        }

        /// <summary>
        /// Current state of the flag.
        /// </summary>
        public bool IsSet => _setCount > 0;

        public static implicit operator bool(Flag flag)
        {
            return flag.IsSet;
        }

        private volatile int _setCount;
    }
}
