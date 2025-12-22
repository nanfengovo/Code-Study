using LoggingDemo1;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using static System.Net.Mime.MediaTypeNames;

namespace NLogDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceCollection service = new ServiceCollection();
            service.AddLogging(logBuilder =>
            {
                logBuilder.AddConsole();
                logBuilder.AddNLog();
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
