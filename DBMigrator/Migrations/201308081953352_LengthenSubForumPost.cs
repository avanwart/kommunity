namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LengthenSubForumPost : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ForumSubCategories", "Description", c => c.String(nullable: false, maxLength: 420));
        }
    }
}
