using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigServices
{
    public interface IConfigService
    {
        public string GetValue(string key);
    }
}
