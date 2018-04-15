using System;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tx.ToolBox.Windsor;
using Tx.ToolBox.Wpf.Tools;

namespace Tx.ToolBox.Wpf.Tests.Demo.Tools
{
    class ToolBarInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.RegisterService<ITool, ImageButton>();
            container.RegisterService<ITool, TextButton>();
            container.RegisterService<ITool, ImageAndTextButton>();

            container.RegisterService<FrameworkElement, ToolBarSampleView>();
        }
    }
}