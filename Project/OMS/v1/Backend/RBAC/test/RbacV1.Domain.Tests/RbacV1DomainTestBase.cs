using Volo.Abp.Modularity;

namespace RbacV1;

/* Inherit from this class for your domain layer tests. */
public abstract class RbacV1DomainTestBase<TStartupModule> : RbacV1TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
