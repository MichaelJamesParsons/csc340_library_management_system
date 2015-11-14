using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Customer : BaseModel, IModelKey
    {
        public int Id { get; set; }

        [Required]
        [Index("LibrarianUsernameIndex", 1, IsUnique = true)]
        [RegularExpression("^(901)[0-9]{6}$", ErrorMessage = "Please enter a valid 9 digit customer number.")]
        [DisplayName("Customer Number")]
        public string CustomerNumber { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Your first name may not contain more than 30 characters."),
            MinLength(2, ErrorMessage = "Your first name must contain at least 2 characters.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Your last name may not contain more than 30 characters."),
            MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@(mymail\.)?eku\.edu)$", ErrorMessage = "You must use your EKU email.")]
        public string Email { get; set; } 

        public ICollection<Reservation> Reservations { get; set; }

        /*public Customer()
        {
            this.Reservations = new HashSet<Reservation>();
        }*/

        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}