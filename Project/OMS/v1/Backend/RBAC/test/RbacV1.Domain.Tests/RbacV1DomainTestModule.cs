using Volo.Abp.Modularity;

namespace RbacV1;

[DependsOn(
    typeof(RbacV1DomainModule),
    typeof(RbacV1TestBaseModule)
)]
public class RbacV1DomainTestModule : AbpModule
{

}
