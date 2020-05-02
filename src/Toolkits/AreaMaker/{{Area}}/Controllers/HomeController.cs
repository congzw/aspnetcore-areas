using Microsoft.AspNetCore.Mvc;

namespace {{ProjectPrefix}}{{Area}}.Controllers
{
    [Area("{{Area}}")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
