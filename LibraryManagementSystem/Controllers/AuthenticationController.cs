using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LibraryManagementSystem.Controllers
{
    /// <summary>
    /// Handles Login/Logout and other session related pages and functionality.
    /// </summary>
    public class AuthenticationController : Controller
    {
        //Contains all of the cached Librarian objects
        private readonly ILibrarianRepository _librarianRepository;

        //ASP.NET authentication handler.
        //Documentation: http://www.asp.net/identity/overview/getting-started/introduction-to-aspnet-identity
        IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication;

        public AuthenticationController(ILibrarianRepository librarianRepository)
        {
            _librarianRepository = librarianRepository;
        }

        /// <summary>
        /// Displays the login page.
        /// Route: /authentication/login
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// Handles POST request for login view. Is the user's credentials are correct,
        /// a session will be created.
        /// </summary>
        /// <param name="viewModel">Instance of Models/ViewModels/AuthenticationLoginViewModel</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AuthenticationLoginViewModel viewModel)
        {
            //Check for validation errors in the model object
            if (!ModelState.IsValid)
                return View(viewModel);

            //Hash the password submitted from the form. The hashed password
            //will be compared to the password hash in the database.
            var hashedPassword = Sha256Hasher.Create(viewModel.Password);

            //Find the librarian with the given email and password combination
            var librarian = _librarianRepository.FindLibrarianByEmailAndPassword(viewModel.Email, hashedPassword);

            //Does the librarian exist?
            if (librarian != null)
            {
                //Create a list of cookies to store in the user's session.
                //The first Claim, "NameIdentifier" will be used as the primary
                //session identifier. The other claims (Name, Surname, Email, Role) will be
                //stored as additional information about the logged in user.
                var identity = new ClaimsIdentity(
                    new [] 
                    {
                        new Claim(ClaimTypes.NameIdentifier, $"{librarian.Id}"),
                        new Claim(ClaimTypes.Name, librarian.FirstName),
                        new Claim(ClaimTypes.Surname, librarian.LastName),
                        new Claim(ClaimTypes.Email, librarian.Email),
                        new Claim(ClaimTypes.Role, "Librarian") 
                    },
                    DefaultAuthenticationTypes.ApplicationCookie,
                    ClaimTypes.NameIdentifier, ClaimTypes.Role
                    );

                //Create the session with the list of cookies defined above
                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = viewModel.IsPersistant
                }, identity);

                //Redirect to the home page (dashboard)
                return RedirectToAction("Index","Home");
            }

            //The librarian doesn't exist. Send error back to login page view
            ModelState.AddModelError(string.Empty, "Invalid email/password combination.");

            return View(viewModel);
        }


        /// <summary>
        /// Logs user out of current session. The user must be logged in to access
        /// this page.
        /// Route: /authentication/logout
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            //Destroy session using ASP.net's Owin API.
            //See initialization of Authentication at the top of this document
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }
    }
}