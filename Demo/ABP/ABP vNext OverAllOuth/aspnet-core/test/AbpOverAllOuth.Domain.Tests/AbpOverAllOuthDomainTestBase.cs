using Volo.Abp.Modularity;

namespace AbpOverAllOuth;

/* Inherit from this class for your domain layer tests. */
public abstract class AbpOverAllOuthDomainTestBase<TStartupModule> : AbpOverAllOuthTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
