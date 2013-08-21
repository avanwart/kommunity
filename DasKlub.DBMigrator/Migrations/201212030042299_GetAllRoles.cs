 
using System.Data.Entity.Migrations;

namespace DBMigrator.Migrations
{
    public partial class GetAllRoles : DbMigration
    {
        public override void Up()
        {
            Sql(@"
 create proc up_GetAllRoles

 as

SELECT  [roleID]
      ,[roleName]
      ,[createDate]
      ,[updatedDate]
      ,[createdByEndUserID]
      ,[updatedByEndUserID]
      ,[description]
  FROM  [Role]
");
        }

        public override void Down()
        {
            Sql(@"   drop proc up_GetAllRoles ");
        }
    }
}