using Microsoft.Extensions.Configuration;

namespace Config1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("config.json",optional:true,reloadOnChange:true);
            IConfigurationRoot configuration = builder.Build();
            //string name = configuration["name"]!;
            //var age = configuration["age"]!;
            //var address = configuration["address"]!;
            //Console.WriteLine(name);
            //Console.WriteLine(age);
            //Console.WriteLine(address);

            var s = configuration.GetSection("Config").Get<Config>();
            Console.WriteLine(s.Name);
            
        }
    }

    class Config
    {
        public string Name { get; set; } = string.Empty;

        public string Age { get; set; } = string.Empty;
    }
}
