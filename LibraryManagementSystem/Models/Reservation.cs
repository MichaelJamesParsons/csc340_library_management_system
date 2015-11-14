using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using LibraryManagementSystem.Models.Interfaces;

namespace LibraryManagementSystem.Models
{
    public class Reservation : BaseModel, IModelKey
    {
        public int Id { get; set; }
        
        public int Customer_Id { get; set; }
        
        //public Customer Customer { get; set; }
        
        public int LibraryItem_Id { get; set; }
        
        [ForeignKey("LibraryItem_Id")]
        public LibraryItem LibraryItem { get; set; }

        [DisplayName("Reserve")]
        public bool IsReserved { get; set; }
        public DateTime CheckOutDate { get; set; }

        public string GetDueDate()
        {
            return CheckOutDate.AddDays(7).ToString("dd.MM.yy");
        }
    }
}