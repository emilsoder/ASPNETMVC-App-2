using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebShopApp.Services.ShoppingCartServices;
using Microsoft.AspNetCore.Http;

namespace WebShopApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }

    }
}
