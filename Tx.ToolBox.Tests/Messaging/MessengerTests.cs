using System;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Messaging;
using System.Threading.Tasks;
using System.Linq;

namespace Tx.ToolBox.Tests.Messaging
{
    [TestFixture]
    class MessengerTests
    {
        [Test]
        public void Ctor_OnInvalidBufferSize_Throws()
        {
            Assert.Throws<ArgumentException>(() => new Messenger(0));
        }

        [Test]
        public void Subscribe_OnNullListener_Throws()
        {
            using (var messenger = new Messenger())
            {
                IListener<MessageA> listener = null;
                Assert.Throws<ArgumentNullException>(() => messenger.Subscribe(listener));
            }
        }

        [Test]
        public void Subscribe_OnNotListener_Throws()
        {
            using (var messenger = new Messenger())
            {
                var listener = new object();
                Assert.Throws<InvalidOperationException>(() => messenger.Subscribe(listener));
            }
        }

        [Test]
        public void Subscribe_OnNullDelegate_Throws()
        {
            using (var messenger = new Messenger())
            {
                Action<MessageA> listener = null;
                Assert.Throws<ArgumentNullException>(() => messenger.Subscribe(listener));
            }
        }

        [Test]
        public void Publish_OnNullMessage_Throws()
        {
            using (var messenger = new Messenger())
            {
                Assert.Throws<AggregateException>(() => messenger.PublishAsync(null).Wait());
            }
        }

        [TestFixture]
        public class FacadeTests
        {
            [Test]
            public async Task OnMessageSent_MessageHandledByApproperiateSubscribers()
            {
                using (var messenger = new Messenger())
                {
                    var listener1 = Mock.Of<IListener<MessageA>>();
                    var listener2 = Mock.Of<Action<MessageA>>();
                    var listener3 = Mock.Of<IListener<MessageB>>();
                    messenger.Subscribe(listener1);
                    messenger.Subscribe(listener2);
                    messenger.Subscribe(listener3);
                    await messenger.PublishAsync(new MessageA());
                    await messenger.PublishAsync(new MessageB());
                    await messenger.PublishAsync(new MessageC());
                    await messenger.PublishAsync(new MessageA());
                    Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Exactly(2));
                    Mock.Get(listener2).Verify(l => l(It.IsAny<MessageA>()), Times.Exactly(2));
                    Mock.Get(listener3).Verify(l => l.Handle(It.IsAny<MessageB>()), Times.Once);
                }
            }

            [Test]
            public async Task OnMessageSent_MessageIgnoredByDisposedListeners()
            {
                using (var messenger = new Messenger())
                {
                    var listener1 = Mock.Of<IListener<MessageA>>();
                    var listener2 = Mock.Of<IListener<MessageA>>();
                    var subscription = messenger.Subscribe(listener1);
                    messenger.Subscribe(listener2);
                    await messenger.PublishAsync(new MessageA());
                    subscription.Dispose();
                    await messenger.PublishAsync(new MessageA());
                    Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
                    Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Exactly(2));
                }
            }

            [Test]
            public async Task OnMessageSent_HandledMessageIsIgnored()
            {
                using (var messenger = new Messenger())
                {
                    var listener1 = Mock.Of<IListener<MessageA>>();
                    Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>()))
                                       .Callback<MessageA>(m => m.Handled = true);
                    var listener2 = Mock.Of<IListener<MessageA>>();
                    messenger.Subscribe(listener1);
                    messenger.Subscribe(listener2);
                    await messenger.PublishAsync(new MessageA());
                    Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
                    Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Never);
                }
            }

            [Test]
            public async Task OnSubscribtionDisposedDuringHandle_RemovesListener()
            {
                using (var messenger = new Messenger())
                {
                    var listener1 = Mock.Of<IListener<MessageA>>();
                    var listener2 = Mock.Of<IListener<MessageA>>();
                    var subscription = messenger.Subscribe(listener1);
                    Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>())).Callback<MessageA>(m => subscription.Dispose());
                    messenger.Subscribe(listener2);
                    await messenger.PublishAsync(new MessageA());
                    await messenger.PublishAsync(new MessageA());
                    Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
                    Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Exactly(2));
                }
            }

            [Test]
            public void OnMessagesSent_ProcessedInOrder()
            {
                const int count = 1000;
                using (var messenger = new Messenger(count))
                {
                    var listener = Mock.Of<IListener<MessageA>>();
                    int nextId = 0;
                    Mock.Get(listener).Setup(l => l.Handle(It.IsAny<MessageA>()))
                                      .Callback<MessageA>(m =>
                                      {
                                          if (nextId++ != m.Id)
                                          {
                                              throw new InvalidOperationException($"Unordered message! Expected: {nextId - 1}, received {m.Id}");
                                          }
                                      });
                    messenger.Subscribe(listener);
                    var tasks = Enumerable.Range(0, count).Select(i => messenger.PublishAsync(new MessageA { Id = i })).ToArray();

                    Assert.IsTrue(tasks.All(t => t.IsCompleted));
                    Mock.Get(listener).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Exactly(count));
                }
            }

            //[Test]
            //public async Task OnMessengerDisposed_IgnoreNewMessages()
            //{
            //    var messenger = new Messenger();
            //    var listener = Mock.Of<IListener<MessageA>>();
            //    messenger.Subscribe(listener);
            //    await messenger.PublishAsync(new MessageA());
            //    messenger.Dispose();
            //    Assert.IsFalse(await messenger.PublishAsync(new MessageA()));
            //    Mock.Get(listener).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            //}
        }
    }

    public class MessageA : MessageBase
    {
        public int Id { get; set; }
    }

    public class MessageB : MessageBase
    {
    }

    public class MessageC : MessageBase
    {
    }
}
