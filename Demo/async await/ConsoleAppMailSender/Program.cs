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
            services.AddScoped<IConfigService, ConfigService>();
            services.AddScoped<ILogProvider, ConsoleLogProvider>();
            services.AddScoped<IMailService, MailService>();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                var mail = serviceProvider.GetRequiredService<IMailService>();
                mail.SendEmail("Hello", "1", "111");
            }

            Console.Read();
        }
    }
}
