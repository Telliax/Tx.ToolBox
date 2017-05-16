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
    }
}
