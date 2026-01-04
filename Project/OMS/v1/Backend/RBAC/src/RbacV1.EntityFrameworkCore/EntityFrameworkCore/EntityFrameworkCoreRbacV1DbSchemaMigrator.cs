using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RbacV1.Data;
using Volo.Abp.DependencyInjection;

namespace RbacV1.EntityFrameworkCore;

public class EntityFrameworkCoreRbacV1DbSchemaMigrator
    : IRbacV1DbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreRbacV1DbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the RbacV1DbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<RbacV1DbContext>()
            .Database
            .MigrateAsync();
    }
}
