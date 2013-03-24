namespace CodeFirstMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondVideoType : DbMigration
    {
        

        public override void Up()
        {
            Sql(@"

EXEC sp_executesql N'
ALTER TABLE Content
  ADD contentVideoURL2 varchar(150)';


-----------------------------------

EXEC sp_executesql N'
  ALTER proc [dbo].[up_SetContent]
 
           @title varchar(150) 
           ,@updatedByUserID int 
           ,@createdByUserID int 
           ,@detail varchar (max)
           ,@metaDescription varchar(500) 
           ,@metaKeywords varchar(500) 
           ,@contentTypeID int 
           ,@releaseDate datetime 
           ,@contentID  int
           ,@contentKey varchar(150)
           ,@rating float
			,@contentPhotoURL varchar(150)
			,@contentPhotoThumbURL varchar(150)
			,@outboundURL varchar(max)
			,@siteDomainID int
			,@isEnabled bit
			,@currentStatus char(1)
			,@contentVideoURL varchar(150)
			,@language char(2)
			,@contentVideoURL2 varchar(150)
 
AS

IF NOT EXISTS (SELECT * FROM CONTENT WHERE CONTENTID = @CONTENTID)
BEGIN

INSERT INTO [Content]
           ([title]
           ,[createDate]
           ,[createdByUserID]
           ,[detail]
           ,[metaDescription]
           ,[metaKeywords]
           ,[contentTypeID]
           ,[releaseDate]
           ,contentKey
           ,rating
           ,contentPhotoURL
           ,contentPhotoThumbURL
           ,outboundURL
           ,siteDomainID
           ,isEnabled
           ,currentStatus
           ,contentVideoURL
           ,[language]
		   ,contentVideoURL2
           )
     VALUES
           (
           @title
           ,getutcdate()
           ,@createdByUserID
           ,@detail
           ,@metaDescription
           ,@metaKeywords
           ,@contentTypeID
           ,@releaseDate
           ,@contentKey
           ,@rating
           ,@contentPhotoURL
           ,@contentPhotoThumbURL
           ,@outboundURL
           ,@siteDomainID
           ,@isEnabled
           ,@currentStatus
           ,@contentVideoURL
           ,@language
		   ,@contentVideoURL2
           )
           SELECT SCOPE_IDENTITY()
END
ELSE
BEGIN


UPDATE [Content]
   SET [title] = @title
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = getutcdate()
      ,[detail] = @detail
      ,[metaDescription] = @metaDescription
      ,[metaKeywords] = @metaKeywordS
      ,[contentTypeID] = @contentTypeID
      ,[releaseDate] = @releaseDate
      ,contentKey = @contentKey
	,rating = @rating
	,contentPhotoURL = @contentPhotoURL
	,contentPhotoThumbURL = @contentPhotoThumbURL
	,outboundURL = @outboundURL
	,siteDomainID = @siteDomainID
	,currentStatus = @currentStatus
	,contentVideoURL = @contentVideoURL
	,[language] =  @language
	,contentVideoURL2 = @contentVideoURL2
 WHERE contentID = @contentID

END';

------------------------------------------------------------


EXEC sp_executesql N'
ALTER proc [dbo].[up_GetContentByID]

@contentID int

as

SELECT [contentID]
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
      ,[Language]
	,contentVideoURL2  
  FROM  [Content]
 WHERE  contentID= @contentID';




------------------------------------------------------------

EXEC sp_executesql N'
ALTER proc [dbo].[up_GetAllContent]
 as
 
 select
 [contentID]
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
      ,[Language]
	,contentVideoURL2  
  FROM  [Content]
';




------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentAllPageWiseKey]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@key varchar(150)
   
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
	,contentVideoURL2  
     INTO #Results
     FROM [Content]
     WHERE metaKeywords like ''%'' + @key + ''%''  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
    
      DROP TABLE #Results
END';

------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWiseKey]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@key varchar(150)
            ,@language char(2)
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
	,contentVideoURL2  
     INTO #Results
     FROM [Content]
     WHERE metaKeywords like ''%'' + @key + ''%''  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     and [language] = @language
      DROP TABLE #Results
END';


------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWiseAll]
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
 ,[Language]
	,contentVideoURL2  
     INTO #Results
     FROM [Content]
	 ORDER BY releaseDate DESC
	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
   
     
      DROP TABLE #Results
