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
        public static IWindsorContainer Register<T>(this IWindsorContainer container) 
            where T : class
        {
            return container.Register(Component.For<T>());
        }
    }
}
