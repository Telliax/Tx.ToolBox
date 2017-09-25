using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tx.ToolBox.Messaging;
using System.Windows.Threading;

namespace Tx.ToolBox.UI.Samples
{
    class SampleAppInstaller : IWindsorInstaller
    {
        public string AppTitle { get; set; }
        public List<ISample> Samples { get; } = new List<ISample>();

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Samples.Select(s => Component.For<ISample>().Instance(s).Named(s.Name)).ToArray());
            container.Register(Component.For<Dispatcher>().Instance(Dispatcher.CurrentDispatcher),
                               Component.For<IMessenger>().ImplementedBy<Messenger>(),
                               Component.For<IEventLog, EventLog>(),
                               Component.For<SampleManager>().DependsOn(Dependency.OnValue<IWindsorContainer>(container)),
                               Component.For<SampleAppWindowModel>(),
                               Component.For<SampleAppWindow>().OnCreate(w => w.Title = AppTitle),
                               Component.For<SampleApp>());
        }
    }
}
