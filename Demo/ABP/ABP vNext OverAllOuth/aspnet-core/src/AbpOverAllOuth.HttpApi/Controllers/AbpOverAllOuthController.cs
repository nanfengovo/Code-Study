using AbpOverAllOuth.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace AbpOverAllOuth.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class AbpOverAllOuthController : AbpControllerBase
{
    protected AbpOverAllOuthController()
    {
        LocalizationResource = typeof(AbpOverAllOuthResource);
    }
}
