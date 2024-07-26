using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.name = HttpContext.Session.GetString("fullname");
            ViewBag.avt = HttpContext.Session.GetString("avatar");
            return View();
        }
    }
}
