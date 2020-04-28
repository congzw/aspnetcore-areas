using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Areas.Demo.Domain;

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
