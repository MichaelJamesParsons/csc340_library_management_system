using System.Web.Mvc;

namespace LibraryManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}