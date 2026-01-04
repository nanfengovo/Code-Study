using Xunit;

namespace RbacV1.EntityFrameworkCore;

[CollectionDefinition(RbacV1TestConsts.CollectionDefinitionName)]
public class RbacV1EntityFrameworkCoreCollection : ICollectionFixture<RbacV1EntityFrameworkCoreFixture>
{

}
