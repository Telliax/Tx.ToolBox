using System;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    public static class AsyncEx
    {
        public static void Forget(this Task task)
        {
        }

        public static void RethrowOnThreadPool(this Exception ex)
        {
            Task.Run(() => throw new AggregateException(ex));
        }

        public static Task Run(this Action action, bool async)
        {
            if (!async)
            {
                action();
                return Task.CompletedTask;
            }

            return Task.Run(action);
        }
    }
}
