using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigServices
{
    public class ConfigService : IConfigService
    {
        public string GetValue(string key)
        {
            return Environment.GetEnvironmentVariable(key) ?? string.Empty;
        }
    }
}
