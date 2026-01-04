using Volo.Abp.Modularity;

namespace RbacV1;

public abstract class RbacV1ApplicationTestBase<TStartupModule> : RbacV1TestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
