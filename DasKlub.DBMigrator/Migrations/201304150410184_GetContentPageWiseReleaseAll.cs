using System.Data.Entity.Migrations;

namespace DasKlub.DBMigrator.Migrations
{
    public partial class GetContentPageWiseReleaseAll : DbMigration
    {
        public override void Up()
        {
            Sql(@"
    
          EXEC sp_executesql N'
            
create PROCEDURE [dbo].[up_GetContentPageWiseReleaseAll]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY releaseDate DESC
      )AS RowNumber
	,[contentID]
      ,[siteDomainID]
      ,[contentKey]
      ,[title]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[detail]
      ,[metaDescription]
      ,[metaKeywords]
      ,[contentTypeID]
      ,[releaseDate]
      ,[rating]
      ,[contentPhotoURL]
      ,[contentPhotoThumbURL]
      ,[contentVideoURL]
      ,[outboundURL]
      ,[isEnabled]
      ,[currentStatus]
    ,[language]
    ,[ContentVideoURL2]
     INTO #Results
     FROM [Content]
	 where releasedate < GETUTCDATE()  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END 
          ';
  

    ");

            Sql(@"
            create PROCEDURE up_GetContentPageWiseKeyRelease
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@key nvarchar(150)
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY releaseDate DESC
      )AS RowNumber
	,[contentID]
      ,[siteDomainID]
      ,[contentKey]
      ,[title]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[detail]
      ,[metaDescription]
      ,[metaKeywords]
      ,[contentTypeID]
      ,[releaseDate]
      ,[rating]
      ,[contentPhotoURL]
      ,[contentPhotoThumbURL]
      ,[contentVideoURL]
      ,[outboundURL]
      ,[isEnabled]
      ,[currentStatus]
        ,[language]
,[ContentVideoURL2]
     INTO #Results
     FROM [Content]
     WHERE metaKeywords like '%' + @key + '%'  and releasedate < getutcdate()

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END");
        }

        public override void Down()
        {
            Sql(@" drop proc up_GetContentPageWiseReleaseAll");

            Sql(@" drop proc up_GetContentPageWiseKeyRelease");
        }
    }
}
