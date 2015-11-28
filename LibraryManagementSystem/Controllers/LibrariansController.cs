using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class LibrariansController : Controller
    {
        private readonly ILibrarianRepository _librarianRepository;
        
        public LibrariansController(ILibrarianRepository librarianRepository)
        {
            _librarianRepository = librarianRepository;
        }

        
        public ActionResult Index()
        {
            return View(_librarianRepository.GetAll());
        }

        
        public ActionResult Details(int? id)
        {
            var librarian = _librarianRepository.Find(id);
            if (librarian == null)
                return HttpNotFound();

            return View(librarian);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,FirstName,LastName,Email")] Librarian librarian)
        {
            if (!ModelState.IsValid)
                return View(librarian);

            var password = SHA256Hasher.Create(librarian.Password);
            librarian.Password = password;

            _librarianRepository.Add(librarian);
            _librarianRepository.Save();

            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(int? id)
        {
            var librarian = _librarianRepository.Find(id);
            if (librarian == null)
                return HttpNotFound();

            return View(librarian);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,FirstName,LastName,Email")] Librarian librarian)
        {
            if (!ModelState.IsValid)
                return View(librarian);

            _librarianRepository.Edit(librarian);
            _librarianRepository.Save();

            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(int? id)
        {
            var librarian = _librarianRepository.Find(id);
            if (librarian == null)
               return HttpNotFound();

            return View(librarian);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var librarian = _librarianRepository.Find(id);
            _librarianRepository.Delete(librarian);
            _librarianRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _librarianRepository.Dispose();
            }
            _librarianRepository.Dispose(disposing);
        }
    }
}
