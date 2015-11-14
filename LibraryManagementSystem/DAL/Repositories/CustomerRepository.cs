using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class CustomerRepository : GenericRepository<LibraryDataContext, Customer>, ICustomerRepository
    {
        public Customer FindCustomerByCustomerNumber(string customerNumber)
        {
            var c = FindBy(n => n.CustomerNumber == customerNumber);
            return c.FirstOrDefault();
        }
    }
}