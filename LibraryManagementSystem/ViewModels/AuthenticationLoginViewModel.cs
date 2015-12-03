using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.ViewModels
{
    /// <summary>
    /// Contains the fields required to login.
    /// </summary>
    public class AuthenticationLoginViewModel
    {
        //The librarian's email
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        //The librarian's plain text password
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //If true, the created session will remain persistant (It will be remembered long term).
        //If false, the session will be terminated shortly after closing the application.
        [DisplayName("Remember Me?")]
        public bool IsPersistant { get; set; }
    }
}