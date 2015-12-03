using System;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    /// <summary>
    /// This repository stores all cached customer objects.
    /// </summary>
    public class CustomerRepository : GenericRepository<LibraryDataContext, Customer>, ICustomerRepository
    {
        /// <summary>
        /// Fetches a customer from the cache
        /// </summary>
        /// <param name="customerNumber">The value of a Customer object's CustomerNumber property.</param>
        /// <returns>Customer Object</returns>
        public Customer FindCustomerByCustomerNumber(string customerNumber)
        {
            return FindBy(n => n.CustomerNumber == customerNumber).FirstOrDefault();
        }


        /// <summary>
        /// Calculates the total late fees for a specific customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>The total fee</returns>
        public int GetTotalFees(int id)
        {
            var oneWeekAgo = DateTime.Now.Subtract(new TimeSpan(7, 0,0,0,0));
            var results = Context.Reservations.Where(x => x.CustomerId == id && x.CheckOutDate < oneWeekAgo).ToArray();
            return results.Sum(r => (int?)(oneWeekAgo - r.CheckOutDate).Days) ?? 0;
        }
    }
}