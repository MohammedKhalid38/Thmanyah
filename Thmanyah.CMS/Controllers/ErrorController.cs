using Microsoft.AspNetCore.Mvc;

namespace Thmanyah.CMS.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
