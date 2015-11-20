using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface ILibrarianRepository : IGenericRepository<Librarian>
    {
        Librarian FindLibrarianByEmailAndPassword(string email, string password);
    }
}
