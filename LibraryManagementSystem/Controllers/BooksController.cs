using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Contains the views and handlers for library item (of type Book) CRUD (Create, Read, Update, Delete)
    /// operations. The user must be logged in to access these operations.
    /// </summary>
    [Authorize]
    public class BooksController : Controller
    {
        //Contains all of the cached Library Item objects
        private readonly ILibraryItemRepository _libraryItemRepository;

        public BooksController(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        
        /// <summary>
        /// Displays the properties of a Book object.
        /// Route: library/book/details/{id}
        /// </summary>
        /// <param name="id">The ID of the Library Item.</param>
        /// <returns>ActionResult</returns>
        public ActionResult Details(int? id)
        {
            //If the ID isn't provided, throw a bad request error
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Fetch the book object from the repository
            var book = (Book) _libraryItemRepository.Find((int)id);

            //If the book isn't found, throw 404 error
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        
        /// <summary>
        /// Displays the form that creates a new book.
        /// Route: /library/book/create
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Create()
        {
            return View();
        }


        /// <summary>
        /// Post request handler for the form that creates a new book. Using the submitted
        /// data from the form, a new Book object is created and saved.
        /// </summary>
        /// <param name="book">An instance of Book.</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,PublicationYear,Author,ItemType,Quantity,CanCheckOut,Isbn")] Book book)
        {
            //If validation errors exist, display them in the Create() view
            if (!ModelState.IsValid)
                return View(book);

            try
            {
                book.ItemType = "Book";

                //Add the book object to the repository
                _libraryItemRepository.Add(book);
                
                //Save the changes to the repository
                _libraryItemRepository.Save();

                //Redirect to the library items listing page
                return RedirectToAction("Index", "LibraryItems");
            }
            catch (Exception)
            {
                //An unknown error occurred. Throw an error message.
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong. Please refresh and try again.");
            }

            return View(book);
        }

        
        /// <summary>
        /// Displays the form to edit an existing book.
        /// Route: /library/books/edit/{id}
        /// </summary>
        /// <param name="id">The ID of an existing Book.</param>
        /// <returns>ActionResult</returns>
        public ActionResult Edit(int? id)
        {
            //Throw 403 bad request if an ID isn't provided
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            
            //Fetch the book from the repository by its ID
            var book = _libraryItemRepository.FindBy(x => x.Id == id).OfType<Book>().First();

            //If the book isn't found, throw 404 error
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        
        /// <summary>
        /// Post handler for edit form. Saves changes of the Book object
        /// to the repository.
        /// </summary>
        /// <param name="book">An instance of Book.</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PublicationYear,Author,ItemType,Quantity,CanCheckOut,Isbn")] Book book)
        {
            //If validation errors exist, display them to the user
            if (!ModelState.IsValid)
                return View(book);

            try
            {
                //Modify the book's cached record in the repository
                _libraryItemRepository.Edit(book);

                //Save the changes to the book
                _libraryItemRepository.Save();

                //Redirect to the library items listing page
                return RedirectToAction("Index", "LibraryItems");
            }
            catch (Exception)
            {
                //An unknown error occurred. Throw an error message.
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong.");
            }

            return View(book);
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
