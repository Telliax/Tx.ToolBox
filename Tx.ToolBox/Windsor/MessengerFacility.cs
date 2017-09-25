using Castle.MicroKernel;
using System;
using System.Linq;
using Castle.Core.Configuration;
using Castle.MicroKernel.ModelBuilder;
using Castle.Core;
using Tx.ToolBox.Messaging;

namespace Tx.ToolBox.Windsor
{
    /// <summary>
    /// Facility, that automatically subscribes components that implement IListener interface
    /// to IMessenger events upon creation and automatically unsubscribes them upon decomission. 
    /// </summary>
    public class MessengerFacility : IFacility
    {
        /// <summary>
        /// Stop message flow *before* container is disposed. Default = false;
        /// </summary>
        public bool DisposeMessengerOnTerminate { get; set; }

        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            _kernel = kernel;
            _kernel.ComponentModelBuilder.AddContributor(new MessengerContributor());
        }

        public void Terminate()
        {
            if (DisposeMessengerOnTerminate)
            {
                if (!_kernel.HasComponent(typeof(IMessenger))) return;
                var disposable = _kernel.Resolve<IMessenger>() as IDisposable;
                if (disposable == null) return;
                disposable.Dispose();
            }
        }

        private IKernel _kernel;

        private class MessengerContributor : IContributeComponentModelConstruction
        {
            public void ProcessModel(IKernel kernel, ComponentModel model)
            {
                var concern = new ListenerLifetimeConcern(kernel);
                model.Lifecycle.Add((ICommissionConcern)concern);
                model.Lifecycle.Add((IDecommissionConcern)concern);
            }
        }

        private class ListenerLifetimeConcern : ICommissionConcern, IDecommissionConcern
        {
            public ListenerLifetimeConcern(IKernel kernel)
            {
                _kernel = kernel;
            }

            void ICommissionConcern.Apply(ComponentModel model, object component)
            {
                var isListener = component.GetType()
                                       .GetInterfaces()
                                       .Where(i => i.IsGenericType)
                                       .Any(i => i.GetGenericTypeDefinition() == typeof(IListener<>));
                if (!isListener) return;
                _subscription = _kernel.Resolve<IMessenger>().Subscribe(component);
            }

            void IDecommissionConcern.Apply(ComponentModel model, object component)
            {
                if (_subscription == null) return;
                _subscription.Dispose();
            }

            private readonly IKernel _kernel;
            private IDisposable _subscription;
        }
    }
}
