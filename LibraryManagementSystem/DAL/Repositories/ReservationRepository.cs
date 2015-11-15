using System.Linq;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class ReservationRepository : GenericRepository<LibraryDataContext, Reservation>, IReservationRepository
    {
        public int CountItemReservations(LibraryItem item)
        {
            var count = FindBy(r => r.LibraryItem.Id == item.Id).Count();
            return count;
        }
    }
}