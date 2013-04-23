namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumPostNotification : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumPostNotifications",
                c => new
                    {
                        ForumPostNotificationID = c.Int(nullable: false, identity: true),
                        UserAccountID = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                        ForumSubCategoryID = c.Int(nullable: false),
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumPostNotificationID)
                .ForeignKey("dbo.ForumSubCategories", t => t.ForumSubCategoryID, cascadeDelete: true)
                .Index(t => t.ForumSubCategoryID);

            Sql(@"  EXEC sp_executesql N' ALTER TABLE ForumPostNotifications ADD CONSTRAINT uc_ForumNotificationKey UNIQUE ([UserAccountID], [ForumSubCategoryID])'; ");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumPostNotifications", "ForumSubCategoryID", "dbo.ForumSubCategories");
            DropIndex("dbo.ForumPostNotifications", new[] { "ForumSubCategoryID" });
            DropTable("dbo.ForumPostNotifications");

            Sql(@"EXEC sp_executesql N' ALTER TABLE ForumPostNotifications DROP CONSTRAINT uc_ForumNotificationKey'; ");
        }
    }
}
