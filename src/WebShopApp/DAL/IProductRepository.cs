using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public interface IProductRepository : IDisposable
    {
        Task<IEnumerable<Products>> GetProducts();
        Task<Products> GetProductByID(int productID);
        void InsertProduct(Products product);
        void DeleteProduct(int productID);
        void UpdateProduct(Products product);
        bool ProductExists(int productID);
        void Save();
    }
}
