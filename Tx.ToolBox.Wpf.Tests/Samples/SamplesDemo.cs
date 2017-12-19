using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NUnit.Framework;
using Tx.ToolBox.Wpf.SampleApp;
using Tx.ToolBox.Wpf.SampleApp.App;

namespace Tx.ToolBox.Wpf.Tests.Samples
{
    [TestFixture, Apartment(ApartmentState.STA)]
    class SamplesDemo
    {
        [Test]
        public void Run()
        {
            using (var boot = new SampleBootstrap("Test app"))
            {
                boot.AddSamples(new Sample(), new Sample(), new Sample());
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
                Thread.Sleep(2000);
                container.Register(Component.For<FrameworkElement>().UsingFactoryMethod(() => new TextBlock { Text = Name }));
            }
        }
    }




}
