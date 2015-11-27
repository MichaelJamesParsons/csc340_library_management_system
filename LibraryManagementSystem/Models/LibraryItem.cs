using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.WebSockets;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class LibraryItem : BaseModel, IModelKey
    {
        public int Id { get; set; }
        public string Title { get; set; }

        [DisplayName("Publication Year")]
        public DateTime PublicationYear { get; set; }
        public string Author { get; set; }
        public int Quantity { get; set; }

        [Required]
        [DisplayName("Type")]
        public string ItemType { get; set; }

        [DisplayName("Allow Checkout")]
        public bool CanCheckOut { get; protected set; }
        public ICollection<Reservation> Reservations { get; set; }

        public LibraryItem()
        {
            LibraryItem.Controller = "";
        }

        public string GetItemType()
        {
            return ItemType;
        }

        public string GetPublicationYear()
        {
            return PublicationYear.ToString("MM/dd/yy");
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