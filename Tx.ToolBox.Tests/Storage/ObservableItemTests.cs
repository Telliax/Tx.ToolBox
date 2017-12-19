using System;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Storage;

namespace Tx.ToolBox.Tests.Storage
{
    [TestFixture]
    class ObservableItemTests
    {
        [Test]
        public void Ctor_OnInvalidId_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ObservableItem<object>(null));
            Assert.Throws<ArgumentException>(() => new ObservableItem<object>(String.Empty));
            Assert.Throws<ArgumentException>(() => new ObservableItem<object>("  "));
        }

        [Test]
        public void Subscribe_OnSubscriberAdded_CurrentValueReceived()
        {
            object res = null;
            var value = new object();
            using (var settings = new ObservableItem<object>(value, "id"))
            using (settings.Subscribe(val => res = val))
            {
                Assert.AreEqual(value, res);
            }
        }

        [Test]
        public void Set_OnItemChanged_NewValueReceived()
        {
            object res = null;
            var value = new object();
            var value2 = new object();
            using (var item = new ObservableItem<object>(value, "id"))
            using (item.Subscribe(val => res = val))
            {
                item.Set(value2);
                Assert.AreEqual(value2, res);
            }
        }

        [Test]
        public void Set_OnUnsubscribe_NewValueIsNotReceived()
        {
            object res = null;
            var value = new object();
            var value2 = new object();
            var value3 = new object();
            using (var item = new ObservableItem<object>(value, "id"))
            {
                using (item.Subscribe(val => res = val))
                {
                    item.Set(value2);
                }
                item.Set(value3);
                Assert.AreEqual(value2, res);
            }
        }

        [Test]
        public void Dispose_OnItemDisposed_Completed()
        {
            var value = new object();
            var item = new ObservableItem<object>(value, "id");
            var completed = false;
            using (item.Subscribe(val => { }, () => completed = true))
            {
                item.Dispose();
                Assert.IsTrue(completed);
            }
        }

        [Test]
        public void Copy_OnCopy_ValueSetToOtherContainer()
        {
            var storage = Mock.Of<IStorage>();
            string resultId = null;
            object resultSettings = null;
            Mock.Get(storage)
                .Setup(c => c.Set(It.IsAny<object>(), It.IsAny<string>()))
                .Callback<object, string>((s, i) =>
                {
                    resultId = i;
                    resultSettings = s;
                });
            var value = new object();
            using (var item = new ObservableItem<object>(value, "id"))
            {
                item.CopyTo(storage);
                Assert.AreEqual("id", resultId);
                Assert.AreEqual(value, resultSettings);
            }
        }
    }
}
