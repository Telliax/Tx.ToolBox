using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    public static class DisposableEx
    {
        public static IDisposable AsDisposable(this Action action)
        {
            return new DisposableHandle(action);
        }

        public static IDisposable Combine(this IEnumerable<IDisposable> disposables)
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
