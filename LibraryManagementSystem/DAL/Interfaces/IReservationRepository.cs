using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    /// <summary>
    /// Defines all of the required methods and properties for a repository of reservation objects.
    /// </summary>
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        /// <summary>
        /// Count the number of reservations assigned to a specific library item.
        /// </summary>
        /// <param name="item">The library item object.</param>
        /// <returns>The count of reservations.</returns>
        int CountItemReservations(LibraryItem item);
    }
}
