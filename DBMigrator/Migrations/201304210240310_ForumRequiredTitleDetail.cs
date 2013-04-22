namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumRequiredTitleDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumCategories", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.ForumCategories", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ForumSubCategories", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.ForumPosts", "Detail", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumPosts", "Detail", c => c.String());
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String());
            AlterColumn("dbo.ForumSubCategories", "Title", c => c.String());
            AlterColumn("dbo.ForumCategories", "Description", c => c.String());
            AlterColumn("dbo.ForumCategories", "Title", c => c.String());
        }
    }
}
