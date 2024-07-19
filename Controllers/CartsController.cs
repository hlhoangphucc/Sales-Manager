using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SHOPTV.Data;
using SHOPTV.Models;
using System.Diagnostics;

namespace SHOPTV.Controllers
{
    public class CartsController : Controller
    {
        private readonly SHOPTVContext _context;

        public CartsController(SHOPTVContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            // Lấy thông tin người dùng từ session
            var username = HttpContext.Session.GetString("username");

            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "User");
            }

            // Đặt các giá trị vào ViewBag để sử dụng trong View nếu cần thiết
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.user = username;
            ViewBag.name = HttpContext.Session.GetString("fullname");
            
            // Lấy danh sách sản phẩm trong giỏ hàng của người dùng hiện tại
            var cartItems = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .Where(c => c.Account.Username == username)
                .ToListAsync();
            return View(cartItems);
        }
        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Set<Account>(), "Id", "Username");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.user = HttpContext.Session.GetString("username");
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Set<Account>(), "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Set<Account>(), "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Set<Account>(), "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Carts == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Carts == null)
            {
                return Problem("Entity set 'NHOM04Context.Cart'  is null.");
            }
            var cart = await _context.Carts.FindAsync(id);
            if (cart != null)
            {
                _context.Carts.Remove(cart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public IActionResult DeleteAll(string username)
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.user = HttpContext.Session.GetString("username");
            username = ViewBag.user;
            var carts = _context.Carts.Include(c => c.Account)
                          .Include(c => c.Product)
                          .Where(c => c.Account.Username == username);
            foreach (var item in carts)
            {
                _context.Carts.Remove(item);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Purchase()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Purchase(string ShippingAddress, string ShippingPhone, string username)
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.user = HttpContext.Session.GetString("username");
            username = ViewBag.user;

            var carts = _context.Carts.Include(c => c.Account)
                                      .Include(c => c.Product)
                                      .Where(c => c.Account.Username == username);
            var account = _context.Accounts.FirstOrDefault(a => a.Username == username);
            int accountId = 0;
            if (account != null)
            {
                accountId = account.Id;
            }
            var total = carts.Sum(c => c.Product.Price * c.Quantity);

            Invoice invoice = new Invoice
            {
                Code = DateTime.Now.ToString("yyyyMMddhhmmss"),
                AccountId = accountId,
                IssuedDate = DateTime.Now,
                ShippingAddress = ShippingAddress,
                ShippingPhone = ShippingPhone,
                Total = total,
                Status = true
            };
            _context.Invoices.Add(invoice);
            _context.SaveChanges();

            foreach (var item in carts)
            {
                InvoiceDetail detail = new InvoiceDetail
                {
                    InvoiceId = invoice.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                };
                _context.InvoiceDetails.Add(detail);
                _context.Carts.Remove(item);

                item.Product.Stock -= item.Quantity;
                _context.Products.Update(item.Product);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        public IActionResult Add(int productid, int quantity)
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.user = HttpContext.Session.GetString("username");
            string username = ViewBag.user;

            var account = _context.Accounts.FirstOrDefault(a => a.Username == username);

            if (account != null)
            {

                var accountid = account.Id;
                var cart = _context.Carts.Where(c => c.AccountId == accountid && c.ProductId == productid).FirstOrDefault();

                if (cart != null)
                {
                    cart.Quantity += quantity;
                    _context.Update(cart);
                    _context.SaveChanges();
                }
                else
                {

                    cart = new Cart
                    {
                        AccountId = accountid,
                        ProductId = productid,
                        Quantity = quantity
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }
                return Json(new { success = true, message = "Product added to cart successfully." });
            }
            else
            {
                return RedirectToAction("Login", "User");
                throw new InvalidOperationException($"Account with username '{username}' not found.");
            }

        }

        [HttpPost]
        public JsonResult UpdateQuantity(int productid, int quantity)
        {
            try
            {
                ViewBag.id = HttpContext.Session.GetInt32("id");
                int? id = ViewBag.id;
                if (id.HasValue && id.Value > 0)
                {
                    var cart = _context.Carts.Where(c => c.AccountId == id && c.ProductId == productid).FirstOrDefault();
                    if (cart != null)
                    {
                        cart.Quantity = quantity;
                        _context.Update(cart);
                        _context.SaveChanges();
                        return Json(new { success = true });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Cart item not found" });
                    }
                }
                return Json(new { success = false, message = "Account not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult GetQuantity()
        {
            try
            {
                ViewBag.id = HttpContext.Session.GetInt32("id");
                int? id = ViewBag.id;
                if (id.HasValue && id.Value > 0)
                {
                    var totalQuantity = _context.Carts.Where(c => c.AccountId == id).Sum(c => c.Quantity);
                    return Json(new { success = true, totalQuantity = totalQuantity });

                }
                return Json(new { success = false, message = "Account not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
