namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForumPropertyUniqueKeys : DbMigration
    {
        public override void Up()
        {

            Sql(@"  EXEC sp_executesql N' ALTER TABLE ForumCategories ADD CONSTRAINT uc_ForumKey UNIQUE ([Key])';
             EXEC sp_executesql N' ALTER TABLE ForumSubCategories ADD CONSTRAINT uc_ForumSubKey UNIQUE ([Key], ForumCategoryID)';");
 
        }
        
        public override void Down()
        {
            Sql(@"EXEC sp_executesql N' ALTER TABLE ForumCategories DROP CONSTRAINT uc_ForumKey';
          EXEC sp_executesql N'    ALTER TABLE ForumSubCategories DROP CONSTRAINT uc_ForumSubKey';");
        }

    }
}
