using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.DAL;
using WebShopApp.Models;
using WebShopApp.Services.ShoppingCartServices;

namespace WebShopApp.Services
{
    public class ShoppingCartDetailsService : IShoppingCartDetailsService
    {
        private WebShopDBContext _context;

        private IShoppingCartDetailsRepository shoppingCartDetailsRepository;
        private IShoppingCartRepository shoppingCartRepository;
        private ICustomerRepository customerRepository;

        public ShoppingCartDetailsService(WebShopDBContext context)
        {
            this._context = context;
            this.shoppingCartDetailsRepository = new ShoppingCartDetailsRepository(_context);
            this.shoppingCartRepository = new ShoppingCartRepository(_context);
            this.customerRepository = new CustomerRepository(_context);
        }

        public int GetCount(string userName)
        {
            return shoppingCartDetailsRepository.CountProductsInShoppingCart(GetCustomerID(userName));
        }

        public List<ShoppingCartDetails> currentShoppingCart(string userName)
        {
          
            var tt = from x in _context.ShoppingCartDetails
                     where x.ShoppingCartID == GetCustomerID(userName)
                     select x;

            return tt.ToList();
        }

        public int GetCustomerID(string userName)
        {
            var customerID = customerRepository.GetCustomerID(userName);
            var shoppingCartID = shoppingCartRepository.GetShoppingCartID(customerID);
            return shoppingCartID;
        }
    }

    public interface IShoppingCartDetailsService
    {
        int GetCount(string userName);
        List<ShoppingCartDetails> currentShoppingCart(string userName);
    }
}
