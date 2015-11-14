using System.Data.Entity;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class LibraryDataContext : DbContext
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Librarian> Librarians { get; set; }
        public virtual DbSet<LibraryItem> LibraryItems { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        public LibraryDataContext() : base("DefaultConnection") {}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasMany(x => x.Reservations).WithRequired().HasForeignKey(x => x.Customer_Id);
            modelBuilder.Entity<LibraryItem>().HasMany(x => x.Reservations).WithRequired().HasForeignKey(x => x.LibraryItem_Id);
        }
    }
}