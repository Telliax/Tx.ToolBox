using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    /// <summary>
    /// Message that can be delivered to subscribers via IMessenger.
    /// </summary>
    public interface IMessage
    {
        /// <summary>
        /// Whether or not message was handled by subscriber.
        /// </summary>
        bool Handled { get; set; }
    }
}
