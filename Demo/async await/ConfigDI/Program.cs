using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigDI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<TestController>();
            services.AddOptions().Configure<Config>(e => configuration.Bind(e));
            using (var sp = services.BuildServiceProvider())
            {
                var c = sp.GetRequiredService<TestController>();
                c.Test();
            }

        }
    }

    public class Config
    {
        public string Name { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;

        public Proxy Proxy { get; set; } = new Proxy();
    }

    public class Proxy
    {
        public string Address { get; set; } = string.Empty;
        public int Port { get; set; }

        public int[] ids { get; set; }
    }
}
