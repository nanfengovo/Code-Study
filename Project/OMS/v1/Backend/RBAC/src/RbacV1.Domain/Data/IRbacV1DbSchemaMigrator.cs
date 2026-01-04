using System.Threading.Tasks;

namespace RbacV1.Data;

public interface IRbacV1DbSchemaMigrator
{
    Task MigrateAsync();
}
