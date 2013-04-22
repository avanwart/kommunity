namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ForumCategories",
                c => new
                    {
                        ForumCategoryID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
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
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        Title = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.ForumSubCategoryID)
                .ForeignKey("dbo.ForumCategories", t => t.ForumCategoryID, cascadeDelete: true)
                .Index(t => t.ForumCategoryID);
            
            CreateTable(
                "dbo.ForumPosts",
                c => new
                    {
                        ForumPostID = c.Int(nullable: false, identity: true),
                        Detail = c.String(),
                        ForumSubCategoryID = c.Int(nullable: false),
                        CreatedByUserID = c.Int(nullable: false),
                        UpdatedByUserID = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ForumPostID)
                .ForeignKey("dbo.ForumSubCategories", t => t.ForumSubCategoryID, cascadeDelete: true)
                .Index(t => t.ForumSubCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ForumPosts", "ForumSubCategoryID", "dbo.ForumSubCategories");
            DropForeignKey("dbo.ForumSubCategories", "ForumCategoryID", "dbo.ForumCategories");
            DropIndex("dbo.ForumPosts", new[] { "ForumSubCategoryID" });
            DropIndex("dbo.ForumSubCategories", new[] { "ForumCategoryID" });
            DropTable("dbo.ForumPosts");
            DropTable("dbo.ForumSubCategories");
            DropTable("dbo.ForumCategories");
        }
    }
}
