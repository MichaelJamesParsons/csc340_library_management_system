using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    /// <summary>
    /// This repository stores all cached customer objects.
    /// </summary>
    public class LibrarianRepository : GenericRepository<LibraryDataContext, Librarian>, ILibrarianRepository
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
        public Librarian FindLibrarianByEmailAndPassword(string email, string password)
        {
            return FindBy(x => x.Email.Equals(email) && x.Password.Equals(password)).FirstOrDefault();
        }
    }
}