namespace LibraryManagementSystem.Models
{
    public class DVD : LibraryItem
    {
        /// <summary>
        /// Sets the DVD's ability to be checked out / reserved.
        /// </summary>
        public DVD()
        {
            CanCheckOut = true;
        }
    }
}