using System.Data.Entity.Migrations;

namespace DBMigrator.Migrations
{
    public partial class Log4NetTable : DbMigration
    {
        public override void Up()
        { 
            Sql(@"

EXEC sp_executesql N'
  
    CREATE TABLE [dbo].[Log4Net] (
    [Id] [int] IDENTITY (1, 1) NOT NULL,
    [Date] [datetime] NOT NULL,
    [Thread] [varchar] (255) NOT NULL,
    [Level] [varchar] (50) NOT NULL,
    [Logger] [varchar] (255) NOT NULL,
    [Message] [varchar] (4000) NOT NULL,
    [Exception] [varchar] (2000) NULL,
    [Location] [varchar] (255) NULL
 );

 
';
EXEC sp_executesql N'

 create proc up_AddLog4Net

  @log_date DateTime
  ,@thread varchar(255)
   ,@log_level varchar(50)
   ,@logger varchar(255)
   ,@message varchar(4000)
   ,@exception varchar(2000)
  ,@location varchar(255)
 
as
  
INSERT INTO Log4Net ([Date],[Thread],[Level],[Logger],[Message],[Exception], [Location]) VALUES (@log_date, @thread, @log_level, @logger, @message, @exception,@location)

';





");
        }
        
        public override void Down()
        {
            Sql(@"

      EXEC sp_executesql N'

        DROP TABLE [dbo].[Log4Net]

';


EXEC sp_executesql N'

DROP PROC up_AddLog4Net

';

 
");
        }
    }
}
