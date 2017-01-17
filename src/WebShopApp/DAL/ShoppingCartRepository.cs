using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp.DAL
{
    public class ShoppingCartRepository : IShoppingCartRepository, IDisposable
    {
        private WebShopDBContext _context;

        public ShoppingCartRepository(WebShopDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ShoppingCarts>> GetShoppingCarts()
        {
            return await _context.ShoppingCarts.ToListAsync();
        }

        public async Task<ShoppingCarts> GetShoppingCartByID(int shoppingCartID)
        {
            return await _context.ShoppingCarts.SingleOrDefaultAsync(x =>
                x.ShoppingCartID == shoppingCartID);
        }

        public void InsertShoppingCart(int customerID)
        {
            _context.ShoppingCarts.Add(new ShoppingCarts
            {
                CustomerID = customerID
            });
        }

        public async void DeleteShoppingCart(int shoppingCartID)
        {
            ShoppingCarts shoppingCart = await _context.ShoppingCarts.SingleOrDefaultAsync(x =>
                x.ShoppingCartID == shoppingCartID);

            _context.ShoppingCarts.Remove(shoppingCart);
        }

        public void UpdateShoppingCart(ShoppingCarts shoppingCart)
        {
            _context.Entry(shoppingCart).State = EntityState.Modified;
        }

        public async Task<bool> ShoppingCartExists(int customerID)
        {
            if (!await _context.ShoppingCarts.AnyAsync(x => x.CustomerID == customerID))
            {
                return false;
            }
            return true;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetShoppingCartID(int customerID)
        {
            return _context.ShoppingCarts.FirstOrDefault(x => x.CustomerID == customerID).ShoppingCartID;
        }
    }
}
