using System.Data.Entity.Migrations;

namespace DasKlub.DBMigrator.Migrations
{
    public partial class BirthDates : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    
          EXEC sp_executesql N'
            
create proc up_GetBirhtdays

@daysForward int

as

   SELECT useraccountid, [birthDate] AS Birthdate
 ,FLOOR(DATEDIFF(dd,EMP.[birthDate],GETUTCDATE()) / 365.25) AS AGEnow
 ,FLOOR(DATEDIFF(dd,EMP.[birthDate],GETUTCDATE()+@daysForward) / 365.25) AS AGEafter
FROM 
[UserAccountDetail] emp
WHERE 1 = (FLOOR(DATEDIFF(dd,EMP.[birthDate],GETUTCDATE()+@daysForward) / 365.25))
          -
          (FLOOR(DATEDIFF(dd,EMP.[birthDate],GETUTCDATE()) / 365.25))
            		ORDER BY Month(Birthdate), Day(Birthdate)

          ';

    ");
        }

        public override void Down()
        {
            Sql(@"
    
          EXEC sp_executesql N'
            
            drop proc up_GetBirhtdays

            
          ';

    ");
        }
    }
}