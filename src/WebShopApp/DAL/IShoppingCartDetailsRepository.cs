using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public interface IShoppingCartDetailsRepository : IDisposable
    {
        Task<IEnumerable<ShoppingCartDetails>> GetShoppingCartDetails();
        Task<ShoppingCartDetails> GetShoppingCartDetailsByID(int shoppingCartID);
        void InsertShoppingCartDetails(Products product, int customerID);
        void DeleteShoppingCartDetails(int shoppingCartID);
        void UpdateShoppingCartDetails(ShoppingCartDetails shoppingCartDetails);
        int CountProductsInShoppingCart(int shoppingCartID);
        void Save();
    }
}
