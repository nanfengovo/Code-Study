using Volo.Abp.Modularity;

namespace AbpOverAllOuth;

[DependsOn(
    typeof(AbpOverAllOuthApplicationModule),
    typeof(AbpOverAllOuthDomainTestModule)
)]
public class AbpOverAllOuthApplicationTestModule : AbpModule
{

}
