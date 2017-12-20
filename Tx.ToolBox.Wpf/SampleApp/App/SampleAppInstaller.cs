﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tx.ToolBox.Messaging;
using Tx.ToolBox.Wpf.SampleApp.Log;
using Tx.ToolBox.Wpf.ToolBar;
using Tx.ToolBox.Wpf.Windsor;
using Tx.ToolBox.Wpf.SampleApp.Samples;
using SampleListView = Tx.ToolBox.Wpf.SampleApp.Samples.SampleListView;

namespace Tx.ToolBox.Wpf.SampleApp.App
{
    class SampleAppInstaller : IWindsorInstaller
    {
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
            container.Register(Component.For<SampleAppWindow>()
                                        .OnCreate(w => w.Title = AppTitle),
                               Component.For<SampleApplication>());
        }
    }
}
