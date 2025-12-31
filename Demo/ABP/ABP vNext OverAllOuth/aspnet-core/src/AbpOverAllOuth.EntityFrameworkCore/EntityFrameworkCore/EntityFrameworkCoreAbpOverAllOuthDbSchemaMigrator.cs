using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AbpOverAllOuth.Data;
using Volo.Abp.DependencyInjection;

namespace AbpOverAllOuth.EntityFrameworkCore;

public class EntityFrameworkCoreAbpOverAllOuthDbSchemaMigrator
    : IAbpOverAllOuthDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreAbpOverAllOuthDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the AbpOverAllOuthDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<AbpOverAllOuthDbContext>()
            .Database
            .MigrateAsync();
    }
}
