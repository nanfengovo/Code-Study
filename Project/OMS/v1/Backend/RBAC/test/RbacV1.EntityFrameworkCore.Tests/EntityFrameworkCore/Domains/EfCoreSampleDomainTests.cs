using RbacV1.Samples;
using Xunit;

namespace RbacV1.EntityFrameworkCore.Domains;

[Collection(RbacV1TestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<RbacV1EntityFrameworkCoreTestModule>
{

}
