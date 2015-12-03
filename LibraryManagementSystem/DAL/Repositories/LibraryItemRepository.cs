using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    /// <summary>
    /// This repository stores all cached LibraryItem objects.
    /// </summary>
    public class LibraryItemRepository : GenericRepository<LibraryDataContext, LibraryItem>, ILibraryItemRepository
    {
        /// <summary>
        /// Searches for library items with a specific type and property value.
        /// </summary>
        /// <param name="type">The type of library item (Book, CD, etc.)</param>
        /// <param name="key">The property to serach by (Title, Author, etc.)</param>
        /// <param name="query">The keywords to search for.</param>
        /// <returns>The list of search results.</returns>
        public ICollection<LibraryItem> SearchLibraryItems(string type, string key, string query)
        {
            IQueryable<LibraryItem> results = null;

            //Determine which property the search is attempting to search
            switch (key)
            {
                case "Title":
                    results = FindBy(x => x.Title.Contains(query));
                    break;
                case "PublicationYear":
                    results = FindBy(x => x.PublicationYear.Equals(query));
                    break;
                case "Author":
                    results = FindBy(x => x.Author.Contains(query));
                    break;
                case "Isbn":
                    results = Context.LibraryItems.OfType<Book>().Where(x => x.Isbn.Equals(query));
                    break;
                default:
                    return new List<LibraryItem>();
            }

            //If the search item type is empty or "All", then we'll return all records found in the query
            if (type.Equals("") || type.Equals("All"))
            {
                return results.ToList();
            }

            //Return only the records that are of a specific type
            return results.Where(x => x.ItemType.Equals(type)).ToList();
        }
    }
}