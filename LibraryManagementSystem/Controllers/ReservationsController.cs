using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;

namespace LibraryManagementSystem.Controllers
{
    public class ReservationsController : Controller
    {
        private LibraryDataContext db = new LibraryDataContext();
        private readonly IReservationRepository _reservationRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILibraryItemRepository _libraryItemRepository;

        public ReservationsController(IReservationRepository reservationRepository, 
            ICustomerRepository customerRepository, ILibraryItemRepository libraryItemRepository)
        {
            this._reservationRepository = reservationRepository;
            this._customerRepository = customerRepository;
            this._libraryItemRepository = libraryItemRepository;
        }


        // GET: Reservations
        public ActionResult Index()
        {
            return View(db.Reservations.ToList());
        }

        // GET: Reservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        public ActionResult Create([Bind(Include = "CustomerNumber,ItemId")]AddReservationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                /*db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();*/
                //return RedirectToAction("Index");
            }
            /*if (customerNumber != null)
            {
                ViewBag.customer = Customer.FindByCustomerNumber(customerNumber);
            }*/

            return View();
        }

        [HttpPost]
        public JsonResult AjaxCreate()
        {
            Int32 customerId;
            Int32 itemId;
            bool isReserved;

            try
            {
                customerId = Int32.Parse(Request.Form["CustomerId"]);
                itemId = Int32.Parse(Request.Form["LibraryItemId"]);
                isReserved = Boolean.Parse(Request.Form["IsReserved"]);
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again."
                });
            }

            var customer = _customerRepository.FindBy(s => s.Id == customerId).Include(s => s.Reservations).FirstOrDefault();
            var item = _libraryItemRepository.Find(itemId);

            if (customer == null)
            {
                return Json(new
                {
                    status = false,
                    response = "Customer does not exist."
                });
            }

            if (item == null)
            {
                return Json(new
                {
                    status = false,
                    response = "Item does not exist."
                });
            }
            
            if (customer.Reservations.Count > 0)
            {
                return Json(new
                {
                    status = false,
                    response = "This custom has already checked out this item."
                });
            }
            

            var reservation = new Reservation
            {
                CheckOutDate = DateTime.Today,
                Customer_Id =  customer.Id,
                LibraryItem_Id = item.Id
                //Customer = customer,
                //LibraryItem = item
            };

            _reservationRepository.Add(reservation);
            _reservationRepository.Save();
           
            string responseMessage;
            if (isReserved)
            {
                responseMessage = $"Item Reserved. You must check it out by {reservation.GetDueDate()}";
            }
            else
            {
                responseMessage = $"Item Reserved. You must return it by {reservation.GetDueDate()}";
            }

            return Json(new
            {
                status = true,
                response = responseMessage
            });
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerId,IsReserved")] Reservation reservation, string customerNumber)
        {
            if (customerNumber.Length != 9)
            {
                ModelState.AddModelError("customerNumber","Invalid Customer Number");
            }

            if (ModelState.IsValid)
            {
                Response.Write(Request.Form + "<br />");
                Response.Write("---->" + customerNumber);
                //db.Reservations.Add(reservation);
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }

            return View(reservation);
        }

        // GET: Reservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IsReserved,CheckOutDate")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
