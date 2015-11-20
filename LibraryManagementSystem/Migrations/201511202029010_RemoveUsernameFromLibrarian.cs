namespace LibraryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveUsernameFromLibrarian : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Librarians", "LibrarianUsernameIndex");
            DropColumn("dbo.Librarians", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Librarians", "Username", c => c.String(nullable: false, maxLength: 50, storeType: "nvarchar"));
            CreateIndex("dbo.Librarians", "Username", unique: true, name: "LibrarianUsernameIndex");
        }
    }
}
