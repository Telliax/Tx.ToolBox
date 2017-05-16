using System;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Messaging;
using System.Threading.Tasks;

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
            var messenger = new Messenger();
            IListener<MessageA> listener = null;
            Assert.Throws<ArgumentNullException>(() => messenger.Subscribe(listener));
        }

        [Test]
        public void Subscribe_OnNotListener_Throws()
        {
            var messenger = new Messenger();
            var listener = new object();
            Assert.Throws<InvalidOperationException>(() => messenger.Subscribe(listener));
        }

        [Test]
        public void Subscribe_OnNullDelegate_Throws()
        {
            var messenger = new Messenger();
            Action<MessageA> listener = null;
            Assert.Throws<ArgumentNullException>(() => messenger.Subscribe(listener));
        }

        [Test]
        public void Publish_OnNullMessage_Throws()
        {
            var messenger = new Messenger();
            Assert.Throws<AggregateException>(() => messenger.PublishAsync(null).Wait());
        }

        [Test]
        public async Task FacadeTest_OnMessageSent_MessageHandledByApproperiateSubscribers()
        {
            var messenger = new Messenger();
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

        [Test]
        public async Task FacadeTest_OnMessageSent_MessageIgnoredByDisposedListeners()
        {
            var messenger = new Messenger();
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

        [Test]
        public async Task FacadeTest_OnMessageSent_HandledMessageIsIgnored()
        {
            var messenger = new Messenger();
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

        [Test]
        public async Task FacadeTest_OnSubscribtionDisposedDuringHandle_RemovesListener()
        {
            var messenger = new Messenger();
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

    public class MessageA : MessageBase
    {
    }

    public class MessageB : MessageBase
    {
    }

    public class MessageC : MessageBase
    {
    }
}
