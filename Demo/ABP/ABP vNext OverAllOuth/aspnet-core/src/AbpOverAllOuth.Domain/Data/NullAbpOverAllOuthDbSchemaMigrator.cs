using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace AbpOverAllOuth.Data;

/* This is used if database provider does't define
 * IAbpOverAllOuthDbSchemaMigrator implementation.
 */
public class NullAbpOverAllOuthDbSchemaMigrator : IAbpOverAllOuthDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
