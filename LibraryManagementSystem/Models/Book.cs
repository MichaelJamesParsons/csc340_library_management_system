namespace LibraryManagementSystem.Models
{
    public class Book : LibraryItem
    {
        /// <summary>
        /// The book's optional ISBN.
        /// </summary>
        public string Isbn { get; set; }

        /// <summary>
        /// Sets the book's ability to be checked out / reserved.
        /// </summary>
        public Book()
        {
            CanCheckOut = true;
        }
    }
}