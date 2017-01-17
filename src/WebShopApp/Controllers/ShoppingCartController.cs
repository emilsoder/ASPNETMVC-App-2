using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApp.Models;
using WebShopApp.DAL;

namespace WebShopApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private WebShopDBContext _context;

        private IProductRepository productRepository;
        private IShoppingCartDetailsRepository shoppingCartDetailsRepository;
        private IShoppingCartRepository shoppingCartRepository;
        private ICustomerRepository customerRepository;

        public ShoppingCartController(WebShopDBContext context)
        {
            this._context = context;
            this.productRepository = new ProductRepository(_context);
            this.shoppingCartDetailsRepository = new ShoppingCartDetailsRepository(_context);
            this.shoppingCartRepository = new ShoppingCartRepository(_context);
            this.customerRepository = new CustomerRepository(_context);
        }

        // GET: ShoppingCart
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var customerID = customerRepository.GetCustomerID(User.Identity.Name);
            var shoppingCartID = shoppingCartRepository.GetShoppingCartID(customerID);

            var tt = from x in _context.ShoppingCartDetails
                     where x.ShoppingCartID == shoppingCartID
                     select x;

            var o = tt.ToList();
            return View(await tt.ToListAsync());
        }

        // GET: ShoppingCart/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartDetails = await _context.ShoppingCartDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartDetails == null)
            {
                return NotFound();
            }

            return View(shoppingCartDetails);
        }

        // GET: ShoppingCart/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartDetails = await _context.ShoppingCartDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartDetails == null)
            {
                return NotFound();
            }
            return View(shoppingCartDetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductID,ProductName,Quantity,ShoppingCartID,UnitPrice")] ShoppingCartDetails shoppingCartDetails)
        {
            if (id != shoppingCartDetails.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shoppingCartDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShoppingCartDetailsExists(shoppingCartDetails.ID))
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
            return View(shoppingCartDetails);
        }

        // GET: ShoppingCart/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingCartDetails = await _context.ShoppingCartDetails.SingleOrDefaultAsync(m => m.ID == id);
            if (shoppingCartDetails == null)
            {
                return NotFound();
            }

            return View(shoppingCartDetails);
        }

        // POST: ShoppingCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shoppingCartDetails = await _context.ShoppingCartDetails.SingleOrDefaultAsync(m => m.ID == id);
            _context.ShoppingCartDetails.Remove(shoppingCartDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ShoppingCartDetailsExists(int id)
        {
            return _context.ShoppingCartDetails.Any(e => e.ID == id);
        }
    }
}
