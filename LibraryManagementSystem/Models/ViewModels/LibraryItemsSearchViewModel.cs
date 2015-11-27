using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.Models.ViewModels
{
    public class LibraryItemsSearchViewModel
    {
        public string FieldName { get; set; }
        public string ItemType { get; set; }
        public string Query { get; set; }

        public ICollection<LibraryItem> Results { get; set; } = new List<LibraryItem>();

        public List<SelectListItem> SearchableFieldsMenu = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Title", Value = "Title"},
            new SelectListItem() { Text = "Author", Value = "Author"},
            new SelectListItem() { Text = "ISBN", Value = "Isbn"},
            new SelectListItem() { Text = "Publication Year", Value = "PublicationYear"},
        };


        public LibraryItemsSearchViewModel()
        {
            FieldName   = "";
            ItemType    = "";
            Query       = "";
        }


        public List<SelectListItem> GetSearchableItemTypesMenu()
        {
            var menu = new List<SelectListItem>();
            menu.AddRange(LibraryItem.GetItemTypes().Select(type => new SelectListItem() {Text = type, Value = type}));

            return menu;
        }
    }
}