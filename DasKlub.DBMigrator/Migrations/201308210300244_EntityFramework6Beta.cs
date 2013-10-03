using System.Data.Entity.Migrations;

namespace DasKlub.DBMigrator.Migrations
{
    public partial class EntityFramework6Beta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumCategories",
                c => new
                    {
                        ForumCategoryID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Key = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false),
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumCategoryID);
            
            CreateTable(
                "dbo.ForumSubCategories",
                c => new
                    {
                        ForumSubCategoryID = c.Int(nullable: false, identity: true),
                        ForumCategoryID = c.Int(nullable: false),
                        Key = c.String(nullable: false, maxLength: 150),
                        Title = c.String(nullable: false, maxLength: 150),
                        Description = c.String(nullable: false),
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumSubCategoryID)
                .ForeignKey("dbo.ForumCategories", t => t.ForumCategoryID, cascadeDelete: true)
                .Index(t => t.ForumCategoryID);
            
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        ForumPostID = c.Int(nullable: false, identity: true),
                        Detail = c.String(nullable: false),
                        ForumSubCategoryID = c.Int(nullable: false),
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumPostID)
                .ForeignKey("dbo.ForumSubCategories", t => t.ForumSubCategoryID, cascadeDelete: true)
                .Index(t => t.ForumSubCategoryID);
            
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
            Sql(@"ALTER TABLE dbo.WallMessage
                  ALTER COLUMN [message] nvarchar(max)");
        }
        
        public override void Down()
        {
            Sql(@"EXEC sp_executesql N' ALTER TABLE ForumPostNotifications DROP CONSTRAINT uc_ForumNotificationKey'; ");

            Sql(@"ALTER TABLE dbo.WallMessage
                  ALTER COLUMN [message]  varchar(max)");

            DropForeignKey("dbo.ForumPostNotifications", "ForumSubCategoryID", "dbo.ForumSubCategories");
            DropForeignKey("dbo.ForumPosts", "ForumSubCategoryID", "dbo.ForumSubCategories");
            DropForeignKey("dbo.ForumSubCategories", "ForumCategoryID", "dbo.ForumCategories");
            DropIndex("dbo.ForumPostNotifications", new[] { "ForumSubCategoryID" });
            DropIndex("dbo.ForumPosts", new[] { "ForumSubCategoryID" });
            DropIndex("dbo.ForumSubCategories", new[] { "ForumCategoryID" });
            DropTable("dbo.ForumPostNotifications");
            DropTable("dbo.ForumPosts");
            DropTable("dbo.ForumSubCategories");
            DropTable("dbo.ForumCategories");
        }
    }
}
