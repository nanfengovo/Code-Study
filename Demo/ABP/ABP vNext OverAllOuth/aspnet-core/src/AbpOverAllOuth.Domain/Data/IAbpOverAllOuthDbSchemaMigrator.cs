using System.Threading.Tasks;

namespace AbpOverAllOuth.Data;

public interface IAbpOverAllOuthDbSchemaMigrator
{
    Task MigrateAsync();
}
