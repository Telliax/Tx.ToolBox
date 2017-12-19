using Castle.MicroKernel;
using Castle.Core.Configuration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;

namespace Tx.ToolBox.Windsor
{
    /// <summary>
    /// Facility that adds default CollectionResolver subresolver to container.
    /// </summary>
    public class CollectionFacility : IFacility
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
