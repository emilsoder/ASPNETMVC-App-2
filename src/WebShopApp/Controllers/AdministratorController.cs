using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebShopApp.Models;

namespace WebShopApp.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly WebShopDBContext _context;
        private readonly AdministratorViewModel _administratorViewModel;
        public AdministratorController(WebShopDBContext context)
        {
            _context = context;
            _administratorViewModel = new AdministratorViewModel();

        }

        public async Task<IActionResult> Index()
        {
            _administratorViewModel._customers = from c in _context.Customers select c;
            _administratorViewModel._products = from c in _context.Products select c;
            _administratorViewModel._shoppingCartDetails = from c in _context.ShoppingCartDetails select c;

            return View(_administratorViewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductID,CategoryNumber,Description,Ingredients,ProductName,UnitPrice")] Products products)
        {
            if (ModelState.IsValid)
            {
                _context.Add(products);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            _administratorViewModel._customers = from c in _context.Customers select c;
            _administratorViewModel.products = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            _administratorViewModel._shoppingCartDetails = from c in _context.ShoppingCartDetails select c;

            return View(_administratorViewModel);
          
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductID,CategoryNumber,Description,Ingredients,ProductName,UnitPrice")] AdministratorViewModel products)
        {
            if (id != products.products.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(products.products);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductsExists(products.products.ProductID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(products.products);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var products = await _context.Products.SingleOrDefaultAsync(m => m.ProductID == id);
            _context.Products.Remove(products);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.ProductID == id);
        }
    }
}
