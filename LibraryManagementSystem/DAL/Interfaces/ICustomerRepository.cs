using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    /// <summary>
    /// Defines all of the required methods and properties for a repository of Customer objects.
    /// </summary>
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        /// <summary>
        /// Fetches a customer from the cache
        /// </summary>
        /// <param name="customerNumber">The value of a Customer object's CustomerNumber property.</param>
        /// <returns>Customer Object</returns>
        Customer FindCustomerByCustomerNumber(string customerNumber);


        /// <summary>
        /// Calculates the total late fees for a specific customer.
        /// </summary>
        /// <param name="id">The ID of the customer.</param>
        /// <returns>The total fee</returns>
        int GetTotalFees(int id);
    }
}
