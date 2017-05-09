using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Messaging
{
    public class Messenger : IMessenger, IDisposable
    {
        public Messenger(int bufferSize = 1000)
        {
            if (bufferSize < 1) throw new ArgumentException("Buffer size can not be less that 1.", nameof(bufferSize));

            var options = new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = bufferSize,
            };
            _pipeLineStart = new TransformBlock<IMessage, IMessage>(m =>
                {
                    IListenerCollection listeners;
                    lock (_subscribers)
                    {
                        _subscribers.TryGetValue(m.GetType(), out listeners);
                    }
                    listeners?.Handle(m);
                    return m;
                }, options);

            _pipeLineEnd = new ActionBlock<IMessage>(m => { });
            _pipeLineStart.LinkTo(_pipeLineEnd);
        }

        public void Dispose()
        {
            if (_disposed) return;
            _pipeLine.Complete();
            _pipeLine.Completion.Wait();
            _disposed = true;
        }

        public IDisposable Subscribe(object listener)
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            var subscribtions = listener.GetType()
                                        .GetInterfaces()
                                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListener<>))
                                        .Select(listenerType => listenerType.GetGenericArguments()[0])
                                        .Select(messageType => Subscribe(messageType, listener))
                                        .ToArray();
            if (!subscribtions.Any())
                throw new InvalidOperationException(
                    $"{listener.GetType().Name} does not implement IListener<T>. Use IMessenger.Subscribe<TMessage> instead.");

            return subscribtions.Combine();
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return Subscribe(typeof(TMessage), new DelegateListener<TMessage>(handler));
        }

        public void Publish(IMessage message)
        {
            PublishAsync(message).Wait();
        }

        public async Task PublishAsync(IMessage message)
        {
            await _pipeLine.SendAsync(message);
        }

        private readonly Dictionary<Type, IListenerCollection> _subscribers = new Dictionary<Type, IListenerCollection>();
        private readonly TransformBlock<IMessage, IMessage> _pipeLine;
        private bool _disposed;
        private ISourceBlock<IMessage> _pipeLineStart;
        private ITargetBlock<IMessage> _pipeLineEnd;

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
