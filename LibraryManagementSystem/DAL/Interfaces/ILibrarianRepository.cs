using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    /// <summary>
    /// Defines all of the required methods and properties for a repository of Librarian objects.
    /// </summary>
    public interface ILibrarianRepository : IGenericRepository<Librarian>
    {
        /// <summary>
        /// Fetches the librarian with the corresponding email and password.
        /// 
        /// PreCondition: 
        /// Assumes that the password parameter has already been hashed with SHA256.
        /// </summary>
        /// <param name="email">The librarian's email.</param>
        /// <param name="password">The librarian's hashed password.</param>
        /// <returns></returns>
        Librarian FindLibrarianByEmailAndPassword(string email, string password);
    }
}
