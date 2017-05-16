using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Messaging
{
    /// <summary>
    /// TPL.Dataflow-based implementation of IMessenger.
    /// Guarantees that messages are processed with a degree of parallelism of 1 in FIFO order.
    /// </summary>
    public class Messenger : IMessenger, IDisposable
    {
        public Messenger(int bufferSize = 1000)
        {
            if (bufferSize < 1) throw new ArgumentException("Buffer size can not be less that 1.", nameof(bufferSize));

            _pipeLine = new ActionBlock<Job>((Action<Job>)Handle, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = bufferSize,
            });
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

            var subscriptions = listener.GetType()
                                        .GetInterfaces()
                                        .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IListener<>))
                                        .Select(listenerType => listenerType.GetGenericArguments()[0])
                                        .Select(messageType => Subscribe(messageType, listener))
                                        .ToArray();
            if (!subscriptions.Any())
                throw new InvalidOperationException(
                    $"{listener.GetType().Name} does not implement IListener<T>. Use IMessenger.Subscribe<TMessage> instead.");

            return subscriptions.Combine();
        }

        public IDisposable Subscribe<TMessage>(Action<TMessage> handler) where TMessage : IMessage
        {
            if (handler == null) throw new ArgumentNullException(nameof(handler));

            return Subscribe(typeof(TMessage), new DelegateListener<TMessage>(handler));
        }

        public async Task<bool> PublishAsync(IMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            var job = new Job(message);
            var enqueued = await _pipeLine.SendAsync(job);
            if (!enqueued)
            {
                job.Result.SetResult(false);
            }
            return await job.Result.Task;
        }

        private readonly Dictionary<Type, IListenerCollection> _subscribers = new Dictionary<Type, IListenerCollection>();
        private readonly ActionBlock<Job> _pipeLine;
        private bool _disposed;

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

        private void Handle(Job job)
        {
            IListenerCollection listeners;
            lock (_subscribers)
            {
                _subscribers.TryGetValue(job.Message.GetType(), out listeners);
            }
            try
            {
                listeners?.Handle(job.Message);
                job.Result.SetResult(true);
            }
            catch (Exception ex)
            {
                job.Result.SetException(ex);
            }
        }

        private class Job
        {
            public Job(IMessage message)
            {
                Message = message;
            }
            public IMessage Message { get; }
            public TaskCompletionSource<bool> Result { get; } = new TaskCompletionSource<bool>();
        }
    }
}
