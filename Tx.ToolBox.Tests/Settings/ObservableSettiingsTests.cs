using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Settings;
using Tx.ToolBox.Tests.Messaging;

namespace Tx.ToolBox.Tests.Settings
{
    [TestFixture]
    class ObservableSettiingsTests
    {
        [Test]
        public void Ctor_OnInvalidId_Throws()
        {
            Assert.Throws<ArgumentException>(() => new ObservableSettings<object>(null));
            Assert.Throws<ArgumentException>(() => new ObservableSettings<object>(String.Empty));
            Assert.Throws<ArgumentException>(() => new ObservableSettings<object>("  "));
        }

        [Test]
        public void Subscribe_OnSubscriberAdded_CurrentValueReceived()
        {
            object res = null;
            var value = new object();
            using (var settings = new ObservableSettings<object>(value, "id"))
            using (settings.Subscribe(val => res = val))
            {
                Assert.AreEqual(value, res);
            }
        }

        [Test]
        public void Set_OnSettingsChanged_NewValueReceived()
        {
            object res = null;
            var value = new object();
            var value2 = new object();
            using (var settings = new ObservableSettings<object>(value, "id"))
            using (settings.Subscribe(val => res = val))
            {
                settings.Set(value2);
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
            using (var settings = new ObservableSettings<object>(value, "id"))
            {
                using (settings.Subscribe(val => res = val))
                {
                    settings.Set(value2);
                }
                settings.Set(value3);
                Assert.AreEqual(value2, res);
            }
        }

        [Test]
        public void Dispose_OnSettingsDisposed_Completed()
        {
            var value = new object();
            var settings = new ObservableSettings<object>(value, "id");
            var completed = false;
            using (settings.Subscribe(val => { }, () => completed = true))
            {
                settings.Dispose();
                Assert.IsTrue(completed);
            }
        }

        [Test]
        public void Copy_OnCopy_ValueSetToOtherContainer()
        {
            var storage = Mock.Of<ISettingsStorage>();
            string resultId = null;
            object resultSettings = null;
            Mock.Get(storage)
                .Setup(c => c.SetSettings(It.IsAny<object>(), It.IsAny<string>()))
                .Callback<object, string>((s, i) =>
                {
                    resultId = i;
                    resultSettings = s;
                });
            var value = new object();
            using (var settings = new ObservableSettings<object>(value, "id"))
            {
                settings.CopyTo(storage);
                Assert.AreEqual("id", resultId);
                Assert.AreEqual(value, resultSettings);
            }
        }
    }
}
