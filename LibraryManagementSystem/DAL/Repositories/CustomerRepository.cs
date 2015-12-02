using System;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class CustomerRepository : GenericRepository<LibraryDataContext, Customer>, ICustomerRepository
    {
        public Customer FindCustomerByCustomerNumber(string customerNumber)
        {
            return FindBy(n => n.CustomerNumber == customerNumber).FirstOrDefault();
        }

        public int GetTotalFees(int id)
        {
            var oneWeekAgo = DateTime.Now.Subtract(new TimeSpan(7, 0,0,0,0));
            var results = Context.Reservations.Where(x => x.CustomerId == id && x.CheckOutDate < oneWeekAgo).ToArray();
            return results.Sum(r => (int?)(oneWeekAgo - r.CheckOutDate).Days) ?? 0;
        }
    }
}