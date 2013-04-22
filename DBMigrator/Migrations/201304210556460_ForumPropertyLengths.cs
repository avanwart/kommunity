namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumPropertyLengths : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ForumCategories", "Key", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.ForumSubCategories", "Key", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.ForumCategories", "Title", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.ForumSubCategories", "Title", c => c.String(nullable: false, maxLength: 150));
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String(nullable: false, maxLength: 420));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ForumSubCategories", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.ForumCategories", "Title", c => c.String(nullable: false));
            DropColumn("dbo.ForumSubCategories", "Key");
            DropColumn("dbo.ForumCategories", "Key");
        }
    }
}
