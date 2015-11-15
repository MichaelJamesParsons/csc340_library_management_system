using System.Security.Cryptography.X509Certificates;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Interfaces
{
    public interface IReservationRepository : IGenericRepository<Reservation>
    {
        int CountItemReservations(LibraryItem item);
    }
}
