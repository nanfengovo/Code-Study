using Microsoft.AspNetCore.Mvc;

namespace FirstAICallWithHttpClient;

public class CallAI : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}