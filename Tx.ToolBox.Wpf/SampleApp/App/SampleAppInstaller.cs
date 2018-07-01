using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Threading;
using Tx.ToolBox.Wpf.SampleApp.App.Events;
using Tx.ToolBox.Wpf.SampleApp.App.List;
using Tx.ToolBox.Wpf.Windsor;
using Tx.ToolBox.Wpf.Tools;
using Tx.ToolBox.Wpf.SampleApp.App.Sample;
using EventLogView = Tx.ToolBox.Wpf.SampleApp.App.Events.EventLogView;

namespace Tx.ToolBox.Wpf.SampleApp.App
{
    class SampleAppInstaller : IWindsorInstaller
    {
        public SampleAppInstaller(AppSettings settings)
        {
            _settings = settings;
        }

        public string AppTitle { get; set; }
        public List<ISample> Samples { get; } = new List<ISample>();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Samples.Select(s => (IRegistration)Component.For<ISample>().Instance(s).Named(s.Name)).ToArray());

            container.Register(Component.For<Dispatcher>()
                                        .Instance(Dispatcher.CurrentDispatcher),
                               Component.For<IMessenger>()
                                        .ImplementedBy<Messenger>(),
                               Component.For<SampleListViewModel>()
                                        .DependsOn(Dependency.OnValue<IWindsorContainer>(container)),
                               Component.For<IToolBar, ToolBarViewModel>());
            container.RegisterView<ToolBarView, ToolBarViewModel>();
            container.RegisterView<EventLogView, EventLogViewModel>();
            container.RegisterView<SampleListView, SampleListViewModel>();
            container.RegisterView<SelectedSampleView, SampleListViewModel>();
            container.RegisterView<LoadingScreenView, SampleListViewModel>();
            container.Register(Component.For<SampleAppWindow>()
                                        .OnCreate(w =>
                                        {
                                            w.Title = _settings.Title;
                                            w.Width = _settings.Size.Width;
                                            w.Height = _settings.Size.Height;
                                            w.WindowState = _settings.FullScreen
                                                ? WindowState.Maximized
                                                : WindowState.Normal;
                                            w.Icon = _settings.Icon;
                                        }),
                               Component.For<SampleApplication>());
        }

        private readonly AppSettings _settings;
    }
}
