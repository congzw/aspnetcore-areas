using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Areas.Demo.Controllers
{
    [Area("Demo")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DiTest()
        {
            return View();
        }
    }
}
