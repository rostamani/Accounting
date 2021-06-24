using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.ViewModels;

namespace Accounting.DataLayer.Repositories
{
    public interface ICustomerRepository
    {
        List<Customers> GetAllCustomers();
        List<Customers> GetCustomersAfterFilter(string filter);
        List<CustomersNamesViewModel> GetCustomersNames(string filter="");
        Customers GetCustomerById(int customerId);
        bool InsertCustomer(Customers customer);
        bool UpdatetCustomer(Customers customer);
        bool DeleteCustomer(Customers customer);
        bool DeleteCustomer(int customerId);
    }
}
