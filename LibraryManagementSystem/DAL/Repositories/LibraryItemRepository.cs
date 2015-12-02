using System.Collections.Generic;
using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class LibraryItemRepository : GenericRepository<LibraryDataContext, LibraryItem>, ILibraryItemRepository
    {
        public ICollection<LibraryItem> SearchLibraryItems(string type, string key, string query)
        {
            IQueryable<LibraryItem> results = null;

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

            if (type.Equals("") || type.Equals("All"))
            {
                return results.ToList();
            }

            return results.Where(x => x.ItemType.Equals(type)).ToList();
        }
    }
}