using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tx.ToolBox.Messaging
{
    /// <summary>
    /// Events aggregator
    /// </summary>
    public interface IMessenger
    {
        /// <summary>
        /// Subscribes an object to all relevant events.
        /// </summary>
        /// <param name="listener">Object, that implements one or more IListener<TMessage> interfaces</param>
        /// <returns>Subscription handle. Dispose to unsubscribe.</returns>
        IDisposable Subscribe(object listener);

        /// <summary>
        /// Subscribes a delegate to TMessage event.
        /// </summary>
        /// <param name="handler">Event handler.</param>
        /// <returns>Subscription handle. Dispose to unsubscribe.</returns>
        IDisposable Subscribe<TMessage>(Action<TMessage> handler)
            where TMessage : IMessage;

        /// <summary>
        /// Sends message to event pipeline.
        /// </summary>
        /// <param name="message">Message to send.</param>
        /// <returns>Awaitable task, that returns true if message was processed successfully. Otherwise - false.</returns>
        Task<bool> PublishAsync(IMessage message);
    }
}
