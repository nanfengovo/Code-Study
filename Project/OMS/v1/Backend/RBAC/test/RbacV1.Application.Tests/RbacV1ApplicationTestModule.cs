using Volo.Abp.Modularity;

namespace RbacV1;

[DependsOn(
    typeof(RbacV1ApplicationModule),
    typeof(RbacV1DomainTestModule)
)]
public class RbacV1ApplicationTestModule : AbpModule
{

}