END';
 



------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
		,@language char(2)
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
	,contentVideoURL2  
     INTO #Results
     FROM [Content]
	where [language] = @language
	
	SELECT @RecordCount = COUNT(*)
    FROM #Results  WHERE [language] = @language
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1 and [language] = @language
   
     
      DROP TABLE #Results
END';


 



------------------------------------------------------------


EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetContentForUser]
 
 @createdByUserID int
 
 AS
 
SELECT  [contentID]
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
	,contentVideoURL2  
  FROM [Content]
  WHERE createdByUserID = @createdByUserID
';



------------------------------------------------------------





EXEC sp_executesql N'
ALTER  proc [dbo].[up_GetContentByKey]
 
            @contentKey varchar(150)
            as
            
SELECT [contentID]
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
      ,[Language]
	,contentVideoURL2  
  FROM [Content]
where contentKey  = @contentKey
';

------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetPreviousNewsLang]
@createDateCurrent datetime
,@language char(2)
as

 
 
SELECT TOP 1 [contentID]
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
	,contentVideoURL2  
  FROM  [Content]
  where [language] = @language AND  [releaseDate] < @createDateCurrent
  order by [releaseDate] desc';



------------------------------------------------------------

EXEC sp_executesql N'

 ALTER proc [dbo].[up_GetPreviousNews]
@createDateCurrent datetime
 
as

 
 
SELECT TOP 1 [contentID]
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
	,contentVideoURL2  
  FROM  [Content]
  where   [releaseDate] < @createDateCurrent
  order by [releaseDate] desc';






------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetNextNews]
@createDateCurrent datetime
 
as

 
 
SELECT TOP 1 [contentID]
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
	,contentVideoURL2  
  FROM  [Content]
  where   [releaseDate] > @createDateCurrent
  order by [releaseDate]  asc';


 

------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetNextNewsLang]
@createDateCurrent datetime

as

 
 
SELECT TOP 1 [contentID]
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
	,contentVideoURL2  
  FROM  [Content]
  where  [releaseDate] > @createDateCurrent
  order by [releaseDate]  asc';



");
        }
        
        public override void Down()
        {
            Sql(@"
EXEC sp_executesql N'
ALTER TABLE Content
  DROP COLUMN contentVideoURL2' ;


------------------------------------------------------------



EXEC sp_executesql N'
 ALTER proc [dbo].[up_SetContent]
 
           @title varchar(150) 
           ,@updatedByUserID int 
           ,@createdByUserID int 
           ,@detail varchar (max)
           ,@metaDescription varchar(500) 
           ,@metaKeywords varchar(500) 
           ,@contentTypeID int 
           ,@releaseDate datetime 
           ,@contentID  int
           ,@contentKey varchar(150)
           ,@rating float
			,@contentPhotoURL varchar(150)
			,@contentPhotoThumbURL varchar(150)
			,@outboundURL varchar(max)
			,@siteDomainID int
			,@isEnabled bit
			,@currentStatus char(1)
			,@contentVideoURL varchar(150)
			,@language char(2)
 
AS

IF NOT EXISTS (SELECT * FROM CONTENT WHERE CONTENTID = @CONTENTID)
BEGIN

INSERT INTO [Content]
           ([title]
           ,[createDate]
           ,[createdByUserID]
           ,[detail]
           ,[metaDescription]
           ,[metaKeywords]
           ,[contentTypeID]
           ,[releaseDate]
           ,contentKey
           ,rating
           ,contentPhotoURL
           ,contentPhotoThumbURL
           ,outboundURL
           ,siteDomainID
           ,isEnabled
           ,currentStatus
           ,contentVideoURL
           ,[language]
           )
     VALUES
           (
           @title
           ,getutcdate()
           ,@createdByUserID
           ,@detail
           ,@metaDescription
           ,@metaKeywords
           ,@contentTypeID
           ,@releaseDate
           ,@contentKey
           ,@rating
           ,@contentPhotoURL
           ,@contentPhotoThumbURL
           ,@outboundURL
           ,@siteDomainID
           ,@isEnabled
           ,@currentStatus
           ,@contentVideoURL
           ,@language
           )
           SELECT SCOPE_IDENTITY()
END
ELSE
BEGIN


UPDATE [Content]
   SET [title] = @title
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = getutcdate()
      ,[detail] = @detail
      ,[metaDescription] = @metaDescription
      ,[metaKeywords] = @metaKeywordS
      ,[contentTypeID] = @contentTypeID
      ,[releaseDate] = @releaseDate
      ,contentKey = @contentKey
	,rating = @rating
	,contentPhotoURL = @contentPhotoURL
	,contentPhotoThumbURL = @contentPhotoThumbURL
	,outboundURL = @outboundURL
	,siteDomainID = @siteDomainID
	,currentStatus = @currentStatus
	,contentVideoURL = @contentVideoURL
	,[language] =  @language
 WHERE contentID = @contentID

END
';

------------------------------------------------------------


EXEC sp_executesql N'
ALTER proc [dbo].[up_GetContentByID]

@contentID int

as

SELECT [contentID]
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
      ,[Language]
  FROM  [Content]
 WHERE  contentID= @contentID';




------------------------------------------------------------

EXEC sp_executesql N'
ALTER proc [dbo].[up_GetAllContent]
 as
 
 select
 [contentID]
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
      ,[Language]
  FROM  [Content]
';


------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentAllPageWiseKey]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@key varchar(150)
   
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
     INTO #Results
     FROM [Content]
     WHERE metaKeywords like ''%'' + @key + ''%''  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
    
      DROP TABLE #Results
END';



 


------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWiseKey]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@key varchar(150)
            ,@language char(2)
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
     INTO #Results
     FROM [Content]
     WHERE metaKeywords like ''%'' + @key + ''%''  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     and [language] = @language
      DROP TABLE #Results
