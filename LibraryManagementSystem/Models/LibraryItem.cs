using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class LibraryItem : IModelKey
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [DisplayName("Publication Year")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Invalid publication year (Ex: 1995)")]
        public string PublicationYear { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayName("Type")]
        public string ItemType { get; set; }

        [NotMapped]
        [DisplayName("Allow Checkout")]
        public bool CanCheckOut { get; protected set; }
        public ICollection<Reservation> Reservations { get; set; }

        public string GetItemType()
        {
            return ItemType;
        }

        public static HashSet<string> GetItemTypes()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Book", "CD", "DVD", "Magazine"
            };
        }
    }
}