using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LoggingDemo1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection service = new ServiceCollection();
            service.AddLogging(logBuilder =>
            {
                logBuilder.AddConsole();
            });
            service.AddScoped<Test>();
            using (var sp = service.BuildServiceProvider())
            {
                var tst = sp.GetRequiredService<Test>();
                tst.Test1();
            }


        }
    }
}
