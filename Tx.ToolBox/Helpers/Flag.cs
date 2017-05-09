using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Helpers
{
    public class Flag
    {
        public IDisposable Set()
        {
            IsSet = true;
            return new Action(() => IsSet = false).AsDisposable();
        }

        public bool IsSet { get; private set; }
    }
}
