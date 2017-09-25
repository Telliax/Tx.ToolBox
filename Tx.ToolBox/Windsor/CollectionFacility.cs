using Castle.MicroKernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Configuration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Tx.ToolBox.Windsor
{
    /// <summary>
    /// Facility that adds default CollectionResolver subresolver to container.
    /// </summary>
    class CollectionFacility : IFacility
    {
        public void Init(IKernel kernel, IConfiguration facilityConfig)
        {
            kernel.Resolver.AddSubResolver(new CollectionResolver(kernel));
        }

        public void Terminate()
        {
        }
    }
}
