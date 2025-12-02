using System.Threading.Tasks;

namespace CancellationToken1
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(100000); // 5秒后取消

            Download3Async("https://www.baidu.com", 100,cts.Token);

            while(Console.ReadLine() != "q")
            {

            }
            cts.Cancel();
            Console.WriteLine("取消请求！！！！");
            Console.ReadLine();

        }

        /*
         * 为下载一个网址N次的方法增加取消功能 分别用GetStringAsync+IsCancelationRequested 、GetStringAsync+ThrowIfCancellationRequested()、带CancellationToken 的GetAsync() 分别实现。取消分别用超时、用户敲按键（不能await）实现
         */

        /// <summary>
        /// 下载N次指定网址的HTML内容 不带取消的版本
        /// </summary>
        /// <param name="url"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        static async Task DownloadAsync(string url,int n)
        {
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < n; i++)
                {
                    var html = await client.GetStringAsync(url);
                    Console.WriteLine($"{DateTime.Now}:{html}");
                }
            }
        }

        static async Task Download2Async(string url, int n,CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < n; i++)
                {
                    var html = await client.GetStringAsync(url);
                    Console.WriteLine($"{DateTime.Now}:{html}");
                    //if (cancellationToken.IsCancellationRequested)
                    //{
                    //    Console.WriteLine("下载任务被取消");
                    //    break;
                    //}

                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
        }

        static async Task Download3Async(string url, int n, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < n; i++)
                {
                    var resp = await client.GetAsync(url,cancellationToken);
                    string html = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine($"{DateTime.Now}:{html}");
                    
                }
            }
        }
    }
}
