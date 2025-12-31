using AbpOverAllOuth.Samples;
using Xunit;

namespace AbpOverAllOuth.EntityFrameworkCore.Applications;

[Collection(AbpOverAllOuthTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<AbpOverAllOuthEntityFrameworkCoreTestModule>
{

}
