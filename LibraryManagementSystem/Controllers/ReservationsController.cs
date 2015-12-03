using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Contains the views and handlers for the check-in, check-out, and reserve operations.
    /// The user must be logged in to access these operations.
    /// </summary>
    [Authorize]
    public class ReservationsController : Controller
    {
        //Contains all of the cached Reservation objects
        private readonly IReservationRepository _reservationRepository;

        //Contains all of the cached Customer objects
        private readonly ICustomerRepository _customerRepository;

        //Contains all of the cached LibraryItem objects
        private readonly ILibraryItemRepository _libraryItemRepository;

        public ReservationsController(IReservationRepository reservationRepository, 
            ICustomerRepository customerRepository, ILibraryItemRepository libraryItemRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _libraryItemRepository = libraryItemRepository;
        }


        /// <summary>
        /// Creates an item reservation. The reservation has two states:
        ///     - checked out
        ///     - reserved
        /// The state is determined by the IsReserved property in the Reservation object.
        /// This method may only be access by an AJAX request.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult CheckOut()
        {
            int customerId;
            int itemId;
            bool isReserved;
            Customer customer;
            LibraryItem item;

            try
            {
                //Get the submitted customer ID
                customerId = int.Parse(Request.Form["CustomerId"]);

                //Get the submitted item ID
                itemId = int.Parse(Request.Form["LibraryItemId"]);

                //Get the submitted reservation state
                isReserved = bool.Parse(Request.Form["IsReserved"]);
            }
            catch (Exception)
            {
                //If the parsing of any of the data above fails, then the data is not valid.
                //Throw an error to let the user know.
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }

            //Fetch the customer by ID from the repository. Also include the customer's existing reservations
            customer = _customerRepository.FindBy(s => s.Id == customerId).Include(s => s.Reservations).FirstOrDefault();

            //If the customer doesn't exist, throw an error
            if (customer == null)
                return Json(new { status = false, response = "Customer does not exist." });

            //If the customer has 5 or more reservations, prevent them from reserving another item
            if (customer.Reservations.Count >= 5)
            {
                return Json(new
                {
                    status = false,
                    response = "Maximum checkout/reservation limit reached. Please return " +
                               "an item or cancel a reservation before attempting to check out another item."
                });
            }

            //Does the customer have overdue items checked out?
            if (_customerRepository.GetTotalFees(customer.Id) > 0)
                return Json(new { status = false,
                    response = "This customer has overdue items checked out. The items must be returned and " +
                               "paid off before this customer may checkout another item." });

            item = _libraryItemRepository.Find(itemId);

            //Does the library item exist?
            if (item == null)
                return Json(new { status = false, response = "Item does not exist." });

            //Is the library item allowed to be checked out?
            if (!item.CanCheckOut)
            {
                return Json(new
                {
                    status = false,
                    response = $"This item is a <b>{item.ItemType}</b>, which is not allowed to be checked out."
                });
            }
            
            //Has the customer already checked out this item?
            if (_reservationRepository.FindBy(i => i.LibraryItemId == item.Id)
                                        .FirstOrDefault(c => c.CustomerId == customer.Id) != null)
            {
                return Json(new
                {
                    status = false,
                    response = "This custom has already checked out this item."
                });
            }

            //Are there any more copies of the library item available to check out?
            if (item.Quantity - _reservationRepository.CountItemReservations(item) == 0)
            {
                return Json(new
                {
                    status = false,
                    response = "There aren't any more copies of that item available."
                });
            }

            //Create the new reservation
            var reservation = new Reservation
            {
                CheckOutDate = DateTime.Today,
                CustomerId =  customer.Id,
                LibraryItemId = item.Id
            };

            //Save the reservation
            _reservationRepository.Add(reservation);
            _reservationRepository.Save();
            
            //Formulate a proper response message based on whether the customer is
            //reserving the item, or checking it out
            var action = isReserved ? "check it out" : "return it";
            var responseMessage = $"Item Reserved. You must {action} by {reservation.GetDueDate()}";

            //Return the response in a JSON string
            return Json(new
            {
                status = true,
                response = new
                {
                    reservation = new
                    {
                        reservation.Id,
                        DueDate = reservation.GetDueDate(),
                        reservation.IsReserved
                    },
                    item = new
                    {
                       item.Title
                    },
                    message = responseMessage
                }
            });
        }


        /// <summary>
        /// Get the details of a reservation.
        /// This method may only be accessed by an AJAX request.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult Details()
        {
            int id;

            try
            {
                //Get the submitted reservation ID
                id = int.Parse(Request.Form["reservation_id"]);
            }
            catch (Exception)
            {
                //If the parsing of the reservation ID fails, then the ID must be invalid.
                //Throw an error to the user.
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }

            //Fetch the reservation from the repository and include the library item information with it
            var reservation = _reservationRepository.FindBy(i => i.Id == id)
                                .Select(e => new{ e,ItemId = e.LibraryItem.Id })
                                .FirstOrDefault();

            //If the reservation isn't found, send an error to the user
            if (reservation == null)
            {
                return Json(new
                {
                    status = false,
                    response = "Sorry, that reservation no longer exists."
                });
            }

            //Fetch the libary item from the repository
            var reservedItem = _libraryItemRepository.Find(reservation.ItemId);

            //Form and return the JSON response
            return Json(new
            {
                status = true,
                response = new
                {
                    id = reservation.e.Id,
                    checkOutDate = reservation.e.FormatCheckOutDate(),
                    dueDate = reservation.e.GetDueDate(),
                    lateFee = reservation.e.CalculateLateFee(),
                    item = new
                    {
                        id = reservedItem.Id,
                        title = reservedItem.Title,
                        type = reservedItem.GetItemType()
                    }
                }
            });
        }


        /// <summary>
        /// Check in a library item (In other words, this deletes a reservation object from the system).
        /// This method may only be accessed by an AJAX request.
        /// </summary>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult CheckIn()
        {
            try
            {
                //Get the submitted reservation ID
                var id = int.Parse(Request.Form["id"]);

                //Fetch the reservation from the repository
                var reservation = _reservationRepository.Find(id);

                //Delete the reservation from the repository
                _reservationRepository.Delete(reservation);

                //Save the changes to the repository
                _reservationRepository.Save();

                //Return a JSON success message
                return Json(new
                {
                    status = true,
                    response = "Item successfully checked in."
                });
            }
            catch (Exception)
            {
                //If the parsing of the ID fails, throw an error message to the user
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }
        }


        /// <summary>
        /// Releases loose objects and other unmanaged resources when the controller
        /// is no longer in use.
        /// Documentation: https://msdn.microsoft.com/en-us/library/fs2xkftw(v=vs.110).aspx
        /// </summary>
        /// <param name="disposing">Indicates whether the method call comes from a 
        ///             Dispose method (its value is true) or from a finalizer (its value is false).
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _libraryItemRepository.Dispose();
            }
            _libraryItemRepository.Dispose(disposing);
        }
    }
}
