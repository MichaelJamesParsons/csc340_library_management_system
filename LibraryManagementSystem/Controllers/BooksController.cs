using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Books
        /*public ActionResult Index()
        {
            ICollection<LibraryItem> items = db.LibraryItems.ToList();
            ICollection<Book> books = new List<Book>();

            foreach (LibraryItem item in items)
            {
                books.Add((Book)item);
            }

            return View(books.ToList());
        }*/

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = (Book)db.LibraryItems.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,PublicationYear,Author,Quantity,CanCheckOut,Isbn")] Book book)
        {
            if (!ModelState.IsValid)
                return View(book);
            
            book.ItemType = "Book";
            db.LibraryItems.Add(book);
            db.SaveChanges();
            return RedirectToAction("Index", "LibraryItems");
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var book = db.LibraryItems.OfType<Book>().FirstOrDefault(x => x.Id == id);
            
            if (book == null)
                return HttpNotFound();

            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,PublicationYear,Author,Quantity,CanCheckOut,Isbn")] Book book)
        {
            if (!ModelState.IsValid)
                return View(book);

            db.Entry(book).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "LibraryItems");
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = (Book)db.LibraryItems.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = (Book)db.LibraryItems.Find(id);
            db.LibraryItems.Remove(book);
            db.SaveChanges();
            return RedirectToAction("Index", "LibraryItems");
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