END';


------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWiseAll]
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
 ,[Language]
     INTO #Results
     FROM [Content]
	 ORDER BY releaseDate DESC
	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
   
     
      DROP TABLE #Results
END';
 



------------------------------------------------------------

EXEC sp_executesql N'
ALTER PROCEDURE [dbo].[up_GetContentPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
		,@language char(2)
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
     INTO #Results
     FROM [Content]
	where [language] = @language
	
	SELECT @RecordCount = COUNT(*)
    FROM #Results  WHERE [language] = @language
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1 and [language] = @language
   
     
      DROP TABLE #Results
END';


 



------------------------------------------------------------


EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetContentForUser]
 
 @createdByUserID int
 
 AS
 
SELECT  [contentID]
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
  FROM [Content]
  WHERE createdByUserID = @createdByUserID
';



------------------------------------------------------------





EXEC sp_executesql N'
ALTER  proc [dbo].[up_GetContentByKey]
 
            @contentKey varchar(150)
            as
            
SELECT [contentID]
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
      ,[Language]
  FROM [Content]
where contentKey  = @contentKey
';

------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetPreviousNewsLang]
@createDateCurrent datetime
,@language char(2)
as

 
 
SELECT TOP 1 [contentID]
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
  FROM  [Content]
  where [language] = @language AND  [releaseDate] < @createDateCurrent
  order by [releaseDate] desc';



------------------------------------------------------------

EXEC sp_executesql N'

 ALTER proc [dbo].[up_GetPreviousNews]
@createDateCurrent datetime
 
as

 
 
SELECT TOP 1 [contentID]
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
  FROM  [Content]
  where   [releaseDate] < @createDateCurrent
  order by [releaseDate] desc';






------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetNextNews]
@createDateCurrent datetime
 
as

 
 
SELECT TOP 1 [contentID]
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
  FROM  [Content]
  where   [releaseDate] > @createDateCurrent
  order by [releaseDate]  asc';


 

------------------------------------------------------------

EXEC sp_executesql N'
 ALTER proc [dbo].[up_GetNextNewsLang]
@createDateCurrent datetime

as

 
 
SELECT TOP 1 [contentID]
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
  FROM  [Content]
  where  [releaseDate] > @createDateCurrent
  order by [releaseDate]  asc';



");
        }
    }
}
