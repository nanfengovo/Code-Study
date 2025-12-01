using System.Threading.Tasks;

namespace async_await_原理
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                var html = await httpClient.GetStringAsync("https://www.baidu.com");
                Console.WriteLine(html);
            }
            string txt = "hello";
            string filename = @"E:\Test\12.3.txt";
            await File.WriteAllTextAsync(filename, txt);
            Console.WriteLine("写入成功！！！");
            var str = await File.ReadAllTextAsync(filename);
            Console.WriteLine("文件内容为"+str);
        }
    }
}
