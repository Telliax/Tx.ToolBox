using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using Tx.ToolBox.Windsor;

namespace Tx.ToolBox.UI.Samples
{
    class SampleBootstrap : IDisposable
    {
        public SampleBootstrap(string appTitle)
        {
            _container = CreateDefaultContainer();
            _installer.AppTitle = appTitle;
        }

        public void AddSample(ISample sample)
        {
            _installer.Samples.Add(sample);
        }

        public void Run()
        {
            _container.Install(_installer);
            _container.Resolve<SampleApp>().Run();
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public static IWindsorContainer CreateDefaultContainer()
        {
            var res = new WindsorContainer();
            res.AddFacility<MessengerFacility>();
            res.AddFacility<CollectionFacility>();
            return res;
        }

        private readonly IWindsorContainer _container;
        private readonly SampleAppInstaller _installer = new SampleAppInstaller();
    }
}
