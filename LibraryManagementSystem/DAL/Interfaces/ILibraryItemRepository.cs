using System.Collections.Generic;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    /// <summary>
    /// Defines all of the required methods and properties for a repository of LibraryItem objects.
    /// </summary>
    public interface ILibraryItemRepository : IGenericRepository<LibraryItem>
    {
        /// <summary>
        /// Searches for library items with a specific type and property value.
        /// </summary>
        /// <param name="type">The type of library item (Book, CD, etc.)</param>
        /// <param name="key">The property to serach by (Title, Author, etc.)</param>
        /// <param name="query">The keywords to search for.</param>
        /// <returns>The list of search results.</returns>
        ICollection<LibraryItem> SearchLibraryItems(string type, string key, string query);
    }
}
