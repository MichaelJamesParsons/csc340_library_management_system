using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Models.ViewModels;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class LibraryItemsController : Controller
    {
        private readonly ILibraryItemRepository _libraryItemRepository;

        public LibraryItemsController(ILibraryItemRepository libraryItemRepository)
        {
            _libraryItemRepository = libraryItemRepository;
        }

        
        public ActionResult Index()
        {
            return View(_libraryItemRepository.GetAll());
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
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Oops! Something went wrong. Please try another search.");
            }

            return View(search);
        }
        
        
        public ActionResult Details(int? id)
        {
            var libraryItem = _libraryItemRepository.Find(id);

            if (libraryItem == null)
               return HttpNotFound();

            return View(libraryItem);
        }

        
        public ActionResult Create(string type)
        {
            var allowedLibraryItemTypes = LibraryItem.GetItemTypes();
            
            if(!allowedLibraryItemTypes.Contains(type))
               return HttpNotFound();

            ViewBag.itemType = type.First().ToString().ToUpper() + type.Substring(1);
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,ItemType,PublicationYear,Author,Quantity,CanCheckOut")] LibraryItem libraryItem)
        {
            if (!ModelState.IsValid)
                return View(libraryItem);

            _libraryItemRepository.Add(libraryItem);
            _libraryItemRepository.Save();

            return RedirectToAction("Index");
        }

        
        public ActionResult Edit(int? id)
        {
            var libraryItem = _libraryItemRepository.Find(id);

            if (libraryItem == null)
                return HttpNotFound();

            return View(libraryItem);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PublicationYear,Author,ItemType,Quantity,CanCheckOut")] LibraryItem libraryItem)
        {
            if (!ModelState.IsValid)
                return View(libraryItem);

            _libraryItemRepository.Edit(libraryItem);
            _libraryItemRepository.Save();

            return RedirectToAction("Index");
        }

        
        public ActionResult Delete(int? id)
        {
            var libraryItem = _libraryItemRepository.Find(id);

            if (libraryItem == null)
                return HttpNotFound();

            return View(libraryItem);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var libraryItem = _libraryItemRepository.FindBy(x => x.Id == id).Include("Reservations").FirstOrDefault();

            //When the repository attempts to delete the library item, if the item is null, it will
            //simply not do anything, so a nullable check is only required here.
            if (libraryItem != null && libraryItem.Reservations.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "This item has been checked out by one or more customers.");
                return View(libraryItem);
            }

            _libraryItemRepository.Delete(libraryItem);
            _libraryItemRepository.Save();

            return RedirectToAction("Index");
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
