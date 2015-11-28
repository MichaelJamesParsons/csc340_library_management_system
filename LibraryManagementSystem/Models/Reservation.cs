using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Reservation : IModelKey
    {
        public int Id { get; set; }
        
        public int CustomerId { get; set; }
        
        public int LibraryItemId { get; set; }
        
        [ForeignKey("LibraryItemId")]
        public LibraryItem LibraryItem { get; set; }

        [DisplayName("Reserve")]
        public bool IsReserved { get; set; }
        public DateTime CheckOutDate { get; set; }

        public string GetDueDate()
        {
            return CheckOutDate.AddDays(7).ToString("MM/dd/yy");
        }

        public string FormatCheckOutDate(string structure = "")
        {
            structure = (structure != "") ? structure : "MM/dd/yy";
            return CheckOutDate.ToString(structure);
        }

        public double CalculateLateFee()
        {
            var currentDate = DateTime.Today.Date;
            var dueDate = DateTime.Parse(this.GetDueDate());

            return currentDate > dueDate ? (currentDate - dueDate).TotalDays : 0;
        }
    }
}