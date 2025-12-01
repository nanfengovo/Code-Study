using System.Text;
using System.Threading.Tasks;

namespace async_await
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string fileName = @"E:\Test\12.1.txt";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 100000; i++)
            {
                sb.AppendLine("hello");
            }
            await File.WriteAllTextAsync(fileName, sb.ToString());
            string s = await File.ReadAllTextAsync(fileName);
            Console.WriteLine(s);
        }
    }
}
