namespace LibraryManagementSystem.Models
{
    public class CD : LibraryItem
    {
        /// <summary>
        /// Sets the CD's ability to be checked out / reserved.
        /// </summary>
        public CD()
        {
            CanCheckOut = true;
        }
    }
}