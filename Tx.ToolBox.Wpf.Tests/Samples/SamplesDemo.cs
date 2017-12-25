using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FontAwesome.WPF;
using NUnit.Framework;
using Tx.ToolBox.Wpf.SampleApp;
using Tx.ToolBox.Wpf.Tools;
using Tx.ToolBox.Wpf.Tools.Buttons;

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

            protected override void OnLoad(IWindsorContainer sampleContainer)
            {
                Thread.Sleep(5000);
                sampleContainer.Resolve<IToolBar>().Setup().Add(new Button()).Complete();
            }

            protected override void OnUnload(IWindsorContainer sampleContainer)
            {
                sampleContainer.Resolve<IToolBar>().Setup().Clear().Complete();
            }
        }

        private class Installer : IWindsorInstaller
        {
            public string Name { get; set; }

            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<FrameworkElement>().UsingFactoryMethod(() => new TextBlock { Text = Name }));
            }
        }

        private class Button : ButtonTool
        {
            public Button()
            {
                Image = ImageAwesome.CreateImageSource(FontAwesomeIcon.AddressBook, Brushes.Black);
            }

            protected override void Execute()
            {
                MessageBox.Show("Sup");
            }
        }

    }




}
