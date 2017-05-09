using System;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Messaging;

namespace Tx.ToolBox.Tests.Messaging
{
    [TestFixture]
    class ListenerCollectionTests
    {
        [Test]
        public void Add_OnInvalidListener_Throws()
        {
            var listener = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageB>();
            Assert.Throws<InvalidOperationException>(() => collection.Add(listener));
        }

        [Test]
        public void Handle_OnInvalidMessage_Throws()
        {
            var listener = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageA>();
            collection.Add(listener);
            Assert.Throws<InvalidOperationException>(() => collection.Handle(new MessageB()));
        }

        [Test]
        public void Handle_OnValidMessage_CallsListenerHandle()
        {
            var listener1 = Mock.Of<IListener<MessageA>>();
            var listener2 = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageA>();
            collection.Add(listener1);
            collection.Add(listener2);
            collection.Handle(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
        }

        [Test]
        public void Handle_OnHandledMessage_Stops()
        {
            var listener1 = Mock.Of<IListener<MessageA>>();
            Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>())).Callback<MessageA>(m => m.Handled = true);
            var listener2 = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageA>();
            collection.Add(listener1);
            collection.Add(listener2);
            collection.Handle(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Never);
        }

        [Test]
        public void Handle_OnSubscribtionDisposed_RemovesListener()
        {
            var listener1 = Mock.Of<IListener<MessageA>>();
            Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>())).Callback<MessageA>(m => m.Handled = true);
            var listener2 = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageA>();
            var handle = collection.Add(listener1);
            collection.Add(listener2);
            handle.Dispose();
            collection.Handle(new MessageA());
            Mock.Get(listener1).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Never);
            Mock.Get(listener2).Verify(l => l.Handle(It.IsAny<MessageA>()), Times.Once);
        }

        [Test]
        public void Handle_OnSubscribtionDisposedDuringHandle_RemovesListener()
        {
            var listener1 = Mock.Of<IListener<MessageA>>();
            var listener2 = Mock.Of<IListener<MessageA>>();
            var collection = new ListenerCollection<MessageA>();
            var handle = collection.Add(listener1);
            collection.Add(listener2);
            Mock.Get(listener1).Setup(l => l.Handle(It.IsAny<MessageA>())).Callback<MessageA>(m => handle.Dispose());
            collection.Handle(new MessageA());
            collection.Handle(new MessageA());
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
}
