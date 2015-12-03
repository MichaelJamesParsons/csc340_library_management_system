using System.Collections.Generic;
using System.Data.Entity;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities;

namespace LibraryManagementSystem.DAL.Initializers
{
    /// <summary>
    /// This database initializer populates the database with default data when the tables
    /// are initially migrated.
    /// </summary>
    public class DatabaseInitializer : CreateDatabaseIfNotExists<LibraryDataContext>
    {
        /// <summary>
        /// Seed the default data.
        /// </summary>
        /// <param name="db">DbContext object</param>
        protected override void Seed(LibraryDataContext db)
        {
            //Create a default librarian
            var librarian = new Librarian()
            {
                FirstName = "Dr.",
                LastName  = "Chang",
                Email     = "admin@test.com",
                Password  = Sha256Hasher.Create("admin123")
            };

            //Save the librarian to the database
            db.Librarians.Add(librarian);

            //Create default library items
            var libraryItems = new List<LibraryItem>()
            {
                new Book()
                {
                    Title = "My Book",
                    PublicationYear = "2010",
                    Author = "John Doe",
                    Quantity = 2,
                    ItemType = "Book",
                    Isbn = "978-3-16-148410-0"
                },
                new CD()
                {
                    Title = "My CD",
                    PublicationYear = "2011",
                    Author = "Remix Music Inc.",
                    Quantity = 1,
                    ItemType = "CD"
                },
                new DVD()
                {
                    Title = "My DVD",
                    PublicationYear = "2012",
                    Author = "Awesome Movie Company",
                    Quantity = 1,
                    ItemType = "DVD"
                },
                new Magazine()
                {
                    Title = "My Magazine",
                    PublicationYear = "2013",
                    Author = "Time Magazine",
                    Quantity = 1,
                    ItemType = "Magazine"
                }
            };

            //Save the library items to the database
            db.LibraryItems.AddRange(libraryItems);

            //Create default customers
            var customers = new List<Customer>()
            {
                new Customer()
                {
                    FirstName = "Michael",
                    LastName  = "Parsons",
                    Email     = "michaeljames_pars16@mymail.eku.edu",
                    CustomerNumber = "901528114"
                },
                new Customer()
                {
                    FirstName = "John",
                    LastName  = "Doe",
                    Email     = "john_doe@mymail.eku.edu",
                    CustomerNumber = "901222222"
                },
                new Customer()
                {
                    FirstName = "Jane",
                    LastName  = "Doe",
                    Email     = "jane_doe@mymail.eku.edu",
                    CustomerNumber = "901111111"
                }
            };

            //Save the customers to the database
            db.Customers.AddRange(customers);

            //Complete seeding process
            base.Seed(db);
        }
    }
}