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
    }
}
