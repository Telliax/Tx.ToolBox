using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Tx.ToolBox.Storage;

namespace Tx.ToolBox.Tests.Storage
{
    [TestFixture]
    class XmlStorageTests
    {
        [Test]
        public void Ctor_NullArgument_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => new XmlStorage(null, true));
            Assert.Throws<ArgumentNullException>(() => new XmlStorage(null));
        }

        public class FacadeTests
        {
            [Test]
            public void Get_AfterLoad_Works()
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tx.ToolBox.Tests.Storage.Test1.xml");
                var item = new TestItem { Name="Boris", Age = 11};
                using (var store = new XmlStorage(stream))
                {
                    store.Load();
                    var item2 = store.Get<TestItem>("1");
                    Assert.AreEqual(item2.Name, item.Name);
                    Assert.AreEqual(item2.Age, item.Age);
                }
            }

            [Test]
            public void Get_AfterLoad_WorksWithCustomSchema()
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Tx.ToolBox.Tests.Storage.Test2.xml");
                var item = new TestItem { Name = "Boris", Age = 11 };
                using (var store = new XmlStorage(stream)
                {
                    Schema = new XmlSchema
                    {
                        IdAttribute = "SettingsId",
                        RootTag = "SettingsContainer",
                        ItemTag = "Settings",
                        TypeAttribute = "SettingsType"
                    }
                })
                {
                    store.Load();
                    var item2 = store.Get<TestItem>("1");
                    Assert.AreEqual(item2.Name, item.Name);
                    Assert.AreEqual(item2.Age, item.Age);
                }
            }

            [Test]
            public void Set_ItemsCanBeRetreaved()
            {
                using (var store = new XmlStorage(new MemoryStream()))
                {
                    var item1 = new TestItem();
                    var item2 = new TestItem();
                    var item3 = new TestItem();
                    store.Set(item1);
                    store.Set(item2, "2");
                    store.Set(item3, "3");
                    Assert.AreSame(item3, store.Get<TestItem>("3"));
                    Assert.AreSame(item1, store.Get<TestItem>());
                    Assert.AreSame(item2, store.Get<TestItem>("2"));
                }
            }

            [Test]
            public void Set_CanOverride()
            {
                using (var store = new XmlStorage(new MemoryStream()))
                {
                    var item1 = new TestItem();
                    var item2 = new TestItem();
                    var item3 = new TestItem();
                    var item4 = new TestItem();
                    var item5 = new TestItem();
                    store.Set(item2, "2");
                    store.Set(item1);
                    store.Set(item5);
                    store.Set(item3, "3");
                    store.Set(item4, "2");
                    Assert.AreSame(item3, store.Get<TestItem>("3"));
                    Assert.AreSame(item5, store.Get<TestItem>());
                    Assert.AreSame(item4, store.Get<TestItem>("2"));
                }
            }

            [Test]
            public void Save_SavedSettings_CanBeLoaded()
            {
                var stream = new MemoryStream();
                var item1 = new TestItem { Name = "a", Age = 1};
                var item2 = new TestItem2{Address = "b", Coordinate = 1.0};
                using (var store1 = new XmlStorage(stream, false))
                {
                    store1.Set(item1);
                    store1.Set(item2, "2");
                    store1.Save();
                }

                using (var store2 = new XmlStorage(stream, true))
                {
                    store2.Load();
                    var item3 = store2.Get<TestItem>();
                    var item4 = store2.Get<TestItem2>("2");

                    Assert.AreEqual(item1.Name, item3.Name);
                    Assert.AreEqual(item1.Age, item3.Age);
                    Assert.AreEqual(item2.Address, item4.Address);
                    Assert.AreEqual(item2.Coordinate, item4.Coordinate);
                }
            }

            public void CopyTo
        }
    }

    public class TestItem
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class TestItem2
    {
        public string Address { get; set; }
        public double Coordinate { get; set; }
    }
}
