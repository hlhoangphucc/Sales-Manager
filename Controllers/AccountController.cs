using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SHOPTV.Data;
using SHOPTV.Models;

namespace SHOPTV.Controllers
{
    public class AccountController : Controller
    {
        private readonly SHOPTVContext _context;
        public AccountController(SHOPTVContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Signup()
        {
            return View();
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            Debug.WriteLine("chay cai nay 1 ");
            // Lấy thông tin người dùng từ session
            var id = HttpContext.Session.GetInt32("id");
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            ViewBag.avatar = HttpContext.Session.GetString("avatar");
            ViewBag.username = account.Username;
            ViewBag.id = id;
            ViewBag.name = HttpContext.Session.GetString("fullname");

            return View(account);
        }

        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            var acc = _context.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);
            if (acc != null)
            {
                var id = acc.Id;
                var fullname = acc.FullName ?? string.Empty;
                var avatar = acc.Avatar ?? string.Empty;
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

        [HttpPost]
        public async Task<IActionResult> Signup(Account account)
        {
            if (ModelState.IsValid)
            {
                if (account.Avatar != null && account.AvatarFile.Length > 0)
                {
                    var fileName = Path.GetFileName(account.AvatarFile.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
             
                    }
                    var filePath = Path.Combine(uploadsFolder, fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await account.AvatarFile.CopyToAsync(fileStream);
                    }

                    account.Avatar = "/images/" + fileName;
                }

                _context.Add(account);
                await _context.SaveChangesAsync(); 

                return RedirectToAction("Index", "Home");
            }

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,FullName,Address,Avatar,AvatarFile")] Account account)
        {
            Debug.WriteLine("chay cai nay 2 ");

            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (account.AvatarFile != null && account.AvatarFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(account.AvatarFile.FileName);
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        var filePath = Path.Combine(uploadsFolder, fileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await account.AvatarFile.CopyToAsync(fileStream);
                        }

                        account.Avatar = "/images/" + fileName;
                    }

                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }
    }
}

