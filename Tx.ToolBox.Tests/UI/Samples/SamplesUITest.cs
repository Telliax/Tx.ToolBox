using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NUnit.Framework;
using Tx.ToolBox.UI.Samples;

namespace Tx.ToolBox.Tests.UI.Samples
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class SamplesUITest
    {
        [Test]
        public void UITest()
        {
            using (var boot = new SampleBootstrap("Test app"))
            {
                boot.AddSample(new Sample());
                boot.AddSample(new Sample());
                boot.AddSample(new Sample());
                boot.Run();
            }
        }

        private class Sample : SampleBase
        {
            public Sample()
            {
                Name = "Sample #" + Guid.NewGuid();
                Description = "Desc: " + Name;
            }

            protected override IEnumerable<IWindsorInstaller> CreateInstallers()
            {
                yield return new Installer { Name = Name };
            }
        }

        private class Installer : IWindsorInstaller
        {
            public string Name { get; set; }

            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<Visual>().UsingFactoryMethod(() => new TextBlock { Text = Name }));
            }
        }
    }




}
