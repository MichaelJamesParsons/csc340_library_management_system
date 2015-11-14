using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.Models;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

namespace LibraryManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*try
            {
                LibraryDataContext db = new LibraryDataContext();
                Librarian bob = new Librarian
                {
                    FirstName = "Bob",
                    LastName = "Doe",
                    Email = "bob@bob.com",
                    Username = "bobby",
                    Password = "test12345"
                };
                db.Librarians.Add(bob);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                Debug.Write("! ---------- Errors:\n");
                /foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                Debug.WriteLine("Finished Loading Errors");
            }*/

            /*LibraryItem t = db.LibraryItems.Find(1);
            ICollection<Reservation> c = t.Customers;*/
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}