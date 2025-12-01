using System.Threading.Tasks;

namespace async1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            int length = await DownLoadHtmlAsync("https://www.youzack.com", @"E:\Test\12.2.txt");
            Console.WriteLine(length);
        }

        static async Task<int> DownLoadHtmlAsync(string url,string filename)
        {
            //HttpClientFactory
            using (HttpClient httpClient = new HttpClient())
            {
                string html = await httpClient.GetStringAsync(url);
                await File.WriteAllTextAsync(filename, html);
                return html.Length;
            }
        }
    }
}
