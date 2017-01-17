using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public interface IShoppingCartRepository : IDisposable
    {
        Task<IEnumerable<ShoppingCarts>> GetShoppingCarts();
        Task<ShoppingCarts> GetShoppingCartByID(int shoppingCartID);
        void InsertShoppingCart(int customerID);
        void DeleteShoppingCart(int shoppingCartID);
        void UpdateShoppingCart(ShoppingCarts shoppingCart);
        Task<bool> ShoppingCartExists(int customerID);
        int GetShoppingCartID(int customerID);
        void Save();
    }
}
