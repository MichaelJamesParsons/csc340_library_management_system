using System.Data.Entity;
using LibraryManagementSystem.DAL;

namespace LibraryManagementSystem.Models
{
    public class BaseModel
    {
        protected static LibraryDataContext Db = new LibraryDataContext();
        public static string Controller { get; set; }
        public static DbSet TableContext { get; set; }


        public BaseModel()
        {
            BaseModel.TableContext = null;
        }

        public string GetController()
        {
            return Controller;
        }
    }
}