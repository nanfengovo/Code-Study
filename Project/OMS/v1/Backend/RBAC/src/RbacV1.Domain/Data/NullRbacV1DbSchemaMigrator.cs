using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace RbacV1.Data;

/* This is used if database provider does't define
 * IRbacV1DbSchemaMigrator implementation.
 */
public class NullRbacV1DbSchemaMigrator : IRbacV1DbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
