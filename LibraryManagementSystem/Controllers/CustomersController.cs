using System;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using MySql.Data.MySqlClient;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private LibraryDataContext db = new LibraryDataContext();

        public CustomersController(ICustomerRepository customerRepo, IReservationRepository reservationRepository)
        {
            this._customerRepo = customerRepo;
        }

        //GET: Customers/Find
        public ActionResult Find()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Find(FormCollection form)
        {
            var customerNumber = Request.Form["customerNumber"];
            if (customerNumber != null && !customerNumber.Equals("") && Regex.IsMatch(customerNumber, @"^(901)[0-9]{6}$"))
            {
                var customer = this._customerRepo.FindCustomerByCustomerNumber(customerNumber);
                if (customer != null)
                {
                    return RedirectToAction("Details", new {customer.Id});
                }
                else
                {
                    ModelState.AddModelError("customerNumber",
                        "Sorry, there isn't a customer with that customer number.");
                }
            }
            else
            {
                ModelState.AddModelError("customerNumber", "Please enter a valid 9 digit customer number.");
            }

            return View();
        }

        // GET: Customers
        public ActionResult Index()
        {
            var customers = _customerRepo.GetAll();
            @ViewBag.customersExist = (customers != null && customers.Any());

            return View(customers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int id)
        {
            var customer = _customerRepo.FindBy(s => s.Id == id).Include(s => s.Reservations).Include("Reservations.LibraryItem").FirstOrDefault();

            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.reservations = customer.Reservations;
            ViewBag.fullName = customer.GetFullName();
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerNumber,FirstName,LastName,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepo.Add(customer);
                    _customerRepo.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong.");
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            var customer = this._customerRepo.Find((int) id);

            if (customer == null)
            {
                HttpNotFound();
            }

            return View(customer);

        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerNumber,FirstName,LastName,Email")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _customerRepo.Edit(customer);
                    _customerRepo.Save();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong.");
                }
            }

            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            var customer = _customerRepo.Find(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        [HttpPost]
        public JsonResult AjaxDelete()
        {
            try
            {
                try
                {
                    //$_POST["id"] == Request.Form["id"]
                    var id = int.Parse(Request.Form["id"]);
                    var customer = _customerRepo.Find(id);
                    _customerRepo.Delete(customer);
                    _customerRepo.Save();
                }
                catch (MySqlException e)
                {
                    //1451 == Foreign key constraint error (Customer has items reserved / checked out
                    if (e.Number == 1451)
                    {
                        return Json(new
                        {
                            status = false,
                            response = "Checked out/reserved items found. This customer must return " +
                                            "all items and cancel all reservations before this account may be deleted."
                        });
                    }

                    throw;
                }
            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = false,
                    response = "Oops! Something went wrong. Please refresh and try again.",
                    error = e.Message
                });
            }
            

            return Json(new
            {
                status = true,
                response = "Customer successfully deleted!"
            });
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = _customerRepo.Find(id);

            if (customer != null)
            {
                try
                {
                    _customerRepo.Delete(customer);
                    _customerRepo.Save();
                }
                catch (MySqlException e)
                {
                    if (e.Number == 1451)
                    {
                        ModelState.AddModelError(String.Empty,
                            "Checked out/reserved items found. This customer must return " +
                            "all items and cancel all reservations before this account may be deleted.");
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty,
                            "Oops! Something went wrong. Please refresh and try again.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    return View("Index");
                }
            }

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
