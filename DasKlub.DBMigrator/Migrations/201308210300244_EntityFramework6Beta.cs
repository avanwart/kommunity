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
                    ForumCategoryID = c.Int(false, true),
                    Title = c.String(false, 50),
                    Key = c.String(false, 50),
                    Description = c.String(false),
                    CreatedByUserID = c.Int(false),
                    UpdatedByUserID = c.Int(),
                    CreateDate = c.DateTime(false),
                    UpdateDate = c.DateTime(),
                })
                .PrimaryKey(t => t.ForumCategoryID);

            CreateTable(
                "dbo.ForumSubCategories",
                c => new
                {
                    ForumSubCategoryID = c.Int(false, true),
                    ForumCategoryID = c.Int(false),
                    Key = c.String(false, 150),
                    Title = c.String(false, 150),
                    Description = c.String(false),
                    CreatedByUserID = c.Int(false),
                    UpdatedByUserID = c.Int(),
                    CreateDate = c.DateTime(false),
                    UpdateDate = c.DateTime(),
                })
                .PrimaryKey(t => t.ForumSubCategoryID)
                .ForeignKey("dbo.ForumCategories", t => t.ForumCategoryID, true)
                .Index(t => t.ForumCategoryID);

            CreateTable(
                "dbo.ForumPosts",
                c => new
                {
                    ForumPostID = c.Int(false, true),
                    Detail = c.String(false),
                    ForumSubCategoryID = c.Int(false),
                    CreatedByUserID = c.Int(false),
                    UpdatedByUserID = c.Int(),
                    CreateDate = c.DateTime(false),
                    UpdateDate = c.DateTime(),
                })
                .PrimaryKey(t => t.ForumPostID)
                .ForeignKey("dbo.ForumSubCategories", t => t.ForumSubCategoryID, true)
                .Index(t => t.ForumSubCategoryID);

            CreateTable(
                "dbo.ForumPostNotifications",
                c => new
                {
                    ForumPostNotificationID = c.Int(false, true),
                    UserAccountID = c.Int(false),
                    IsRead = c.Boolean(false),
                    ForumSubCategoryID = c.Int(false),
                    CreatedByUserID = c.Int(false),
                    UpdatedByUserID = c.Int(),
                    CreateDate = c.DateTime(false),
                    UpdateDate = c.DateTime(),
                })
                .PrimaryKey(t => t.ForumPostNotificationID)
                .ForeignKey("dbo.ForumSubCategories", t => t.ForumSubCategoryID, true)
                .Index(t => t.ForumSubCategoryID);
            Sql(
                @"  EXEC sp_executesql N' ALTER TABLE ForumPostNotifications ADD CONSTRAINT uc_ForumNotificationKey UNIQUE ([UserAccountID], [ForumSubCategoryID])'; ");
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
            DropIndex("dbo.ForumPostNotifications", new[] {"ForumSubCategoryID"});
            DropIndex("dbo.ForumPosts", new[] {"ForumSubCategoryID"});
            DropIndex("dbo.ForumSubCategories", new[] {"ForumCategoryID"});
            DropTable("dbo.ForumPostNotifications");
            DropTable("dbo.ForumPosts");
            DropTable("dbo.ForumSubCategories");
            DropTable("dbo.ForumCategories");
        }
    }
}