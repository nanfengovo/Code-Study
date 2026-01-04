using System;
using System.Collections.Generic;
using System.Text;
using RbacV1.Localization;
using Volo.Abp.Application.Services;

namespace RbacV1;

/* Inherit your application services from this class.
 */
public abstract class RbacV1AppService : ApplicationService
{
    protected RbacV1AppService()
    {
        LocalizationResource = typeof(RbacV1Resource);
    }
}
