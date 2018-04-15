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

        protected override void OnLoad(IWindsorContainer sampleContainer)
        {
            _toolBar = sampleContainer.Resolve<IToolBar>();
            _tools = sampleContainer.ResolveAll<ITool>();
            _toolBar.Setup()
                    .Add(_tools)
                    .Complete();
        }

        protected override void OnUnload(IWindsorContainer sampleContainer)
        {
            _toolBar.Setup()
                    .Remove(_tools)
                    .Complete();
        }

        private ITool[] _tools;
        private IToolBar _toolBar;
    }
}
