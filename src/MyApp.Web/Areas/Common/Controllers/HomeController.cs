using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Areas.Common.Controllers
{
    [Area("Common")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
