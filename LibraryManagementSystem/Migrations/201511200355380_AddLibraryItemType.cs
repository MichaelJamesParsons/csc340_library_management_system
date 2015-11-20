namespace LibraryManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLibraryItemType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.LibraryItems", "ItemType", c => c.String(nullable: false, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.LibraryItems", "ItemType", c => c.String(unicode: false));
        }
    }
}
