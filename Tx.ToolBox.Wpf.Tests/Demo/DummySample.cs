using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tx.ToolBox.Wpf.SampleApp;
using Tx.ToolBox.Wpf.Tests.Demo.Tools;

namespace Tx.ToolBox.Wpf.Tests.Demo
{
    class DummySample : SampleBase
    {
        public DummySample()
        {
            Name = "Dummy";
            
        }

        protected override IEnumerable<IWindsorInstaller> CreateInstallers()
        {
            Thread.Sleep(3000);
            yield return new Installer();
        }

        private class Installer : IWindsorInstaller
        {
            public void Install(IWindsorContainer container, IConfigurationStore store)
            {
                container.Register(Component.For<FrameworkElement, Control>());
            }
        }

    }
}
