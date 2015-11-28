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
                        Email = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CustomerNumber, unique: true, name: "LibrarianUsernameIndex");
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        LibraryItemId = c.Int(nullable: false),
                        IsReserved = c.Boolean(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LibraryItems", t => t.LibraryItemId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.LibraryItemId);
            
            CreateTable(
                "dbo.LibraryItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, unicode: false),
                        PublicationYear = c.String(unicode: false),
                        Author = c.String(nullable: false, unicode: false),
                        Quantity = c.Int(nullable: false),
                        ItemType = c.String(nullable: false, unicode: false),
                        Isbn = c.String(unicode: false),
                        Discriminator = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Librarians",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Password = c.String(nullable: false, unicode: false),
                        FirstName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        LastName = c.String(nullable: false, maxLength: 30, storeType: "nvarchar"),
                        Email = c.String(maxLength: 100, storeType: "nvarchar"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true, name: "LibrarianEmailIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Reservations", "LibraryItemId", "dbo.LibraryItems");
            DropIndex("dbo.Librarians", "LibrarianEmailIndex");
            DropIndex("dbo.Reservations", new[] { "LibraryItemId" });
            DropIndex("dbo.Reservations", new[] { "CustomerId" });
            DropIndex("dbo.Customers", "LibrarianUsernameIndex");
            DropTable("dbo.Librarians");
            DropTable("dbo.LibraryItems");
            DropTable("dbo.Reservations");
            DropTable("dbo.Customers");
        }
    }
}
