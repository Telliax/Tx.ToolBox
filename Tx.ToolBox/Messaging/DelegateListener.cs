using System;

namespace Tx.ToolBox.Messaging
{
    class DelegateListener<TMessage> : IListener<TMessage>
        where TMessage : IMessage
    {
        public DelegateListener(Action<TMessage> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public void Handle(TMessage message)
        {
            _handler(message);
        }

        private readonly Action<TMessage> _handler;
    }
}