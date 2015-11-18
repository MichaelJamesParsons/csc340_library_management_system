using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    public class LibraryItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private readonly ILibraryItemRepository _libraryItemRepository;

        public LibraryItemsController(ILibraryItemRepository libraryItemRepository)
        {
            this._libraryItemRepository = libraryItemRepository;
        }

        // GET: LibraryItems
        public ActionResult Index()
        {
            var items = db.LibraryItems.Include("LibraryItems.ISBN");
            return View(db.LibraryItems.ToList());
        }


        public ActionResult Search()
        {
            try
            {
                var searchResults = _libraryItemRepository.GetAll();
            }
            catch (Exception e)
            {
                
            }

            ViewBag.searchResults = null;

            return View();
        }

        /*
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: LibraryItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,PublicationYear,Author,Quantity,CanCheckOut")] LibraryItem libraryItem)
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
        */
    }
}
