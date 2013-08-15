namespace DBMigrator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WallMessageMessageColumnType : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER TABLE dbo.WallMessage
                  ALTER COLUMN [message] nvarchar(max)");
        }
        
        public override void Down()
        {

            Sql(@"ALTER TABLE dbo.WallMessage
                  ALTER COLUMN [message]  varchar(max)");
        }
    }
}
