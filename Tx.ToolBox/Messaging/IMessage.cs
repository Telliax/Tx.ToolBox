using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    public interface IMessage
    {
        bool Handled { get; set; }
    }
}
