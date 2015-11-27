using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.DAL;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class LibraryItemsController : Controller
    {
        private LibraryDataContext db = new LibraryDataContext();
        private readonly ILibraryItemRepository _libraryItemRepository;

        public LibraryItemsController(ILibraryItemRepository libraryItemRepository)
        {
            this._libraryItemRepository = libraryItemRepository;
        }

        // GET: LibraryItems
        public ActionResult Index()
        {
            return View(db.LibraryItems.ToList());
        }

        [HttpGet]
        public ActionResult Search([Bind(Include = "FieldName, ItemType, Query")] LibraryItemsSearchViewModel search)
        {
            try
            {
                var searchType  = (!string.IsNullOrEmpty(search.ItemType)) ? search.ItemType : "";
                var searchKey   = (!string.IsNullOrEmpty(search.FieldName)) ? search.FieldName : "";
                var searchQuery = (!string.IsNullOrEmpty(search.Query)) ? search.Query : "";

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    var searchResults = _libraryItemRepository.SearchLibraryItems(searchType, searchKey, searchQuery);
                    ViewBag.searchResults = searchResults;
                    search.Results = searchResults;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong. Please try another search.");
            }

            return View(search);
        }
        
        // GET: LibraryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryItem libraryItem = db.LibraryItems.Find(id);
            if (libraryItem == null)
            {
                return HttpNotFound();
            }
            return View(libraryItem);
        }

        // GET: LibraryItems/Create
        public ActionResult Create(string type)
        {
            var allowedLibraryItemTypes = LibraryItem.GetItemTypes();
            
            if(!allowedLibraryItemTypes.Contains(type))
               return HttpNotFound();

            ViewBag.itemType = type.First().ToString().ToUpper() + type.Substring(1);
            return View();
        }

        // POST: LibraryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ItemType,PublicationYear,Author,Quantity,CanCheckOut")] LibraryItem libraryItem)
        {
            if (ModelState.IsValid)
            {
                db.LibraryItems.Add(libraryItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(libraryItem);
        }

        // GET: LibraryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryItem libraryItem = db.LibraryItems.Find(id);
            if (libraryItem == null)
            {
                return HttpNotFound();
            }
            return View(libraryItem);
        }

        // POST: LibraryItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PublicationYear,Author,Quantity,CanCheckOut")] LibraryItem libraryItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(libraryItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(libraryItem);
        }

        // GET: LibraryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LibraryItem libraryItem = db.LibraryItems.Find(id);
            if (libraryItem == null)
            {
                return HttpNotFound();
            }
            return View(libraryItem);
        }

        // POST: LibraryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LibraryItem libraryItem = db.LibraryItems.Find(id);
            db.LibraryItems.Remove(libraryItem);
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
