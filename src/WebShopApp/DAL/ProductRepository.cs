using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public class ProductRepository : IProductRepository, IDisposable
    {
        private WebShopDBContext _context;

        public ProductRepository(WebShopDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Products>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products> GetProductByID(int productID)
        {
            return await _context.Products.SingleOrDefaultAsync(x =>
                x.ProductID == productID);
        }

        public void InsertProduct(Products product)
        {
            _context.Products.Add(product);
        }

        public async void DeleteProduct(int productID)
        {
            Products product = await _context.Products.SingleOrDefaultAsync(x =>
                x.ProductID == productID);

            _context.Products.Remove(product);
        }

        public void UpdateProduct(Products product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public async void Save()
        {
            await _context.SaveChangesAsync();
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

        public bool ProductExists(int productID)
        {
            return _context.Products.Any(e => e.ProductID == productID);
        }
    }
}
