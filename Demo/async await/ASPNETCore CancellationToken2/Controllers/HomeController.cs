using ASPNETCore_CancellationToken2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ASPNETCore_CancellationToken2.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            try
            {
                await Download3Async("https://www.baidu.com", 10000, cancellationToken);
                return View();
            }
            catch (Exception)
            {

                Debug.WriteLine("下载任务被取消");
                throw;
            }
            
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static async Task Download3Async(string url, int n, CancellationToken cancellationToken)
        {
            using (HttpClient client = new HttpClient())
            {
                for (int i = 0; i < n; i++)
                {
                    var resp = await client.GetAsync(url, cancellationToken);
                    string html = await resp.Content.ReadAsStringAsync();
                    Debug.WriteLine(html);
                }
            }
        }
    }
}
