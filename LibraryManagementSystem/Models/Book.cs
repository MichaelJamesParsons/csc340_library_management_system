namespace LibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        public string Isbn { get; set; }

        public Book()
        {
            this.CanCheckOut = true;
        }
    }
}