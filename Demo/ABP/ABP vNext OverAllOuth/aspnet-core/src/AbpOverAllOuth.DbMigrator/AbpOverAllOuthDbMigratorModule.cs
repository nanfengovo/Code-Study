using AbpOverAllOuth.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace AbpOverAllOuth.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpOverAllOuthEntityFrameworkCoreModule),
    typeof(AbpOverAllOuthApplicationContractsModule)
    )]
public class AbpOverAllOuthDbMigratorModule : AbpModule
{
}
