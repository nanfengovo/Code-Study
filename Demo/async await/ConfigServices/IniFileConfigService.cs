using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigServices
{
    public class IniFileConfigService : IConfigService
    {
        public string FilePath { get; set; }

        public string GetValue(string key)
        {
            var kv = File.ReadAllLines(FilePath).Select(s => s.Split('=')).Select(strs => new { Name = strs[0], Value = strs[1] }).SingleOrDefault(kv => kv.Name == key);
            if(kv != null)
            {
                return kv.Value;
            }
            else
            {
                return null;
            }
        }
    }
}
