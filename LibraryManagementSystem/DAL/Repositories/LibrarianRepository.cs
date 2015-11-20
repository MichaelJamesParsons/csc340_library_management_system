using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class LibrarianRepository : GenericRepository<LibraryDataContext, Librarian>, ILibrarianRepository
    {
        public Librarian FindLibrarianByEmailAndPassword(string email, string password)
        {
            return FindBy(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
        }
    }
}