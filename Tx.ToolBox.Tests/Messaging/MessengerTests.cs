using System;
using System.Diagnostics;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Messaging;

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
        public void Publish_OnMessageSent_MessageHandledByApproperiateSubscribers()
        {
            var messenger = new Messenger();
            var listener1 = Mock.Of<IListener<MessageA>>();
            var listener2 = Mock.Of<IListener<MessageA>>();
            messenger.Subscribe(listener1);
            messenger.Subscribe(listener2);
            messenger.Publish(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
        }

        [Test]
        public void Publish_OnMessageSent_MessageIgnoredByOtherSubscribers1()
        {
            var messenger = new Messenger();
            var listener1 = Mock.Of<IListener<MessageA>>();
            var listener2 = Mock.Of<IListener<MessageB>>();
            messenger.Subscribe(listener1);
            messenger.Subscribe(listener2);
            messenger.Publish(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageB>()), Times.Never);
        }

        [Test]
        public void Publish_OnMessageSent_MessageIgnoredByOtherSubscribers2()
        {
            var messenger = new Messenger();
            var listener1 = Mock.Of<IListener<MessageA>>();
            messenger.Subscribe(listener1);
            messenger.Publish(new MessageB());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Never);
        }

        [Test]
        public void Publish_OnMessageSent_MessageIgnoredByDisposedListeners()
        {
            var messenger = new Messenger();
            var listener1 = Mock.Of<IListener<MessageA>>();
            var listener2 = Mock.Of<IListener<MessageA>>();
            var subscription = messenger.Subscribe(listener1);
            messenger.Subscribe(listener2);
            messenger.Publish(new MessageA());
            subscription.Dispose();
            messenger.Publish(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Exactly(2));
        }

        [Test]
        public void Publish_OnMessageSent_HandledMessageIsIgnored()
        {
            var messenger = new Messenger();
            var listener1 = Mock.Of<IListener<MessageA>>();
            Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>()))
                               .Callback<MessageA>(m => m.Handled = true);
            var listener2 = Mock.Of<IListener<MessageA>>();
            messenger.Subscribe(listener1);
            messenger.Subscribe(listener2);
            messenger.Publish(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Never);
        }
    }
}
