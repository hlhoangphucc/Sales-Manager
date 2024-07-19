using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SHOPTV.Data;
using SHOPTV.Models;

namespace NHOM04.Controllers
{
    public class ProductsController : Controller
    {
        private readonly SHOPTVContext _context;

        public ProductsController(SHOPTVContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.name = HttpContext.Session.GetString("fullname");
            var SHOPTVContext = _context.Products.Include(p => p.ProductType);
            return View(await SHOPTVContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Detail(int? id)
        {
            ViewBag.id = HttpContext.Session.GetInt32("id");
            ViewBag.name = HttpContext.Session.GetString("fullname");
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.Set<ProductType>(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,Image,Status")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.Set<ProductType>(), "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.Set<ProductType>(), "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,Image,Status")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(_context.Set<ProductType>(), "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'NHOM04Context.Product'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Search(string name, int minPrice = 0, int maxPrice = int.MaxValue)
        {
            var products = await _context.Products
                                         .Where(p => p.Name.Contains(name))
                                         .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                         .ToListAsync();
            return View(products);
        }
        public async Task<IActionResult> SearchFillter(string name, int minPrice = 0, int maxPrice = int.MaxValue)
        {
            var products = await _context.Products
                                   .Where(p => p.Name.Contains(name))
                                   .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                                      .Select(p => new
                                      {
                                          p.Id,
                                          p.Name,
                                          p.Price,
                                          Image = p.Image
                                      })
                                   .ToListAsync();
            return Json(products);
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
