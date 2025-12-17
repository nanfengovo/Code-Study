using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigDI
{
    public class TestController
    {
        private readonly IOptionsSnapshot<Config> _optConfig;

        public TestController(IOptionsSnapshot<Config> optConfig)
        {
            _optConfig = optConfig;
        }

        public void Test()
        {
            Console.WriteLine(_optConfig.Value.Age);
            Console.WriteLine("************************");
            Console.WriteLine(_optConfig.Value.Age);
        }

        public void Test1()
        {
            Console.WriteLine(_optConfig.Value.Age);
            Console.WriteLine("************************");
            Console.WriteLine(_optConfig.Value.Name);
            Console.WriteLine(_optConfig.Value.Proxy.Address);
            Console.WriteLine(_optConfig.Value.Proxy.Port);
            Console.WriteLine("************************");
            Console.WriteLine(string.Join(",", _optConfig.Value.Proxy.ids));
        }
    }
}
