using RbacV1.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace RbacV1.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class RbacV1Controller : AbpControllerBase
{
    protected RbacV1Controller()
    {
        LocalizationResource = typeof(RbacV1Resource);
    }
}
