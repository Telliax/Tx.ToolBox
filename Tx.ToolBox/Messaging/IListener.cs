using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    /// <summary>
    /// Implementaions of this interface are recognized by IMessenger as event handlers for TMessage.
    /// </summary>
    /// <typeparam name="TMessage">Type of message.</typeparam>
    public interface IListener<in TMessage> where TMessage : IMessage
    {
        /// <summary>
        /// This method is called by IMessenger when it TMessage is published.
        /// </summary>
        /// <param name="message">Message received by IMessenger.</param>
        void Handle(TMessage message);
    }
}
