using Volo.Abp.Modularity;

namespace AbpOverAllOuth;

[DependsOn(
    typeof(AbpOverAllOuthDomainModule),
    typeof(AbpOverAllOuthTestBaseModule)
)]
public class AbpOverAllOuthDomainTestModule : AbpModule
{

}
