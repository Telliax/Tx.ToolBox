using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tx.ToolBox.Helpers;
using Tx.ToolBox.Threading;

namespace Tx.ToolBox.Messaging
{
    /// <summary>
    /// Guarantees that messages are processed with a degree of parallelism of 1 in FIFO order.
    /// </summary>
    public class Messenger : IMessenger, IDisposable
    {
        public Messenger(int bufferSize = 1000)
        {
            if (bufferSize < 1) throw new ArgumentException("Buffer size can not be less than 1.", nameof(bufferSize));
            _scheduler = new FifoScheduler(bufferSize);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _scheduler.Dispose();
            _disposed = true;
        }

        public IDisposable Subscribe(object listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            var subscriptions = listener.GetType()
                                        .GetInterfaces()
                                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListener<>))
                                        .Select(listenerType => listenerType.GetGenericArguments()[0])
                                        .Select(messageType => Subscribe(messageType, listener))
                                        .ToArray();
            if (!subscriptions.Any())
                throw new InvalidOperationException(
                    $"{listener.GetType().Name} does not implement IListener<T>. Use IMessenger.Subscribe<TMessage> instead.");

            return subscriptions.AsDisposable();
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return Subscribe(typeof(TMessage), new DelegateListener<TMessage>(handler));
        }

        public async Task PublishAsync(IMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            await Task.Factory.StartNew(Publish, CancellationToken.None, TaskCreationOptions.None, _scheduler);

            void Publish()
            {
                IListenerCollection listeners;
                lock (_subscribers)
                {
                    _subscribers.TryGetValue(message.GetType(), out listeners);
                }
                listeners?.Handle(message);
            }
        }

        private readonly Dictionary<Type, IListenerCollection> _subscribers = new Dictionary<Type, IListenerCollection>();
        private bool _disposed;
        private readonly FifoScheduler _scheduler;

        private IDisposable Subscribe(Type messageType, object listener)
        {
            IListenerCollection list;

            lock (_subscribers)
            {
                if (!_subscribers.TryGetValue(messageType, out list))
                {
                    var collectionType = typeof(ListenerCollection<>).MakeGenericType(messageType);
                    list = (IListenerCollection)Activator.CreateInstance(collectionType);
                    _subscribers[messageType] = list;
                }
            }

            return list.Add(listener);
        }
    }
}
