using Microsoft.AspNetCore.Mvc;

namespace OpenRealEstate.WebSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}