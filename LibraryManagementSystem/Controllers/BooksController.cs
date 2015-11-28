using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly ILibraryItemRepository _libraryItemRepository;

        public BooksController(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = (Book) _libraryItemRepository.Find((int)id);

            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,PublicationYear,Author,ItemType,Quantity,CanCheckOut,Isbn")] Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            try
            {
                book.ItemType = "Book";
                _libraryItemRepository.Add(book);
                _libraryItemRepository.Save();
                return RedirectToAction("Index", "LibraryItems");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong. Please refresh and try again.");
            }

            return View(book);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            var book = _libraryItemRepository.FindBy(x => x.Id == id).OfType<Book>().First();
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PublicationYear,Author,ItemType,Quantity,CanCheckOut,Isbn")] Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            try
            {
                _libraryItemRepository.Edit(book);
                _libraryItemRepository.Save();
                return RedirectToAction("Index", "LibraryItems");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong.");
            }

            return View(book);
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
