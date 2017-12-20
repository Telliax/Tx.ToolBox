using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tx.ToolBox.Threading
{
    interface IActionQueue
    {
        Task<bool> Post(Action<CancellationToken> action);
    }
}
