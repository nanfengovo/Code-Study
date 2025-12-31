using AbpOverAllOuth.Samples;
using Xunit;

namespace AbpOverAllOuth.EntityFrameworkCore.Domains;

[Collection(AbpOverAllOuthTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<AbpOverAllOuthEntityFrameworkCoreTestModule>
{

}
