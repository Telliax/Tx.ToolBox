using System;
using Castle.Windsor;
using Tx.ToolBox.Windsor;
using Castle.MicroKernel.Registration;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using FontAwesome.WPF;
using Tx.ToolBox.Wpf.SampleApp.App;
using Tx.ToolBox.Wpf.SampleApp.App.List;

namespace Tx.ToolBox.Wpf.SampleApp
{
    public class SampleBootstrap : IDisposable
    {
        public SampleBootstrap(AppSettings settings)
        {
            _container = CreateDefaultContainer();
            _sampleInstaller = new SampleAppInstaller(settings);
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
            _container.Resolve<SampleListViewModel>().Dispose();
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

    public class AppSettings
    {
        public string Title { get; set; } = "MyApp";
        public bool FullScreen { get; set; } = true;
        public Size Size { get; set; } = new Size(800, 600);
        public ImageSource Icon { get; set; }
    }

}
