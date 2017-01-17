using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApplication.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace WebShopApplication.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductsContext context)
        {
            context.Database.EnsureCreated();

            // Look for any blog posts.
            if (context.Product.Any())
            {
                return;   // DB has been seeded
            }

            var products = new Products[]
            {
                new Products { ProductName = "Tonfiskpizza", ProductDescription = "En saftig god pizza med färsk tonfisk", ProductIngredients = "{ \"ingredients\": [ \"Tomatsås\", \"Tonfisk\", \"Sallad\", \"Vitlök\" ] }", ProductPrice = 68.0M },
                new Products { ProductName = "Salami special", ProductDescription = "Klassisk italiensk salami special", ProductIngredients = "{ \"ingredients\": [ \"Tomatsås\", \"Skinka\",  \"Lök\", \"Salami\" ] }", ProductPrice = 68.0M }
            };
            foreach (Products s in products)
            {
                context.Product.Add(s);
            }
            context.SaveChanges();
        }
    }
}