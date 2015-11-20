using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Librarian : IModelKey
    {
        public int Id { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Your password must contain at least 8 characters in length.")]
        public string Password { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Your first name may not contain more than 30 characters."), 
            MinLength(2, ErrorMessage = "Your first name must contain at least 2 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Your last name may not contain more than 30 characters."), 
            MinLength(2, ErrorMessage = "Your last name must contain at least 2 characters.")]
        public string LastName { get; set; }

        [Index("LibrarianEmailIndex", 1, IsUnique = true)]
        public string Email { get; set; }
    }
}