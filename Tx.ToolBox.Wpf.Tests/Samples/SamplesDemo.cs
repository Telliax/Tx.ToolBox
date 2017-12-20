using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using NUnit.Framework;
using Tx.ToolBox.Wpf.SampleApp;

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
                boot.AddSamples(new Sample(1), new Sample(2), new Sample(3));
                boot.Run();
            }
        }

        private class Sample : SampleBase
        {
            public Sample(int id)
            {
                Name = "Sample #" + id;
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
                Thread.Sleep(5000);
                container.Register(Component.For<FrameworkElement>().UsingFactoryMethod(() => new TextBlock { Text = Name }));
            }
        }
    }




}
