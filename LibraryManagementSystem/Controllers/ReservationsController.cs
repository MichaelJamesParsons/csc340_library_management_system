using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILibraryItemRepository _libraryItemRepository;

        public ReservationsController(IReservationRepository reservationRepository, 
            ICustomerRepository customerRepository, ILibraryItemRepository libraryItemRepository)
        {
            _reservationRepository = reservationRepository;
            _customerRepository = customerRepository;
            _libraryItemRepository = libraryItemRepository;
        }

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
                customerId = int.Parse(Request.Form["CustomerId"]);
                itemId = int.Parse(Request.Form["LibraryItemId"]);
                isReserved = bool.Parse(Request.Form["IsReserved"]);
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }

            customer = _customerRepository.FindBy(s => s.Id == customerId).Include(s => s.Reservations).FirstOrDefault();

            //Does the customer exist?
            if (customer == null)
                return Json(new { status = false, response = "Customer does not exist." });

            //Does the customer already have 5 items checked out?
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
            
            var action = isReserved ? "check it out" : "return it";
            var responseMessage = $"Item Reserved. You must {action} by {reservation.GetDueDate()}";

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


        [HttpPost]
        public JsonResult Details()
        {
            int id;

            try
            {
                id = int.Parse(Request.Form["reservation_id"]);
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }

            var reservation = _reservationRepository.FindBy(i => i.Id == id)
                                .Select(e => new{ e,ItemId = e.LibraryItem.Id })
                                .FirstOrDefault();

            if (reservation == null)
            {
                return Json(new
                {
                    status = false,
                    response = "Sorry, that reservation no longer exists."
                });
            }

            var reservedItem = _libraryItemRepository.Find(reservation.ItemId);

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


        [HttpPost]
        public JsonResult CheckIn()
        {
            try
            {
                var id = int.Parse(Request.Form["id"]);
                var reservation = _reservationRepository.Find(id);
                _reservationRepository.Delete(reservation);
                _reservationRepository.Save();

                return Json(new
                {
                    status = true,
                    response = "Item successfully checked in."
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }
        }


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
