using AbpOverAllOuth.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbpOverAllOuth
{
    public class TestPermissionAppService:AbpOverAllOuthAppService
    {
        [Authorize(AbpOverAllOuthPermissions.Create)]
        public string CreatePermissionTest()
        {
            return "Create Permission Test";
        }

        public string Test()
        {
            return "可以访问";
        }
    }
}
