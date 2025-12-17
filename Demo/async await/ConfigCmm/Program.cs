using ConfigDI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ConfigCmm
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddCommandLine(args);
            IConfigurationRoot configuration = configurationBuilder.Build();
            ServiceCollection services = new ServiceCollection();
            services.AddScoped<TestController>();
            services.AddOptions().Configure<Config>(e => configuration.Bind(e));
            using (var sp = services.BuildServiceProvider())
            {
                var c = sp.GetRequiredService<TestController>();
                c.Test1();
            }
        }
    }
}
