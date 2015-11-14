using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL.Repositories
{
    public class ReservationRepository : GenericRepository<LibraryDataContext, Reservation>, IReservationRepository
    {
    }
}