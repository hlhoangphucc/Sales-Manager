using Microsoft.AspNetCore.Mvc;
using NHOM04.Data;
using NHOM04.Models;
using System.Diagnostics;

namespace NHOM04.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly NHOM04Context _context;

        public HomeController(NHOM04Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.name = HttpContext.Session.GetString("fullname");
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
        //[HttpPost]
 
        //public async Task<IActionResult> Signup([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,Status")] Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(account);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View("Login");
        //}
        //public IActionResult Logout()
        //{
        //    Session["use"] = null;
        //    return View();
        //}
        //GET: Register

        //public ActionResult Register()
        //{
        //    return View();
        //}
    }
}