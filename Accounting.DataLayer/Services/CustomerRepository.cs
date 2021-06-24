using Accounting.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Accounting.ViewModels;

namespace Accounting.DataLayer.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private Accounting_DBEntities _db;
        public CustomerRepository(Accounting_DBEntities db)
        {
            _db = db;
        }
        public bool DeleteCustomer(Customers customer)
        {
            try
            {
                _db.Entry(customer).State = EntityState.Deleted;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            var customer = GetCustomerById(customerId);
            return DeleteCustomer(customer);
        }

        public List<Customers> GetAllCustomers()
        {
            return _db.Customers.ToList();
        }

        public Customers GetCustomerById(int customerId)
        {
            return _db.Customers.Find(customerId);
        }

        public List<Customers> GetCustomersAfterFilter(string filter="")
        {
            return _db.Customers.Where(c => c.Name.Contains(filter) || c.Email.Contains(filter) || c.Mobile.Contains(filter)).ToList();
        }

        public List<CustomersNamesViewModel> GetCustomersNames(string filter)
        {
            if (filter == "")
                return _db.Customers.Select(c => new CustomersNamesViewModel { CustomerId=c.CustomerId,Name=c.Name}).ToList();
            else
                return _db.Customers.Where(c => c.Name.Contains(filter) || c.Email.Contains(filter) || c.Mobile.Contains(filter)).Select(c => new CustomersNamesViewModel { CustomerId = c.CustomerId, Name = c.Name }).ToList();
        }

        public bool InsertCustomer(Customers customer)
        {
            try
            {
                
                _db.Customers.Add(customer);
                return true;
            }
            catch
            {

                return false;
            }
        }

        

        public bool UpdatetCustomer(Customers customer)
        {
            try
            {
                var local = _db.Set<Customers>()
               .Local
               .FirstOrDefault(f => f.CustomerId == customer.CustomerId);
                if (local != null)
                {
                    _db.Entry(local).State = EntityState.Detached;
                }
                _db.Entry(customer).State = EntityState.Modified;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
