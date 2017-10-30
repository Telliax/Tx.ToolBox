using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    /// <summary>
    /// A bunch of helper methods realted to IDisposable interface
    /// </summary>
    public static class DisposableEx
    {
        /// <summary>
        /// Creates a disposable object that executes given action when dispose is called.
        /// </summary>
        /// <param name="action">Action to call on dispose</param>
        /// <returns>Disposable wrapper.</returns>
        public static IDisposable AsDisposable(this Action action)
        {
            return new DisposableHandle(action);
        }

        /// <summary>
        /// Creates a disposable object that disposes every item in given collection.
        /// </summary>
        /// <param name="disposables">Collection of disposable objects.</param>
        /// <returns>Disposable wrapper.</returns>
        public static IDisposable AsDisposable(this IEnumerable<IDisposable> disposables)
        {
            return new DisposableHandle(disposables.Select<IDisposable, Action>(x => x.Dispose).ToArray());
        }

        private class DisposableHandle : IDisposable
        {
            public DisposableHandle(Action action)
            {
                _action = action ?? throw new ArgumentNullException(nameof(action));
            }

            public DisposableHandle(Action[] actions) : this(() => Array.ForEach(actions, a => a()))
            {
                if (actions.Any(a => a == null)) throw new ArgumentNullException(nameof(actions));
            }

            public void Dispose()
            {
                if (_disposed) return;
                _action();
                _disposed = true;
            }

            private readonly Action _action;
            private bool _disposed;
        }
    }
}
