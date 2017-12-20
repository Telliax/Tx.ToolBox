using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Tx.ToolBox.Wpf.SampleApp
{
    public abstract class SampleBase : ISample
    {
        protected SampleBase()
        {
            Name = GetType().Name;
            Description = String.Empty;
        }

        public string Name { get; protected set; }
        public string Description { get; protected set; }

        public virtual FrameworkElement ResolveView()
        {
            return _container.Resolve<FrameworkElement>();
        }

        public void Load(IWindsorContainer appContainer)
        {
            _container = SampleBootstrap.CreateDefaultContainer();
            appContainer.AddChildContainer(_container);
            var installers = CreateInstallers().ToArray();
            _container.Install(installers);
        }

        public void Unload()
        {
            _container.Dispose();
        }

        protected abstract IEnumerable<IWindsorInstaller> CreateInstallers();

        private IWindsorContainer _container;
    }
}
