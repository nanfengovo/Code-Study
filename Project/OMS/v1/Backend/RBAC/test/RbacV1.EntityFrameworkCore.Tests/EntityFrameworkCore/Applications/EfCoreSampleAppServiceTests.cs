using RbacV1.Samples;
using Xunit;

namespace RbacV1.EntityFrameworkCore.Applications;

[Collection(RbacV1TestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<RbacV1EntityFrameworkCoreTestModule>
{

}
