using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebShopApp.Models;
using WebShopApp.DAL;
using Microsoft.AspNetCore.Authorization;

namespace WebShopApp.Controllers
{
    public class ProductsController : Controller
    {
        public static string _selectedCategory { get; set; }

        private WebShopDBContext _context;

        private IProductRepository productRepository;
        private IShoppingCartDetailsRepository shoppingCartDetailsRepository;
        private IShoppingCartRepository shoppingCartRepository;
        private ICustomerRepository customerRepository;

        public ProductsController(WebShopDBContext context)
        {
            this._context = context;
            this.productRepository = new ProductRepository(_context);
            this.shoppingCartDetailsRepository = new ShoppingCartDetailsRepository(_context);
            this.shoppingCartRepository = new ShoppingCartRepository(_context);
            this.customerRepository = new CustomerRepository(_context);
        }

        private string GetUserName
        {
            get
            {
                return User.Identity.Name;
            }
        }

        // GET: Products
        public async Task<IActionResult> Index(string selectedCategory)
        {
            ViewData["selectedCategory"] = _selectedCategory ?? "pizza";

            var products = from s in await productRepository.GetProducts()
                           select s;

            return View(products);
        }

        [HttpPost, ActionName("AddProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(int productID, string selectedCategory)
        {
            _selectedCategory = selectedCategory;

            if (User.Identity.IsAuthenticated)
            {
                var customerID = customerRepository.GetCustomerID(GetUserName);
                if (!await shoppingCartRepository.ShoppingCartExists(customerID))
                {
                    shoppingCartRepository.InsertShoppingCart(customerID);
                    shoppingCartRepository.Save();
                }
                else
                {
                    shoppingCartDetailsRepository.InsertShoppingCartDetails(await productRepository.GetProductByID(productID), customerID);
                    shoppingCartDetailsRepository.Save();
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Details(int id)
        {
            var products = await productRepository.GetProductByID(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Create([Bind("ProductID,Description,Ingredients,ProductName,UnitPrice")] Products product)
        {
            if (ModelState.IsValid)
            {
                productRepository.InsertProduct(product);
                productRepository.Save();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Edit(int id)
        {
            var products = await productRepository.GetProductByID(id);
            if (products == null)
            {
                return NotFound();
            }
            return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public IActionResult Edit(int id, [Bind("ProductID,Description,Ingredients,ProductName,UnitPrice")] Products product)
        {
            if (id != product.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productRepository.UpdateProduct(product);
                    productRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!productRepository.ProductExists(product.ProductID))
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
            return View(product);
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var products = await productRepository.GetProductByID(id);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Products product = await productRepository.GetProductByID(id);
            productRepository.DeleteProduct(id);
            productRepository.Save();

            return RedirectToAction("Index");
        }
    }
}
