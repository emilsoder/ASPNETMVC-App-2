using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShopApp.Models;

namespace WebShopApp.DAL
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        private WebShopDBContext _context;

        public CustomerRepository(WebShopDBContext context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Customers>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customers> GetCustomerByID(int customerID)
        {
            return await _context.Customers.SingleOrDefaultAsync(x =>
                x.CustomerID == customerID);
        }

        public async void InsertCustomer(Customers customer)
        {
            try
            {
                _context.Customers.Add(customer);
                SaveAsync();
            }
            catch (DbUpdateException)
            {
                return;
            }

        }

        public async void DeleteCustomer(int customerID)
        {
            Customers customer = await _context.Customers.SingleOrDefaultAsync(x =>
                x.CustomerID == customerID);

            _context.Customers.Remove(customer);
        }

        public void UpdateCustomer(Customers customer)
        {
            _context.Entry(customer).State = EntityState.Modified;
        }

        public void EditCustomerDetails(Customers customer)
        {
            try
            {
                _context.Update(customer);
                SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return;
            }
        }

        public async void SaveAsync()
        {
            try
            {
                 _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return;
            }
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

        public int GetCustomerID(string userName)
        {
            var result =  _context.Customers.FirstOrDefault(x => x.Email == userName);
            return result.CustomerID;
        }
    }
}
