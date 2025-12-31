using Volo.Abp.Modularity;

namespace AbpOverAllOuth;

public abstract class AbpOverAllOuthApplicationTestBase<TStartupModule> : AbpOverAllOuthTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
