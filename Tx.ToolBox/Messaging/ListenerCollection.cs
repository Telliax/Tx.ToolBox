using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Messaging
{
    class ListenerCollection<TMessage> : IListenerCollection where TMessage : IMessage
    {
        public IDisposable Add(object listener)
        {
            if (!(listener is IListener<TMessage> typedListener))
                throw new InvalidOperationException(
                    $"Invalid listener type. Type {listener.GetType()} can not handle messages of type {typeof(TMessage)}.\n"
                    + "This is most likely a bug in IMessenger implementation.");

            lock (_list)
            {
                _list.Add(typedListener.Handle);
            }
            return new Action(() =>
            {
                lock (_list)
                {
                    if (_handleFlag)
                    {
                        _removedHandlers.Add(typedListener.Handle);
                    }
                    else
                    {
                        _list.Remove(typedListener.Handle);
                    }
                }
            }).AsDisposable();
        }

        public void Handle(object message)
        {
            if (message.GetType() != typeof(TMessage))
                throw new InvalidOperationException(
                    $"Invalid message type. Expected: {typeof(TMessage)}. Recieved: {message.GetType()}.\n"
                    +"This is most likely a bug in IMessenger implementation.");

            lock (_list)
            {
                var typedMessage = (TMessage)message;

                using (_handleFlag.Set())
                {
                    foreach (var handler in _list)
                    {
                        if (typedMessage.Handled) return;
                        handler(typedMessage);
                    }
                }

                if (_removedHandlers.Any())
                {
                    _list.RemoveRange(_removedHandlers);
                    _removedHandlers.Clear();
                }
            }
        }

        private readonly Flag _handleFlag = new Flag();
        private readonly List<Action<TMessage>> _list = new List<Action<TMessage>>();
        private readonly List<Action<TMessage>> _removedHandlers = new List<Action<TMessage>>();
    }
}