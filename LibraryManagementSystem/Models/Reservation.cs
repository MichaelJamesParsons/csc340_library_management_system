using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Reservation : IModelKey
    {
        /// <summary>
        /// The primary key for this object in the database.
        /// </summary>
        public int Id { get; set; }
        

        /// <summary>
        /// The Id of the customer that owns this reservation.
        /// </summary>
        public int CustomerId { get; set; }
        

        /// <summary>
        /// The Id of the library item associated with this reservation.
        /// </summary>
        public int LibraryItemId { get; set; }
        

        /// <summary>
        /// The library item object that contains the item Id provided in 
        /// the previous property.
        /// </summary>
        [ForeignKey("LibraryItemId")]
        public LibraryItem LibraryItem { get; set; }


        /// <summary>
        /// A flag that determines if the item is reserved (true), or
        /// checked out (false).
        /// </summary>
        [DisplayName("Reserve")]
        public bool IsReserved { get; set; }
        public DateTime CheckOutDate { get; set; }


        /// <summary>
        /// Returns the formatted due date relative to the checkout date.
        /// The due date will be calculated as 7 days after the initial checkout date.
        /// </summary>
        /// <returns>The due date.</returns>
        public string GetDueDate()
        {
            return CheckOutDate.AddDays(7).ToString("MM/dd/yy");
        }


        /// <summary>
        /// Formats the reservation's checkout date with a specific pattern. 
        /// Example: MM/dd/yy or MM-dd-yy
        /// </summary>
        /// <param name="structure">The format pattern. Default: MM/dd/yy</param>
        /// <returns>The formatted date</returns>
        public string FormatCheckOutDate(string structure = "")
        {
            structure = (structure != "") ? structure : "MM/dd/yy";
            return CheckOutDate.ToString(structure);
        }

        
        /// <summary>
        /// Calculates the late fee based off of the current date
        /// and the reservation's checkout date. $1 will be charged for
        /// every day the item is late, after the 7 day checkout period.
        /// </summary>
        /// <returns>The late fee.</returns>
        public double CalculateLateFee()
        {
            var currentDate = DateTime.Today.Date;
            var dueDate = DateTime.Parse(this.GetDueDate());

            return currentDate > dueDate ? (currentDate - dueDate).TotalDays : 0;
        }
    }
}