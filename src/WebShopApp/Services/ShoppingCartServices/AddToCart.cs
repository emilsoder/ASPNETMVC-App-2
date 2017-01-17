using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WebShopApp.Services.ShoppingCartServices
{
    public class ShoppingCartViewModel
    {
        public List<ShoppingCartDetails> unitPrice { get; set; }
        public List<Products> productName { get; set; }
    }
    public class ShoppingCartService
    {
        public static WebShopDBContext dbContext(WebShopDBContext dbx)
        {
            _context = dbx;
            return dbx;
        }

        public static WebShopDBContext _context { get; set; }

        public static void Initialize(string _user)
        {
            dbContext(_context);
            GetCustomerID(_user);
            _items = ItemsInShoppingCart();
        }

        [Authorize]
        public static void AddToCart(int _productID, int _quantity)
        {
            if (!_context.ShoppingCarts.Any(x => x.CustomerID == _customerID))
            {
                _context.ShoppingCarts.Add(new ShoppingCarts
                {
                    CustomerID = _customerID
                });
                _context.SaveChanges();
            }

            int _shoppingCartID = _context.ShoppingCarts.FirstOrDefault(x =>
                x.CustomerID == _customerID).ShoppingCartID;

            decimal _unitPrice = (_context.Products.FirstOrDefault(x => 
                x.ProductID == _productID)).UnitPrice;
            string _productName = (_context.Products.FirstOrDefault(x =>
                x.ProductID == _productID)).ProductName;

            _context.ShoppingCartDetails.Add(new ShoppingCartDetails
            {
                ShoppingCartID = _shoppingCartID,
                ProductID = _productID,
                UnitPrice = _unitPrice,
                Quantity = _quantity,
                ProductName = _productName
            });

            _context.SaveChanges();
        }
        public static List<ShoppingCartDetails> addedItemsList { get; set; }
        public static int GetCustomerID(string _email)
        {
            _customerID = _context.Customers.FirstOrDefault(x => x.Email == _email).CustomerID;
            GetShoppingCartID();
            return _customerID;
        }

        public static int GetShoppingCartID()
        {
            if (_context.ShoppingCarts.Any(x => x.CustomerID == _customerID))
            {
                return _context.ShoppingCarts.FirstOrDefault(x => x.CustomerID == _customerID).ShoppingCartID;
            }
            else
            {
                var uid = (_context.Customers.Single(x => x.CustomerID == _customerID).CustomerID);
                _context.ShoppingCarts.Add(new ShoppingCarts
                {
                    CustomerID = uid

                });
                _context.SaveChanges();
            }
            return -0;
        }
       

        public static int ItemsInShoppingCart()
        {
            int _shoppingCartID = _customerID;
            var query = (from x in _context.ShoppingCartDetails where x.ShoppingCartID == _shoppingCartID select x.Quantity);
            int item = query.Sum();
            var query2 = (from x in _context.ShoppingCartDetails where x.ShoppingCartID == _shoppingCartID select x).ToList();
            addedItemsList = query2;
            return item;
        }

        public static int _customerID { get; set; }
        public static int _items { get; set; }
        public static object Users { get; set; }

        private bool ShoppingCartExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
        public static ApplicationUser _appUser { get; set; }
        public static ApplicationUser currentUser(ApplicationUser s)
        {
            _appUser = s;
            return s;
        }
    }
}