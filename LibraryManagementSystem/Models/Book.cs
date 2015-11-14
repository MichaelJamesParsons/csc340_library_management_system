namespace LibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        public string Isbn { get; set; }

        public Book()
        {
            this.CanCheckOut = true;
            Book.ItemType = "Book";
            Book.Controller = "Books";
            Book.TableContext = Db.LibraryItems;
        }
    }
}