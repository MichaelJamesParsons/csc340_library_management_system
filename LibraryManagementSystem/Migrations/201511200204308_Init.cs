namespace LibraryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerNumber = c.String(nullable: false, maxLength: 9, storeType: "nvarchar"),
                        FirstName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        LastName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CustomerNumber, unique: true, name: "LibrarianUsernameIndex");
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Customer_Id = c.Int(nullable: false),
                        LibraryItem_Id = c.Int(nullable: false),
                        IsReserved = c.Boolean(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LibraryItems", t => t.LibraryItem_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.Customer_Id)
                .Index(t => t.LibraryItem_Id);
            
            CreateTable(
                "dbo.LibraryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(unicode: false),
                        PublicationYear = c.DateTime(nullable: false, precision: 0),
                        Author = c.String(unicode: false),
                        Quantity = c.Int(nullable: false),
                        ItemType = c.String(unicode: false),
                        CanCheckOut = c.Boolean(nullable: false),
                        Isbn = c.String(unicode: false),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Librarians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(nullable: false, maxLength: 50, storeType: "nvarchar"),
                        Password = c.String(nullable: false, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        LastName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Username, unique: true, name: "LibrarianUsernameIndex")
                .Index(t => t.Email, unique: true, name: "LibrarianEmailIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Reservations", "LibraryItem_Id", "dbo.LibraryItems");
            DropIndex("dbo.Librarians", "LibrarianEmailIndex");
            DropIndex("dbo.Librarians", "LibrarianUsernameIndex");
            DropIndex("dbo.Reservations", new[] { "LibraryItem_Id" });
            DropIndex("dbo.Reservations", new[] { "Customer_Id" });
            DropIndex("dbo.Customers", "LibrarianUsernameIndex");
            DropTable("dbo.Librarians");
            DropTable("dbo.LibraryItems");
            DropTable("dbo.Reservations");
            DropTable("dbo.Customers");
        }
    }
}
