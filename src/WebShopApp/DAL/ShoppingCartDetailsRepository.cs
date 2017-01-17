using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public class ShoppingCartDetailsRepository : IShoppingCartDetailsRepository, IDisposable
    {
        private WebShopDBContext _context;
        public ShoppingCartDetailsRepository(WebShopDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ShoppingCartDetails>> GetShoppingCartDetails()
        {
            return await _context.ShoppingCartDetails.ToListAsync();
        }

        public async Task<ShoppingCartDetails> GetShoppingCartDetailsByID(int shoppingCartID)
        {
            return await _context.ShoppingCartDetails.SingleOrDefaultAsync(x =>
                x.ShoppingCartID == shoppingCartID);
        }

        public void InsertShoppingCartDetails(Products product, int customerID)
        {
            _context.ShoppingCartDetails.Add(new ShoppingCartDetails
            {
                ShoppingCartID = customerID,
                ProductID = product.ProductID,
                UnitPrice = product.UnitPrice,
                ProductName = product.ProductName,
                Quantity = 1
            });
            Save();
        }

        public async void DeleteShoppingCartDetails(int shoppingCartID)
        {
            ShoppingCartDetails shoppingCartDetails = await _context.ShoppingCartDetails.SingleOrDefaultAsync(x =>
                x.ShoppingCartID == shoppingCartID);

            _context.ShoppingCartDetails.Remove(shoppingCartDetails);
        }

        public void UpdateShoppingCartDetails(ShoppingCartDetails shoppingCartDetails)
        {
            _context.Entry(shoppingCartDetails).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public int CountProductsInShoppingCart(int shoppingCartID)
        {
            return (_context.ShoppingCartDetails
                             .Include(x => x.Quantity)
                             .Where(x => x.ShoppingCartID == shoppingCartID)
                             .Select(x => x.Quantity).Sum());
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
    }
}
