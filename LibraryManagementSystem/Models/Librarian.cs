using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Librarian : IModelKey
    {
        /// <summary>
        /// The primary key for this object in the database.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The librarian's first name.
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Your first name may not contain more than 30 characters."), 
            MinLength(2, ErrorMessage = "Your first name must contain at least 2 characters.")]
        public string FirstName { get; set; }


        /// <summary>
        /// The librarian's last name.
        /// </summary>
        [Required]
        [MaxLength(30, ErrorMessage = "Your last name may not contain more than 30 characters."), 
            MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        public string LastName { get; set; }


        /// <summary>
        /// The librarian's email.
        /// </summary>
        [MaxLength(100, ErrorMessage = "Email address is too long (must contain less than 100 characters)")]
        [Index("LibrarianEmailIndex", 1, IsUnique = true)]
        public string Email { get; set; }


        /// <summary>
        /// The librarian's hashed password (Uses Sha256 hash).
        /// </summary>
        [Required]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters in length.")]
        public string Password { get; set; }
    }
}