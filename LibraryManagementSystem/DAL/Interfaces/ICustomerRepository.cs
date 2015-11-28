using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Customer FindCustomerByCustomerNumber(string customerNumber);
        int GetTotalFees(int id);
    }
}
