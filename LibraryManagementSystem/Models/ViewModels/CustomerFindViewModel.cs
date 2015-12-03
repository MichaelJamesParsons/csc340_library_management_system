using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Models.ViewModels
{
    /// <summary>
    /// Contains the fields required to search for a customer by CustomerNumber.
    /// </summary>
    public class CustomerFindViewModel
    {
        [Required]
        [Index("LibrarianUsernameIndex", 1, IsUnique = true)]
        [RegularExpression("^(901)[0-9]{6}$", ErrorMessage = "Please enter a valid 9 digit customer number.")]
        [StringLength(9)]
        [DisplayName("Customer Number")]
        public string CustomerNumber { get; set; }
    }
}