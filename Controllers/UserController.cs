using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NHOM04.Data;
using NHOM04.Models;

namespace NHOM04.Controllers
{
    public class UserController : Controller
    {
        private readonly NHOM04Context _context;
        public UserController(NHOM04Context context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var acc = _context.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);
            if (acc != null /*&& acc.IsAdmin == true*/)
            {
                var id = acc.Id;
                var fullname = acc.FullName;
                var avatar = acc.Avatar;
                var checkLogin = 1;
                HttpContext.Session.SetInt32("checkLogin", checkLogin);
                HttpContext.Session.SetInt32("id", id);
                HttpContext.Session.SetString("fullname", fullname);
                HttpContext.Session.SetString("avatar", avatar);
                HttpContext.Session.SetString("username", Username);
                if (acc.IsAdmin == true)
                {
                    return RedirectToAction("Home", "Admin");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.ErrorMsg = "Đăng nhập thất bại!";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public IActionResult Login(string Username, string Password)
        //{
        //    ViewData["username"] = Username;
        //    HttpContext.Session.SetString("username", Username);
        //    return RedirectToAction("Index","Home");
        //}

    }
}
