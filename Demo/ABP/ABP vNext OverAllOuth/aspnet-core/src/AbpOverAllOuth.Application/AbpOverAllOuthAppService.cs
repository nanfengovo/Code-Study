using System;
using System.Collections.Generic;
using System.Text;
using AbpOverAllOuth.Localization;
using Volo.Abp.Application.Services;

namespace AbpOverAllOuth;

/* Inherit your application services from this class.
 */
public abstract class AbpOverAllOuthAppService : ApplicationService
{
    protected AbpOverAllOuthAppService()
    {
        LocalizationResource = typeof(AbpOverAllOuthResource);
    }
}
