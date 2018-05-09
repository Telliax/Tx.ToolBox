using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Tx.ToolBox.Windsor
{
    public static class WindsorEx
    {
        public static IWindsorContainer Register<TComponent>(this IWindsorContainer container, string id = null) 
            where TComponent : class
        {
            return container.Register(Component.For<TComponent>().MaybeNamed(id));
        }

        public static IWindsorContainer RegisterService<TService, TImpl>(this IWindsorContainer container, string id = null)
            where TService : class
            where TImpl : class, TService
        {
            return container.Register(Component.For<TService>().ImplementedBy<TImpl>().MaybeNamed(id));
        }

        private static ComponentRegistration<T> MaybeNamed<T>(this ComponentRegistration<T> component, string id) 
            where T : class
        {
            if (id != null)
            {
                component = component.Named(id);
            }
            return component;
        }
    }
}
