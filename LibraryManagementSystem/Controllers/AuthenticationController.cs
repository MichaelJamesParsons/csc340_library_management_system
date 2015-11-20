﻿using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using LibraryManagementSystem.DAL.Interfaces;
using LibraryManagementSystem.Utilities;
using LibraryManagementSystem.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LibraryManagementSystem.Controllers
{

    public class AuthenticationController : Controller
    {
        private readonly ILibrarianRepository _librarianRepository;
        IAuthenticationManager Authentication => HttpContext.GetOwinContext().Authentication;

        public AuthenticationController(ILibrarianRepository librarianRepository)
        {
            _librarianRepository = librarianRepository;
        }

        // GET: Authentication
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public ActionResult Login(AuthenticationLoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var hashedPassword = SHA256Hasher.Create(viewModel.Password);
            var librarian = _librarianRepository.FindLibrarianByEmailAndPassword(viewModel.Email, hashedPassword);

            if (librarian != null)
            {
                var identity = new ClaimsIdentity(
                    new [] 
                    {
                        new Claim(ClaimTypes.Name, viewModel.Email), 
                    },
                    DefaultAuthenticationTypes.ApplicationCookie,
                    ClaimTypes.Name, ClaimTypes.Role
                    );

                identity.AddClaim(new Claim(ClaimTypes.Role, "librarian"));

                Authentication.SignIn(new AuthenticationProperties
                {
                    IsPersistent = viewModel.IsPersistant
                }, identity);

                return RedirectToAction("Index","Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email/password combination.");

            return View(viewModel);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Logout()
        {
            Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

    }
}