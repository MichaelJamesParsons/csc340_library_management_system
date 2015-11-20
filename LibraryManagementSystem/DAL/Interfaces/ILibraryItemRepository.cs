using System.Collections.Generic;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface ILibraryItemRepository : IGenericRepository<LibraryItem>
    {
        ICollection<LibraryItem> SearchLibraryItems(string type, string key, string query);
    }
}
