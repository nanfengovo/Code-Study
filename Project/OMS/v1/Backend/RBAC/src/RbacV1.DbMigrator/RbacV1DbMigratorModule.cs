using RbacV1.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace RbacV1.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(RbacV1EntityFrameworkCoreModule),
    typeof(RbacV1ApplicationContractsModule)
    )]
public class RbacV1DbMigratorModule : AbpModule
{
}
