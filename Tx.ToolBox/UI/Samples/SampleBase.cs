using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Castle.Windsor;
using Castle.MicroKernel.Registration;

namespace Tx.ToolBox.UI.Samples
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
        public Visual View { get; protected set; }

        public void Load(IWindsorContainer appContainer)
        {
            _container = SampleBootstrap.CreateDefaultContainer();
            appContainer.AddChildContainer(_container);
            var installers = CreateInstallers().ToArray();
            _container.Install(installers);
            View = _container.Resolve<Visual>();
        }

        public void Unload()
        {
            _container.Dispose();
        }

        protected abstract IEnumerable<IWindsorInstaller> CreateInstallers();

        private IWindsorContainer _container;
    }
}
