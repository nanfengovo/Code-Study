using ConfigServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IniFileConfigExtensions
    {
        public static void AddIniFileConfig(this IServiceCollection services,string filePath)
        {
            services.AddScoped(typeof(IConfigService), s => new IniFileConfigService { FilePath = "mail.ini" });
        }
    }
}
