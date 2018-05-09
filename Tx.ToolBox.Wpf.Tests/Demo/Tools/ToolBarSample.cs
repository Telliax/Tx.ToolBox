using System.Collections.Generic;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Tx.ToolBox.Wpf.SampleApp;
using Tx.ToolBox.Wpf.Tools;

namespace Tx.ToolBox.Wpf.Tests.Demo.Tools
{
    class ToolBarSample : SampleBase
    {
        public ToolBarSample()
        {
            Name = "Toolbar Sample";
        }

        protected override IEnumerable<IWindsorInstaller> CreateInstallers()
        {
            yield return new ToolBarInstaller();
        }
    }
}
