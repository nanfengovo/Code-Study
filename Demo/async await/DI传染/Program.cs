namespace DI传染
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    interface ILog
    {
        public void Log(string msg);
    }

    class LogImp1 : ILog
    {
        public void Log(string msg)
        {
            Console.WriteLine($"debug：{msg}");
        }
    }

    interface IConfig
    {
        public string GetString(string msg);
    }

    class Config : IConfig
    {
        public string GetString(string msg)
        {
            return msg;
        }
    }

}
