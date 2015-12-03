namespace LibraryManagementSystem.Models.ViewModels
{
    /// <summary>
    /// A generic ViewModel that handles reservation operations.
    /// </summary>
    public class ReservationViewModel
    {
    }

    /// <summary>
    /// Contains the required fields to add a reservation.
    /// </summary>
    public class AddReservationViewModel
    {
        /// <summary>
        /// A customer's CustomerNumber.
        /// </summary>
        public string CustomerNumber;

        /// <summary>
        /// A library item's Id.
        /// </summary>
        public string ItemId;
    }
}