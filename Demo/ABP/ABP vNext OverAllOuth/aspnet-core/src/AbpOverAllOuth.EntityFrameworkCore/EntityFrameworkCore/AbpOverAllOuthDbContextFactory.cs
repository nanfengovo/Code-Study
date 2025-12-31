using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AbpOverAllOuth.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class AbpOverAllOuthDbContextFactory : IDesignTimeDbContextFactory<AbpOverAllOuthDbContext>
{
    public AbpOverAllOuthDbContext CreateDbContext(string[] args)
    {
        AbpOverAllOuthEfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<AbpOverAllOuthDbContext>()
            .UseSqlServer(configuration.GetConnectionString("Default"));

        return new AbpOverAllOuthDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../AbpOverAllOuth.DbMigrator/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
