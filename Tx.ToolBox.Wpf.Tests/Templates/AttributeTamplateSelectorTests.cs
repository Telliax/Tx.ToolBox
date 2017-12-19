using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using NUnit.Framework;
using Tx.ToolBox.Wpf.Templates;

namespace Tx.ToolBox.Wpf.Tests.Templates
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class AttributeTamplateSelectorTests
    {
        [SetUp]
        public void Setup()
        {
            _selector = new AttributeTemplateSelector();
        }

        [Test]
        public void SelectTemplate_ForNonPublicViews_Throws()
        {
            Assert.Throws<NotSupportedException>(() => _selector.SelectTemplate(new ViewModel1(), new ContentControl()));
        }

        [Test]
        public void SelectTemplate_ForPublicViews_Works()
        {
            var template = _selector.SelectTemplate(new ViewModel2(), new ContentControl());
            var content = template.LoadContent();
            Assert.AreEqual(content.GetType(), typeof(View2));
        }

        [Test]
        public void SelectTemplate_ForNestedAttributes_ReturnsTopmost()
        {
            var template = _selector.SelectTemplate(new ViewModel3(), new ContentControl());
            var content = template.LoadContent();
            Assert.AreEqual(content.GetType(), typeof(View2));
        }

        [Test]
        public void SelectTemplate_WithNoAttributes_FallsBackToDefault()
        {
            var template = _selector.SelectTemplate(new ViewModel4(), new ContentControl());
            Assert.IsNull(template);
        }

        private AttributeTemplateSelector _selector;
    }

    [Template(typeof(View1))]
    class ViewModel1 { }
    [Template(typeof(View2))]
    class ViewModel2 { }
    [Template(typeof(View2))]
    class ViewModel3 : ViewModel1 { }

    class ViewModel4 {}

    class View1 : FrameworkElement { }
    public class View2 : FrameworkElement { }
}
