using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;
using MySql.Data.MySqlClient;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository _customerRepo;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepo = customerRepository;
        }
        

        public ActionResult Find()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Find([Bind(Include = "CustomerNumber")] CustomerFindViewModel viewModel)
        {
            //Check for errors from the model
            if (!ModelState.IsValid)
                return View(viewModel);

            //Search for a customer with the given customer number
            var customer = _customerRepo.FindCustomerByCustomerNumber(viewModel.CustomerNumber);

            //If the customer exists, redirect to the customer's detail page
            if (customer != null)
                return RedirectToAction("Details", new { customer.Id });

            //The customer doesn't exist. Send an error to the view
            ModelState.AddModelError("customerNumber",
                "Sorry, there isn't a customer with that customer number.");

            return View(viewModel);
        }


        public ActionResult Index()
        {
            var customers = _customerRepo.GetAll();
            @ViewBag.customersExist = (customers != null && customers.Any());

            return View(customers);
        }


        public ActionResult Details(int? id)
        {
            //Get the customer's database record and all of their checked out/reserved items
            var customer = _customerRepo.FindBy(s => s.Id == id).Include(s => s.Reservations).Include("Reservations.LibraryItem").FirstOrDefault();

            //Throw 404 if the customer doesn't exist
            if (customer == null)
                return HttpNotFound();

            @ViewBag.LateFee = _customerRepo.GetTotalFees(customer.Id);

            return View(customer);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustomerNumber,FirstName,LastName,Email")] Customer customer)
        {
            //Check for errors from the model
            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                //Attempt to save the new customer object to the database
                _customerRepo.Add(customer);
                _customerRepo.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong. Please refresh and try again.");
            }

            return View(customer);
        }

        
        public ActionResult Edit(int? id)
        {
            var customer = _customerRepo.Find(id);

            if (customer == null)
                HttpNotFound();

            return View(customer);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustomerNumber,FirstName,LastName,Email")] Customer customer)
        {
            if (!ModelState.IsValid)
                return View(customer);

            try
            {
                _customerRepo.Edit(customer);
                _customerRepo.Save();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong.");
            }

            return View(customer);
        }

        
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
                    var id = int.Parse(Request.Form["id"]);
                    var customer = _customerRepo.FindBy(x => x.Id == id).Include("Reservations").FirstOrDefault();

                    if (customer.Reservations.Count > 0)
                    {
                        return Json(new
                        {
                            status = false,
                            response = "Checked out/reserved items found. This customer must return " +
                                            "all items and cancel all reservations before this account may be deleted."
                        });
                    }

                    _customerRepo.Delete(customer);
                    _customerRepo.Save();
                }
                catch (MySqlException e)
                {
                    //1451 == MySQL Foreign key constraint error 
                    //              (Customer has items reserved / checked out)
                    if (e.Number == 1451)
                    {
                        return Json(new
                        {
                            status = false,
                            response = "Checked out/reserved items found. This customer must return " +
                                            "all items and cancel all reservations before this account may be deleted."
                        });
                    }
                    //If the error isn't from a foreign key constraint,
                    //throw error to the generic error message.
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

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = _customerRepo.Find(id);

            if (customer == null)
                return RedirectToAction("Index");

            try
            {
                _customerRepo.Delete(customer);
                _customerRepo.Save();
            }
            catch (MySqlException e)
            {
                if (e.Number == 1451)
                {
                    ModelState.AddModelError(string.Empty,
                        "Checked out/reserved items found. This customer must return " +
                        "all items and cancel all reservations before this account may be deleted.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty,
                        "Oops! Something went wrong. Please refresh and try again.");
                }
            }

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customerRepo.Dispose();
            }
            _customerRepo.Dispose(disposing);
        }
    }
}
