using System;
using Castle.Windsor;
using Tx.ToolBox.Windsor;
using Castle.MicroKernel.Registration;
using System.Collections.Generic;
using Tx.ToolBox.Wpf.SampleApp.App;

namespace Tx.ToolBox.Wpf.SampleApp
{
    public class SampleBootstrap : IDisposable
    {
        public SampleBootstrap(string appTitle)
        {
            _container = CreateDefaultContainer();
            _sampleInstaller = new SampleAppInstaller();
            _sampleInstaller.AppTitle = appTitle;
            _installers = new List<IWindsorInstaller> { _sampleInstaller };
        }

        public void AddCustomInstallers(params IWindsorInstaller[] installers)
        {
            _installers.AddRange(installers);
        }

        public void AddSamples(params ISample[] samples)
        {
            _sampleInstaller.Samples.AddRange(samples);
        }

        public void Run()
        {
            _container.Install(_installers.ToArray());
            _container.Resolve<SampleApplication>().Run();
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
        private readonly SampleAppInstaller _sampleInstaller;
        private readonly List<IWindsorInstaller> _installers;
    }
}
