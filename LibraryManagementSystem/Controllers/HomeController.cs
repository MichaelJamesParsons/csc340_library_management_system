using System.Web.Mvc;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// This controller handles pages that are not specific to domain objects or 
    /// application modules. The user must be logged in to view these pages.
    /// </summary>
    [Authorize]
    public class HomeController : Controller
    {
        /// <summary>
        /// Displays the main dashboard the the librarian sees after logging in.
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }
    }
}