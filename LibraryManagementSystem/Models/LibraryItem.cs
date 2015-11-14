using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Type")]
        public static string ItemType { get; set; }

        [DisplayName("Allow Checkout")]
        public bool CanCheckOut { get; protected set; }
        public ICollection<Reservation> Reservations { get; set; }

        public LibraryItem()
        {
            LibraryItem.ItemType = "";
            LibraryItem.Controller = "";
        }

        public string GetItemType()
        {
            return ItemType;
        }
    }
}