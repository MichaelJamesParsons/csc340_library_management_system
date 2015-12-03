namespace LibraryManagementSystem.Models
{
    public class Magazine : LibraryItem
    {
        /// <summary>
        /// Sets the Magazine's ability to be checked out / reserved.
        /// </summary>
        public Magazine()
        {
            CanCheckOut = false;
        }
    }
}