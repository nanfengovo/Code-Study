using ConfigServices;
using LogServices;
using MailServices;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppMailSender
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            //services.AddScoped<IConfigService, ConfigService>();
            //services.AddScoped(typeof(IConfigService),s=>new IniFileConfigService { FilePath = "mail.ini"});
            //services.AddScoped<ILogProvider, ConsoleLogProvider>();
            services.AddIniFileConfig("mail.ini");
            services.AddScoped<IMailService, MailService>();
            services.AddConsoleLog();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var mail = serviceProvider.GetRequiredService<IMailService>();
                mail.SendEmail("Hello", "1", "111");
            }

            Console.Read();
        }
    }
}
