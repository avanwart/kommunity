using System.Data.Entity.Migrations;

namespace DBMigrator.Migrations
{
    public partial class ContestResults : DbMigration
    {
        public override void Up()
        {
            Sql(@"



 
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'up_GetContestResults')

EXEC sp_executesql N'

  create proc [dbo].[up_GetContestResults]
  
  @contestID int

  as


CREATE TABLE #votes 
( 
     vote INT, 
     youtubeusername varchar(255),
    videoURL VARCHAR(255),
	publishdate datetime
)

insert into #votes 
 

SELECT  
 count( distinct ua.ipAddress ) as ''votes''
,vid.providerUserKey as ''username''
 , vid.providerKey as ''url''
 , vid.publishDate as ''publishdate''   
     
  FROM  [ContestVideoVote] cvv  INNER JOIN 
  [ContestVideo] conv on cvv.contestVideoID = conv.contestVideoID
  INNER JOIN Video vid ON conv.videoID = vid.videoID INNER JOIN UserAccount ua 
  on cvv.userAccountID = ua.userAccountID  INNER JOIN Contest cont on cont.contestID = conv.contestID
  where conv.contestID = @contestID and ua.createDate < cont.deadline
  group by vid.providerKey, vid.providerUserKey, ua.ipAddress,  vid.publishDate 
    
    
    SELECT COUNT(vote) as ''cnts'',youtubeusername, videoURL, publishDate  from #votes
     group by youtubeusername, videoURL, publishDate
     order by ''cnts'' desc, publishdate asc


drop table #votes

 
';


");
        }

        public override void Down()
        {
            Sql(@"
EXEC sp_executesql N'

    drop proc up_GetContestResults
';
");
        }
    }
}