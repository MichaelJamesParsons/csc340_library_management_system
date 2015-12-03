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
        /// <summary>
        /// The primary key for this object in the database.
        /// </summary>
        public int Id { get; set; }


        /// <summary>
        /// The item's title.
        /// </summary>
        [Required]
        public string Title { get; set; }


        /// <summary>
        /// The year that the item was published, released, or otherwise made available to the public.
        /// </summary>
        [DisplayName("Publication Year")]
        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Invalid publication year (Ex: 1995)")]
        public string PublicationYear { get; set; }


        /// <summary>
        /// The individual or organization the wrote/created the library item.
        /// </summary>
        [Required]
        public string Author { get; set; }


        /// <summary>
        /// The number of copies owned by the library.
        /// This does not represent the number of remaining copies left.
        /// </summary>
        [Required]
        public int Quantity { get; set; }


        /// <summary>
        /// Identifies the item's type, such as book, CD, DVD, etc.
        /// </summary>
        [Required]
        [DisplayName("Type")]
        public string ItemType { get; set; }


        /// <summary>
        /// Determines if the item is alloed to be checked out.
        /// 
        /// This property is not stored in the database, but rather
        /// set by the constructor.
        /// </summary>
        [NotMapped]
        [DisplayName("Allow Checkout")]
        public bool CanCheckOut { get; protected set; }
        public ICollection<Reservation> Reservations { get; set; }

        /// <summary>
        /// Returns the item's type.
        /// </summary>
        /// <returns>The item's type.</returns>
        public string GetItemType()
        {
            return ItemType;
        }


        /// <summary>
        /// A list of item available item types.
        /// </summary>
        /// <returns></returns>
        public static HashSet<string> GetItemTypes()
        {
            return new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Book", "CD", "DVD", "Magazine"
            };
        }
    }
}