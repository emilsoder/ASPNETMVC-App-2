using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShopApp.Models
{
    public class AdministratorViewModel
    {
        public AdministratorViewModel administratorViewModel { get; set; }

        public Customers customers { get; set; }
        public Products products { get; set; }
        public ShoppingCartDetails shoppingCartDetails { get; set; }
        public ShoppingCarts shoppingCarts { get; set; }

        public IEnumerable<Customers> _customers { get; set; }
        public IEnumerable<Products> _products { get; set; }
        public IEnumerable<ShoppingCartDetails> _shoppingCartDetails { get; set; }
        public IEnumerable<ShoppingCarts> _shoppingCarts { get; set; }
    }
}
