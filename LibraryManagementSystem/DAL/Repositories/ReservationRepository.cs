using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    /// <summary>
    /// This repository stores all cached LibraryItem objects.
    /// </summary>
    public class ReservationRepository : GenericRepository<LibraryDataContext, Reservation>, IReservationRepository
    {
        /// <summary>
        /// Count the number of reservations assigned to a specific library item.
        /// </summary>
        /// <param name="item">The library item object.</param>
        /// <returns>The count of reservations.</returns>
        public int CountItemReservations(LibraryItem item)
        {
            var count = FindBy(r => r.LibraryItem.Id == item.Id).Count();
            return count;
        }
    }
}