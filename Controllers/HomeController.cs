using Microsoft.AspNetCore.Mvc;
using SHOPTV.Data;
using SHOPTV.Models;
using System.Diagnostics;

namespace SHOPTV.Controllers
{
    public class HomeController : Controller
    {
        private readonly SHOPTVContext _context;

        public HomeController(SHOPTVContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.name = HttpContext.Session.GetString("fullname");
            ViewBag.avt = HttpContext.Session.GetString("avatar");
            var prod = _context.Products.Skip(1).Take(8);
            return View(prod);
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
 
    }
}