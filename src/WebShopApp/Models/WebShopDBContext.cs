using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebShopApp.Models
{
    public class WebShopDBContext : DbContext
    {
        public WebShopDBContext(DbContextOptions<WebShopDBContext> options) : base(options)
        {
        }

        //public WebShopDBContext()
        //{
        //}

        public DbSet<Products> Products { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<ShoppingCarts> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartDetails> ShoppingCartDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Customers>().ToTable("Customers");
            modelBuilder.Entity<ShoppingCarts>().ToTable("ShoppingCarts");
            modelBuilder.Entity<ShoppingCartDetails>().ToTable("ShoppingCartDetails");
        }
    }
}
