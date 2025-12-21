using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Xml;

namespace 开发自己的配置提供者
{
    public class FxConfigProvider:FileConfigurationProvider
    {
        public override void Load(Stream steam)
        {
            var data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(steam);
            var csNodes = xmlDoc.SelectNodes("/configuration/connectionStrings/add");
            foreach (XmlNode node in csNodes.Cast<XmlNode>())
            {
                var name = node.Attributes["name"].Value;
                var connStr = node.Attributes["connectionString"].Value;
                data.Add($"connectionStrings:{name}", connStr);
            }
        }
    }
}
