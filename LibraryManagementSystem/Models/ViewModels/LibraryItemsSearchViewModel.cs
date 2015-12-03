using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace LibraryManagementSystem.Models.ViewModels
{
    /// <summary>
    /// Contains the fields required to search for a library item by type and property value.
    /// </summary>
    public class LibraryItemsSearchViewModel
    {
        /// <summary>
        /// The LibraryItem property to search by.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The type of library item to search for.
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// The search keywords.
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Stores the results from a search query
        /// </summary>
        public ICollection<LibraryItem> Results { get; set; } = new List<LibraryItem>();

        /// <summary>
        /// The properties that are allowed to be searched by when searching for 
        /// library items.
        /// </summary>
        public List<SelectListItem> SearchableFieldsMenu = new List<SelectListItem>()
        {
            new SelectListItem() { Text = "Title", Value = "Title"},
            new SelectListItem() { Text = "Author", Value = "Author"},
            new SelectListItem() { Text = "ISBN", Value = "Isbn"},
            new SelectListItem() { Text = "Publication Year", Value = "PublicationYear"},
        };


        /// <summary>
        /// The LibraryItemsSearchViewModel constructor sets default values 
        /// for all of the nullable properties of this class.
        /// </summary>
        public LibraryItemsSearchViewModel()
        {
            FieldName   = "";
            ItemType    = "";
            Query       = "";
        }


        /// <summary>
        /// Generates a select list menu of the valid types of library items.
        /// </summary>
        /// <returns>A select list menu</returns>
        public List<SelectListItem> GetSearchableItemTypesMenu()
        {
            var menu = new List<SelectListItem>();
            menu.AddRange(LibraryItem.GetItemTypes().Select(type => new SelectListItem() {Text = type, Value = type}));

            return menu;
        }
    }
}