using System.Collections.Generic;
using System.Data.Entity;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Utilities;

namespace LibraryManagementSystem.DAL.Initializers
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<LibraryDataContext>
    {
        protected override void Seed(LibraryDataContext db)
        {
            var librarian = new Librarian()
            {
                FirstName = "Dr.",
                LastName  = "Chang",
                Email     = "admin@test.com",
                Password  = SHA256Hasher.Create("admin123")
            };

            db.Librarians.Add(librarian);

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
                    Quantity = 5,
                    ItemType = "CD"
                },
                new DVD()
                {
                    Title = "My DVD",
                    PublicationYear = "2012",
                    Author = "Awesome Movie Company",
                    Quantity = 5,
                    ItemType = "DVD"
                },
                new Magazine()
                {
                    Title = "My Magazine",
                    PublicationYear = "2013",
                    Author = "Time Magazine",
                    Quantity = 5,
                    ItemType = "Magazine"
                }
            };

            db.LibraryItems.AddRange(libraryItems);

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
                    Email     = "john_doe@mymail.eku.edu",
                    CustomerNumber = "901111111"
                }
            };

            db.Customers.AddRange(customers);

            base.Seed(db);
        }
    }
}