using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Customer : IModelKey
    {
        /// <summary>
        /// The primary key for this object in the database.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// A unique customer number.
        /// This number must start with "901" and contain 6 digits following the initial 3.
        /// </summary>
        [Required]
        [Index("LibrarianUsernameIndex", 1, IsUnique = true)]
        [RegularExpression("^(901)[0-9]{6}$", ErrorMessage = "Please enter a valid 9 digit customer number.")]
        [StringLength(9)]
        [DisplayName("Customer Number")]
        public string CustomerNumber { get; set; }


        /// <summary>
        /// The customer's first name.
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Your first name may not contain more than 30 characters."),
            MinLength(2, ErrorMessage = "Your first name must contain at least 2 characters.")]
        [DisplayName("First Name")]
        public string FirstName { get; set; }


        /// <summary>
        /// The customer's last name.
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Your last name may not contain more than 30 characters."),
            MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        [DisplayName("Last Name")]
        public string LastName { get; set; }


        /// <summary>
        /// The customer's email.
        /// </summary>
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+(@(mymail\.)?eku\.edu)$", ErrorMessage = "You must use your EKU email.")]
        public string Email { get; set; } 


        /// <summary>
        /// A collection of the customer's reservations.
        /// </summary>
        public ICollection<Reservation> Reservations { get; set; }


        /// <summary>
        /// Get the customer's full name.
        /// </summary>
        /// <returns>The customer's full name.</returns>
        public string GetFullName()
        {
            return $"{FirstName} {LastName}";
        }
    }
}