using NUnit.Framework;
using Tx.ToolBox.Helpers;

namespace Tx.ToolBox.Tests.Helpers
{
    [TestFixture]
    class FlagTests
    {
        [Test]
        public void Ctor_OnCreated_IsNotSetByDefault()
        {
            var flag = new Flag();
            Assert.IsFalse(flag.IsSet);
        }

        [Test]
        public void SetTemporary_OnCall_IsSetReturnsTrue()
        {
            var flag = new Flag();
            flag.Set();
            Assert.IsTrue(flag.IsSet);
        }

        [Test]
        public void SetTemporary_OnDisposed_IsSetReturnsFalse()
        {
            var flag = new Flag();
            flag.Set().Dispose();
            Assert.IsFalse(flag.IsSet);
        }

        [Test]
        public void SetTemporary_OnDisposedMultipleTimes_UnsetsOnce()
        {
            var flag = new Flag();
            var handle1 = flag.Set();
            var handle2 = flag.Set();
            handle1.Dispose();
            handle1.Dispose();
            Assert.IsTrue(flag.IsSet);
        }

        [Test]
        public void SetPermanently_OnCall_IsSetReturnsTrue()
        {
            var flag = new Flag();
            flag.SetPermanently();
            Assert.IsTrue(flag.IsSet);
        }

        [Test]
        public void SetPermanently_CanNotBeUnset()
        {
            var flag = new Flag();
            var handle1 = flag.Set();
            flag.SetPermanently();
            handle1.Dispose();
            Assert.IsTrue(flag.IsSet);
            handle1.Dispose();
            Assert.IsTrue(flag.IsSet);
        }

        [TestFixture]
        public class FacadeTests
        {
            [Test]
            public void OnNestedUsings_FlagResetsOnLastUsing()
            {
                var flag = new Flag();
                using (flag.Set())
                {
                    Assert.IsTrue(flag.IsSet);
                    using (flag.Set())
                    {
                        Assert.IsTrue(flag.IsSet);
                        using (flag.Set())
                        {
                            Assert.IsTrue(flag.IsSet);
                        }
                        Assert.IsTrue(flag.IsSet);
                    }
                    Assert.IsTrue(flag.IsSet);
                }
                Assert.IsFalse(flag.IsSet);
            }
        }
    }
}
