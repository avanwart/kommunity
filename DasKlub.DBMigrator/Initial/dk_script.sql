USE [master]
GO
/****** Object:  Database DasKlubDB    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE DATABASE DasKlubDB
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DasKlubDB_data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DasKlubDB.mdf' , SIZE = 303104KB , MAXSIZE = 1024000KB , FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'DasKlubDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\DasKlubDB_log.ldf' , SIZE = 504KB , MAXSIZE = UNLIMITED, FILEGROWTH = 10%)
GO
ALTER DATABASE DasKlubDB SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC DasKlubDB.[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE DasKlubDB SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE DasKlubDB SET ANSI_NULLS OFF 
GO
ALTER DATABASE DasKlubDB SET ANSI_PADDING OFF 
GO
ALTER DATABASE DasKlubDB SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE DasKlubDB SET ARITHABORT OFF 
GO
ALTER DATABASE DasKlubDB SET AUTO_CLOSE OFF 
GO
ALTER DATABASE DasKlubDB SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE DasKlubDB SET AUTO_SHRINK OFF 
GO
ALTER DATABASE DasKlubDB SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE DasKlubDB SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE DasKlubDB SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE DasKlubDB SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE DasKlubDB SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE DasKlubDB SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE DasKlubDB SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE DasKlubDB SET  DISABLE_BROKER 
GO
ALTER DATABASE DasKlubDB SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE DasKlubDB SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE DasKlubDB SET TRUSTWORTHY OFF 
GO
ALTER DATABASE DasKlubDB SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE DasKlubDB SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE DasKlubDB SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE DasKlubDB SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE DasKlubDB SET RECOVERY SIMPLE 
GO
ALTER DATABASE DasKlubDB SET  MULTI_USER 
GO
ALTER DATABASE DasKlubDB SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE DasKlubDB SET DB_CHAINING OFF 
GO
ALTER DATABASE DasKlubDB SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE DasKlubDB SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'DasKlubDB', N'ON'
GO
USE DasKlubDB
 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [dbo].[up_AddAcknowledgement]

             @userAccountID int
            ,@statusUpdateID int
            ,@createdByUserID int
            ,@acknowledgementType char(1)
as
-- add record
INSERT INTO [Acknowledgement]
           ([userAccountID]
           ,[statusUpdateID]
           ,[createDate]
           ,[createdByUserID]
           ,acknowledgementType)
     VALUES
           (@userAccountID  
           ,@statusUpdateID  
           ,GETUTCDATE()
           ,@createdByUserID
           ,@acknowledgementType  )

SELECT SCOPE_IDENTITY()
;

drop proc up__Delete  


GO
/****** Object:  StoredProcedure [dbo].[up_AddArtist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddArtist]

 
 @createdByUserID int 
,@isHidden bit 
,@name nvarchar(50) 
,@altName nvarchar(50) 

AS

INSERT INTO [Artist]
           ([createDate]
           ,[createdByUserID]
           ,[isHidden]
           ,[name]
           ,altName)
     VALUES
           (
           GETUTCDATE()
           ,@createdByUserID
           ,@isHidden
           ,@name
           ,@altName
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddArtistEvent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddArtistEvent]

 @artistID int
,@eventID int
,@rankOrder tinyint


AS

INSERT INTO [Artistevent]
           ([artistID]
           ,[eventID]
           ,[rankOrder])
     VALUES
           (@artistID
           ,@eventID
           ,@rankOrder)

GO
/****** Object:  StoredProcedure [dbo].[up_AddArtistProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddArtistProperty]

@createdByUserID int
,@artistID int
,@propertyContent varchar(max)
,@propertyType char(2)


           as
           
INSERT INTO  [ArtistProperty]
           (
           [createDate]
           ,[createdByUserID]
           ,[artistID]
           ,[propertyContent]
           ,[propertyType])
     VALUES
           (
           GETUTCDATE()
           ,@createdByUserID
           ,@artistID
           ,@propertyContent
           ,@propertyType)
           
SELECT SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[up_AddBlockedUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[up_AddBlockedUser]
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 @userAccountIDBlocking int
,@userAccountIDBlocked int
,@createdByUserID int
           
as           

INSERT INTO  [BlockedUser]
           ( [userAccountIDBlocking]
           ,[userAccountIDBlocked]
           ,[createDate]
           ,[createdByUserID])
     VALUES
           ( @userAccountIDBlocking
           ,@userAccountIDBlocked
           ,GETUTCDATE()
           ,@createdByUserID)
           
  SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddBrand]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddBrand]

@brandKey nvarchar(75)
,@name nvarchar(50)
,@description nvarchar(900)
,@createdByUserID int
,@brandImageMain varchar(255)
,@brandImageThumb varchar(255)
,@isEnabled bit
,@siteDomainID int
,@metaDescription nvarchar(155)
,@userAccountID int

AS

INSERT INTO [Brand]
           ([brandKey]
           ,[name]
           ,[description]
           ,[createDate]
           ,[createdByUserID]
           ,[brandImageMain]
           ,[brandImageThumb]
           ,[isEnabled]
           ,siteDomainID
           ,metaDescription
           ,userAccountID)
     VALUES
           (@brandKey
           ,@name
           ,@description
           ,GETUTCDATE()
           ,@createdByUserID
           ,@brandImageMain
           ,@brandImageThumb
           ,@isEnabled
           ,@siteDomainID
           ,@metaDescription
           ,@userAccountID)

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddCategory]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddCategory]

 @departmentID int
,@name nvarchar(50)
,@description nvarchar(255)
,@createdByUserID int
,@categoryKey nvarchar(75)

AS

INSERT INTO [Category]
           ([departmentID]
           ,[name]
           ,[description]
           ,[createDate]
           ,[createdByUserID]
           ,categoryKey)
     VALUES
           (@departmentID
           ,@name
           ,@description
           ,GETUTCDATE()
           ,@createdByUserID
           ,@categoryKey)

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddChatRoom]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddChatRoom]

@userName varchar(25)
,@chatMessage nvarchar(max)
,@createdByUserID int
,@ipAddress varchar(50)
,@roomID	int

AS

INSERT INTO [ChatRoom]
           ([userName]
           ,[chatMessage]
           ,[createDate]
           ,[createdByUserID]
           ,ipAddress
           ,roomID
           )
     VALUES
           (@userName
           ,@chatMessage
           ,GETUTCDATE()
           ,@createdByUserID
           ,@ipAddress
           ,@roomID
           )
           
SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddChatRoomUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_AddChatRoomUser]


 
           
		   @createdByUserID  int 
        ,@ipAddress	varchar(50) 
           ,@roomID int
		   ,@connectionCode varchar(50)


 as

INSERT INTO [dbo].[ChatRoomUser]
           (  [createDate]
         
           ,[createdByUserID]
           ,[ipAddress]
           ,[roomID]
		   ,connectionCode)
     VALUES
           ( 
          GETUTCDATE()
      
           ,@createdByUserID 
        ,@ipAddress 
           ,@roomID
		   ,@connectionCode )

SELECT SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[up_AddClickLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddClickLog]

@ipAddress varchar(25)
,@currentURL varchar(255)
,@referringURL varchar(255)
,@createdByUserID int
,@productID int
,@clickType char(1)
as

INSERT INTO  [ClickLog]
           ([ipAddress]
           ,[currentURL]
           ,[referringURL]
           ,[createdByUserID]
           ,[createDate]
           ,productID
           ,clickType
           )
     VALUES
           (@ipAddress
           ,@currentURL
           ,@referringURL
           ,@createdByUserID
           ,GETUTCDATE()
           ,@productID
           ,@clickType
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddColor]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddColor]

@name varchar(25)
,@createdByUserID INT
,@siteDomainID int = 0

AS

INSERT INTO [Color]
           ([name]
           ,[createDate]
           ,[createdByUserID]
           ,[siteDomainID])
     VALUES
           (@name
           ,GETUTCDATE()
           ,@createdByUserID
           ,@siteDomainID
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddContentComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddContentComment]

           @createdByUserID int
           ,@statusType char(1)
           ,@detail nvarchar(max)
           ,@contentID int
           ,@fromName varchar(50)
           ,@fromEmail  varchar(50)
           ,@ipAddress  varchar(50)
as

INSERT INTO [ContentComment]
           (
           createDate
		   ,createdByUserID  
           ,statusType 
           ,detail  
           ,contentID  
           ,fromName  
           ,fromEmail  
           ,ipAddress  
           )
     VALUES
           (GETUTCDATE()
			,@createdByUserID  
           ,@statusType 
           ,@detail  
           ,@contentID  
           ,@fromName  
           ,@fromEmail  
           ,@ipAddress 
           )

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddContestVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddContestVideo]

@videoID int
,@contestID int
,@createdByUserID int
,@subContest char(1)
       
AS

INSERT INTO [ContestVideo]
           ([videoID]
           ,[contestID]
           ,[createDate]
           ,[createdByUserID]
           ,subContest
           )
     VALUES
           (@videoID
           ,@contestID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@subContest
           )
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddContestVideoVote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddContestVideoVote]

 
@userAccountID int
,@contestVideoID int
,@createdByUserID int

as


INSERT INTO  [ContestVideoVote]
           (
            [userAccountID]
           ,[contestVideoID]
           ,[createDate]
           ,[createdByUserID])
     VALUES
           (
            @userAccountID
           ,@contestVideoID
           ,GETUTCDATE()
           ,@createdByUserID)
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddCreatePlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddCreatePlaylist]


@createdByUserID int 
,@playlistBegin datetime
,@playListName varchar(50)
,@userAccountID int 
,@autoPlay bit
           
           
AS
           
INSERT INTO [Playlist]
           ([createDate]
           ,[createdByUserID]
           ,[playlistBegin]
           ,[playListName]
           ,[userAccountID]
           ,[autoPlay])
     VALUES
     (
           GETUTCDATE()
           ,@createdByUserID
           ,@playlistBegin
           ,@playListName
           ,@userAccountID 
           ,@autoPlay )
SELECT SCOPE_IDENTITy()



GO
/****** Object:  StoredProcedure [dbo].[up_AddDirectMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddDirectMessage]

@createdByUserID int
           ,@fromUserAccountID int
           ,@toUserAccountID int
           ,@isRead bit
           ,@message nvarchar(max)
           ,@isEnabled bit
AS

INSERT INTO [DirectMessage]
           ([createDate]
           ,[createdByUserID]
           ,[fromUserAccountID]
           ,[toUserAccountID]
           ,[isRead]
           ,[message]
           ,[isEnabled])
     VALUES
           (GETUTCDATE()
           ,@createdByUserID
           ,@fromUserAccountID
           ,@toUserAccountID
           ,@isRead
           ,@message
           ,@isEnabled
           )

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddErrorLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddErrorLog]


@createdByUserID int
,@message varchar(max)
,@url varchar(255)
,@responseCode int

           
           as

INSERT INTO  [ErrorLog]
           (
            [createdByUserID]
           ,[message]
           ,[createDate]
           ,[url]
		   ,responseCode)
     VALUES
           (
           @createdByUserID
           ,@message
           ,GETUTCDATE()
           ,@url
		   ,@responseCode)
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddEvent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddEvent]


	@name nvarchar(50)
	,@venueID int 
	,@createdByUserID int 
	,@localTimeBegin datetime 
	,@notes nvarchar(max) 
	,@ticketURL nvarchar(max) 
	,@localTimeEnd datetime 
	,@eventCycleID int 
	,@rsvpURL nvarchar(max) 
	,@isReoccuring bit 
	,@isEnabled bit 
	,@eventDetailURL nvarchar(max)

           AS

INSERT INTO [Event]
           (
           	 name
	, venueID
	, createdByUserID
	, localTimeBegin
	, notes
	, ticketURL
	, localTimeEnd
	, eventCycleID
	, rsvpURL
	, isReoccuring
	, isEnabled
	, eventDetailURL
	,createDate
           )
     VALUES
           (
                      	@name 
	,@venueID 
	,@createdByUserID 
	,@localTimeBegin
	,@notes 
	,@ticketURL
	,@localTimeEnd
	,@eventCycleID
	,@rsvpURL
	,@isReoccuring
	,@isEnabled
	,@eventDetailURL
	,GETUTCDATE()
           )
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddGeoData]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddGeoData]
 
@countrycode varchar(3)
,@postalcode varchar(10)
,@placename varchar(180)
,@state varchar(100)
,@county varchar(100)
,@community varchar(100)
,@latitude varchar(25)
,@longitude varchar(25)
,@accuracy varchar(5)
           
           as

INSERT INTO [GeoData]
           ([countrycode]
           ,[postalcode]
           ,[placename]
           ,[state]
           ,[county]
           ,[community]
           ,[latitude]
           ,[longitude]
           ,[accuracy])
     VALUES
           (@countrycode
           ,@postalcode
           ,@placename
           ,@state
           ,@county
           ,@community
           ,@latitude
           ,@longitude
           ,@accuracy
           )

GO
/****** Object:  StoredProcedure [dbo].[up_AddGoogleTaxonomy]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddGoogleTaxonomy]

@parentID int
,@name varchar(150)

as

INSERT INTO  [GoogleTaxonomy]
           ([parentID]
           ,[name])
     VALUES
           (
            @parentID
           ,@name
           )

SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddHostedVideoLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddHostedVideoLog]

@secondsElapsed int, @ipAddress varchar(25),@viewURL	varchar(255)	, @videoType	char(2)	 

AS

INSERT INTO [HostedVideoLog]
           ([createDate]
           ,viewURL
           ,[ipAddress]
		   ,secondsElapsed
		   ,videoType)
     VALUES
           (
            GETUTCDATE()
           ,@viewURL
           ,@ipAddress
		   ,@secondsElapsed
		   ,@videoType
           )
           
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddMultiProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddMultiProperty]

@propertyTypeID int
,@name varchar(50)
,@createdByUserID int
,@propertyContent varchar(MAX)

AS

INSERT INTO [MultiProperty]
           ([propertyTypeID]
           ,[name]
           ,[createDate]
           ,[createdByUserID]
           ,[propertyContent])
     VALUES
           (
            @propertyTypeID
           ,@name
           ,GETUTCDATE()
           ,@createdByUserID
           ,@propertyContent
           )

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddMultiPropertyVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddMultiPropertyVideo]

@multiPropertyID int
,@videoID int

AS

IF NOT EXISTS 
(
	SELECT * 
	FROM [MultiPropertyVideo] 
	WHERE multiPropertyID = @multiPropertyID AND videoID = @videoID
)
BEGIN

INSERT INTO [MultiPropertyVideo]
           (
           [multiPropertyID]
           ,videoID
           )
     VALUES
           (
           @multiPropertyID
           ,@videoID
           )
           
           SELECT 1
END
ELSE 
BEGIN 
	SELECT 0
END	
GO
/****** Object:  StoredProcedure [dbo].[up_AddOrder]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddOrder]


           @createdByUserID int
           ,@status char(1)
           ,@totalCost smallmoney
           ,@transactionResponse varchar(max)
           ,@countryTo char(2)
           ,@currency char(3)
           ,@contactEmail varchar(50)
           ,@IpAddress varchar(25)
           ,@sessionGUID	uniqueidentifier
           
           AS

INSERT INTO [Order]
           ([createDate]
           ,[createdByUserID]
           ,[status]
           ,[totalCost]
           ,[transactionResponse]
           ,[countryTo]
           ,[currency]
           ,[contactEmail]
           ,IpAddress
           ,sessionGUID)
     VALUES
           (GETUTCDATE()
           ,@createdByUserID  
           ,@status  
           ,@totalCost  
           ,@transactionResponse  
           ,@countryTo 
           ,@currency 
           ,@contactEmail 
           ,@IpAddress
           ,@sessionGUID
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddPhotoItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddPhotoItem]

@createdByUserID int
,@title varchar(100)
,@filePathRaw varchar(255)
,@filePathThumb varchar(255)
,@filePathStandard varchar(255)
           
           AS

INSERT INTO  [PhotoItem]
           ( [createDate]
         
           ,[createdByUserID]
           ,[title]
           ,[filePathRaw]
           ,[filePathThumb]
           ,[filePathStandard])
     VALUES
           (GETUTCDATE()
           ,@createdByUserID
           ,@title
           ,@filePathRaw
           ,@filePathThumb
           ,@filePathStandard)
SELECT SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[up_AddPlaylistVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddPlaylistVideo]

@playlistID int
,@videoID int
,@createdByUserID int
,@rankOrder smallint
           
AS           


INSERT INTO  [PlaylistVideo]
           ([playlistID]
           ,[videoID]
           ,[createDate]
           ,[createdByUserID]
           ,[rankOrder])
     VALUES
           (@playlistID
           ,@videoID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@rankOrder  
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddProfileLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddProfileLog]


			@lookingUserAccountID int
           ,@lookedAtUserAccountID int
           ,@createdByUserID int
           
           AS

INSERT INTO [ProfileLog]
           ([lookingUserAccountID]
           ,[lookedAtUserAccountID]
           ,[createDate]
           ,[createdByUserID])
     VALUES
           (@lookingUserAccountID
           ,@lookedAtUserAccountID
           ,GETUTCDATE()
           ,@createdByUserID
           )
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddReview]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_AddReview]
 
@createdByUserID int
,@productID int
,@authorName varchar(50) 
,@rating float 
,@authorURL varchar(500) 
,@location varchar(50) 
,@eMail varchar(50) 
,@description varchar(max)
,@publishDate datetime
,@isApproved bit
           
           as

INSERT INTO  [Review]
           ([createDate]
           ,[createdByUserID]
           ,[productID]
           ,[authorName]
           ,[rating]
           ,[authorURL]
           ,[location]
           ,[eMail]
           ,[description]
           ,publishDate
           ,isApproved)
     VALUES
           (GETUTCDATE()
           ,@createdByUserID
           ,@productID 
           ,@authorName
           ,@rating 
           ,@authorURL 
           ,@location
           ,@eMail
           ,@description
           ,@publishDate
           ,@isApproved
           )
 
SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddRSSItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddRSSItem]

           
            @rssResourceID int
           ,@createdByUserID int
           ,@authorName nvarchar(max)
           ,@commentsURL nvarchar(max)
           ,@description nvarchar(max)
           ,@pubDate datetime
           ,@title nvarchar(max)
           ,@languageName varchar(5)
		   ,@guidLink nvarchar(max)
			,@link nvarchar(max)

as


INSERT INTO [RSSItem]
           ([rssResourceID]
           ,[createDate]
           ,[createdByUserID]
           ,[authorName]
           ,[commentsURL]
           ,[description]
           ,[pubDate]
           ,[title]
           ,[languageName]
           ,guidLink
           ,link
           )
     VALUES
           (@rssResourceID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@authorName
           ,@commentsURL
           ,@description
           ,@pubDate
           ,@title
           ,@languageName
           ,@guidLink
           ,@link
           )


select scope_identity()


GO
/****** Object:  StoredProcedure [dbo].[up_AddRssResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddRssResource]

@createdByUserID int
,@rssResourceURL varchar(400)
,@resourceName varchar(150)
,@providerKey char(2)
,@isEnabled bit
,@artistID int

           AS

INSERT INTO [RssResource]
           (
            [createDate]
           ,[createdByUserID]
           ,[rssResourceURL]
           ,[resourceName]
           ,[providerKey]
           ,[isEnabled]
           ,[artistID])
     VALUES
           (
           GETUTCDATE()
           ,@createdByUserID
           ,@rssResourceURL
           ,@resourceName
           ,@providerKey
           ,@isEnabled
           ,@artistID
           )
           
  SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddSiteComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddSiteComment]

@detail nvarchar(max)
,@createdByUserID int

           
           AS


INSERT INTO [SiteComment]
           ([detail]
           ,[createDate]
           ,[createdByUserID])
     VALUES
           (@detail
           ,GETUTCDATE()
           ,@createdByUserID)
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddSiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddSiteDomain]

		@propertyType char(5)
      ,@createdByUserID int
      ,@language char(2)
      ,@description nvarchar(max)


	  as

	  INSERT INTO [dbo].[SiteDomain]
           ([propertyType]
           ,[createDate]
           ,[createdByUserID]
           ,[language]
           ,[description])
     VALUES
           (@propertyType
           ,GETUTCDATE()
           ,@createdByUserID
           ,@language
           ,@description)
SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddSong]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddSong]

@artistID int 
,@createdByUserID int 
,@isHidden bit 
,@name nvarchar(150)
,@songKey  nvarchar(150)
           
           AS

INSERT INTO  [Song]
           ([artistID]
           ,[createDate]
           ,[createdByUserID]
           ,[isHidden]
           ,[name]
           ,songKey)
     VALUES
           (@artistID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@isHidden
           ,@name
           ,@songKey)
SELECT SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[up_AddSongProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddSongProperty]


            @createdByUserID int
           ,@songID int
           ,@propertyContent varchar(max)
           ,@propertyType char(2)
           


AS


INSERT INTO  [SongProperty]
           ( [createDate]
           
           ,[createdByUserID]
           ,[songID]
           ,[propertyContent]
           ,[propertyType])
     VALUES
           (getutcdate()
           
           ,@createdByUserID
           ,@songID
           ,@propertyContent
           ,@propertyType
           )

select scope_identity()
GO
/****** Object:  StoredProcedure [dbo].[up_AddStatusComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddStatusComment]

@statusUpdateID int
,@userAccountID int
,@statusType char(1)
,@createdByUserID int
,@message nvarchar(max)

as

INSERT INTO  [StatusComment]
           ([statusUpdateID]
           ,[userAccountID]
           ,[statusType]
           ,[createdByUserID]
           ,[createDate]
           ,[message])
     VALUES
           (
			@statusUpdateID  
			,@userAccountID  
			,@statusType  
			,@createdByUserID  
			,GETUTCDATE()
			,@message  
           )
           
SELECT SCOPE_IDENTITY()


GO
/****** Object:  StoredProcedure [dbo].[up_AddStatusCommentAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/ 

 CREATE proc [dbo].[up_AddStatusCommentAcknowledgement]


@userAccountID int
,@statusCommentID int
,@createdByUserID int
,@acknowledgementType char(1)

 as

INSERT INTO [dbo].[StatusCommentAcknowledgement]
           ([userAccountID]
           ,[statusCommentID]
           ,[createDate]
           ,[createdByUserID]
           ,[acknowledgementType])
     VALUES
           (@userAccountID
           ,@statusCommentID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@acknowledgementType)

		   SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddStatusUpdate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
           
           CREATE proc [dbo].[up_AddStatusUpdate]
           
           @createdByUserID int
           ,@userAccountID int
           ,@message nvarchar(666)
           ,@statusType char(1)
           ,@photoItemID int --= null
           ,@zoneID int --= null
           ,@isMobile bit
           
           AS

INSERT INTO [StatusUpdate]
           ([createDate]
           ,[createdByUserID]
           ,[userAccountID]
           ,[message]
           ,statusType
           ,photoItemID
           ,zoneID
           ,isMobile
           )
     VALUES
           (GETUTCDATE()
           ,@createdByUserID
           ,@userAccountID
           ,@message
           ,@statusType
           ,@photoItemID
           ,@zoneID
           ,@isMobile
           )

SELECT SCOPE_IDENTITY()




 
GO
/****** Object:  StoredProcedure [dbo].[up_AddStatusUpdateNotification]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddStatusUpdateNotification]

@statusUpdateID int
,@createdByUserID int 
,@isRead bit
,@userAccountID int
,@responseType char(1)
as

INSERT INTO  StatusUpdateNotification
           ([statusUpdateID]
           ,[createDate]
           ,[createdByUserID]
           ,[isRead]
           ,[userAccountID]
           ,responseType)
     VALUES
           (
           @statusUpdateID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@isRead
           ,@userAccountID
           ,@responseType
           )


SELECT SCOPE_IDENTITY ()



GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 

/*
	FROM: RMW
	DATE: 2009-04-15
	DESC: add a new end user
	UPDT: 2012-06-05
*/

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_AddUserAccount]

	 @username nvarchar(30)
	,@email nvarchar(75)
	,@password varchar(50)
	,@passwordFormat varchar(50)
	,@passwordSalt varchar(50)
	,@passwordQuestion varchar(50)
	,@passwordAnswer  varchar(50)
	,@isOnLine bit
	,@isApproved bit
	,@comment varchar(255)
	,@isLockedOut  bit
	,@lastPasswordChangedDate datetime
	,@ipAddress varchar(25)
AS

  INSERT INTO UserAccount
			   (
			    username
			   ,eMail
			   ,[password]
			   ,passwordFormat
			   ,passwordQuestion
			   ,passwordAnswer
			   ,createDate
			   ,isOnLine
			   ,isApproved
			   ,comment
			   ,lastPasswordChangeDate
			   ,isLockedOut
			   ,lastActivityDate
			   ,ipAddress
			   )
		 VALUES
			   (
				 @username
				,@email
				,@password
				,@passwordFormat
				,@passwordQuestion
				,@passwordAnswer
				,GetUtcDate()
				,@isOnLine
				,@isApproved
				,@comment
				,@LastPasswordChangedDate
				,@isLockedOut
				,GetUtcDate()
				,@ipAddress
			   )
			   -- return the newly inserted primary key
			   SELECT CAST(SCOPE_IDENTITY() AS INT)
GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAccountDetail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddUserAccountDetail]

 @userAccountID int 
,@createdByUserID int 
,@country char(2) 
,@region varchar(25) 
,@city varchar(25) 
,@postalCode varchar(15) 
 
 ,@interestedInID	int	 
,@youAreID	int	 
,@relationshipStatusID	int
,@profilePicURL varchar(75) 
,@aboutDesc nvarchar(max)
 
,@birthDate date 
,@religion char(1) 
,@ethnicity char(1) 
,@heightCM float 
,@weightKG float 
,@diet char(1) 
,@accountViews int 
,@externalURL varchar(100) 
,@smokes char(1) 
,@drinks char(1) 
,@handed char(1)
,@displayAge bit
,@profileThumbPicURL	varchar(75)	
,@bandsSeen nvarchar(max)
,@bandsToSee nvarchar(max)
,@enableProfileLogging bit
,@lastPhotoUpdate datetime
,@emailMessages	bit
,@showOnMap bit
,@referringUserID int
,@browerType	varchar(15)
,@membersOnlyProfile	bit
,@messangerName varchar(25)
,@messangerType char(2)
,@hardwareSoftware nvarchar(max)
,@firstName	nvarchar(20)	 
,@lastName	nvarchar(20)	
,@defaultLanguage char(2)
,@latitude decimal(9,6)
,@longitude decimal(9,6)

AS

INSERT INTO [UserAccountDetail]
           (
                interestedInID
,youAreID
,relationshipStatusID	

           ,[userAccountID]
           ,[createDate]
           ,[createdByUserID]
           ,[country]
           ,[region]
           ,[city]
           ,[postalCode]
       

          
           ,[profilePicURL]
           ,[aboutDesc]
          
           ,[birthDate]
           ,[religion]
           ,[ethnicity]
           ,[heightCM]
           ,[weightKG]
           ,[diet]
           ,[accountViews]
           ,[externalURL]
           ,[smokes]
           ,[drinks]
           ,[handed]
           ,displayAge
           ,profileThumbPicURL
           ,bandsSeen
,bandsToSee
,enableProfileLogging
,lastPhotoUpdate
,emailMessages
,showOnMap
,referringUserID
,browerType
,membersOnlyProfile
,messangerName
,messangerType
,hardwareSoftware
,firstName	  
,lastName 
,defaultLanguage
,latitude
,longitude
           )
     VALUES
 (
      @interestedInID
,@youAreID
,@relationshipStatusID


  ,@userAccountID
  ,GETUTCDATE()
,@createdByUserID
,@country
,@region
,@city
,@postalCode
 
,@profilePicURL
,@aboutDesc
 
,@birthDate
,@religion
,@ethnicity
,@heightCM
,@weightKG
,@diet
,@accountViews
,@externalURL
,@smokes
,@drinks
,@handed
,@displayAge
,@profileThumbPicURL
,@bandsSeen
,@bandsToSee
,@enableProfileLogging
,@lastPhotoUpdate
,@emailMessages
,@showOnMap
,@referringUserID
,@browerType
,@membersOnlyProfile
,@messangerName
,@messangerType
,@hardwareSoftware
,@firstName	  
,@lastName 
,@defaultLanguage
,@latitude 
,@longitude
 )

SELECT SCOPE_IDENTITY()
GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAccountRole]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create proc [dbo].[up_AddUserAccountRole]
 

 @userAccountID int
,@roleID  int

		   as

INSERT INTO [dbo].[UserAccountRole]
           ([userAccountID]
           ,[roleID])
     VALUES
           (@userAccountID
           ,@roleID
		   )
GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAccountUserGroup]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddUserAccountUserGroup]

 @userAccountID int
,@userGroupId smallint

AS

INSERT INTO  [UserAccountUserGroup]
           ([userAccountID]
           ,[userGroupId])
     VALUES
           (
            @userAccountID
           ,@userGroupId
           )



GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAccountVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddUserAccountVideo]

@videoID int
,@userAccountID int
,@videoType char(1)           
           AS

IF EXISTS 
(SELECT * 
FROM [UserAccountVideo] 
WHERE videoID = @videoID
AND userAccountID = @userAccountID AND videoType = @videoType) 
BEGIN

	UPDATE [UserAccountVideo] 
	SET createDate = GETUTCDATE()
	WHERE videoID = @videoID
	AND userAccountID = @userAccountID AND videoType = @videoType

END
ELSE
BEGIN
INSERT INTO [UserAccountVideo]
           ([videoID]
           ,[userAccountID]
           ,[createDate]
           ,videoType)
     VALUES
           (@videoID
           ,@userAccountID
           ,GETUTCDATE()
           ,@videoType
           )
END
GO
/****** Object:  StoredProcedure [dbo].[up_AddUserAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_AddUserAddress]

@firstName nvarchar(15) 
,@middleName nvarchar(15) 
,@lastName nvarchar(15) 
,@addressLine1 nvarchar(75) 
,@addressLine2 nvarchar(50) 
,@addressLine3 nvarchar(50) 
,@city nvarchar(50) 
,@region nvarchar(50) 
,@postalCode nvarchar(15) 
,@countryISO char(2) 
,@createdByUserID int 
,@userAccountID int 
,@addressStatus char(1)
,@choice1 varchar(max)
,@choice2 varchar(max)

AS

INSERT INTO [UserAddress]
           ([firstName]
           ,[middleName]
           ,[lastName]
           ,[addressLine1]
           ,[addressLine2]
           ,[addressLine3]
           ,[city]
           ,[region]
           ,[postalCode]
           ,[countryISO]
           ,[createDate]
           ,[createdByUserID]
           ,[userAccountID]
			,addressStatus
			,choice1
			,choice2
           )
     VALUES
           (@firstName
           ,@middleName
           ,@lastName
           ,@addressLine1
           ,@addressLine2
           ,@addressLine3
           ,@city
           ,@region
           ,@postalCode
           ,@countryISO
           ,GETUTCDATE()
           ,@createdByUserID
           ,@userAccountID
			,@addressStatus
			,@choice1
			,@choice2
            )

SELECT SCOPE_IDENTITY()           




GO
/****** Object:  StoredProcedure [dbo].[up_AddUserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddUserConnection]

@fromUserAccountID int
,@toUserAccountID int
,@createdByUserID int
,@statusType char(1)
,@isConfirmed	bit
           
AS           

INSERT INTO [UserConnection]
           ([fromUserAccountID]
           ,[toUserAccountID]
           ,[createDate]
           ,[createdByUserID]
           ,[statusType]
           ,isConfirmed
           )
     VALUES
           (@fromUserAccountID
           ,@toUserAccountID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@statusType
           ,@isConfirmed
           )
           
SELECT SCOPE_IDENTITY()           




GO
/****** Object:  StoredProcedure [dbo].[up_AddUserPhoto]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddUserPhoto]

@userAccountID int
,@createdByUserID int
,@picURL varchar(75)
,@thumbPicURL varchar(75)
,@description varchar(255)
,@rankOrder tinyint


           AS

INSERT INTO [UserPhoto]
           ([userAccountID]
           ,[createDate]
           ,[createdByUserID]
           ,[picURL]
           ,[thumbPicURL]
           ,[description]
           ,[rankOrder])
     VALUES
           (
           @userAccountID
           ,GETUTCDATE()
           ,@createdByUserID
           ,@picURL
           ,@thumbPicURL
           ,@description
           ,@rankOrder
           )

SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddVenue]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddVenue]
           
 @venueName	nvarchar(50)
,@addressLine1	nvarchar(50)
,@addressLine2	nvarchar(25)
,@city	nvarchar(25)
,@region	nvarchar(20)
,@postalCode	nvarchar(15)
,@countryISO	char(2)
,@createdByUserID	int
,@venueURL	nvarchar(MAX)
,@isEnabled	bit
,@latitude	decimal(10, 7)
,@longitude	decimal(10, 7)
,@phoneNumber	varchar(15)
,@venueType	char(1)
,@description nvarchar(max)

           AS


INSERT INTO [Venue]
           (
			 venueName
			,addressLine1
			,addressLine2
			,city
			,region
			,postalCode
			,countryISO
			,createdByUserID
			,venueURL
			,isEnabled
			,createDate
			,latitude
			,longitude
			,phoneNumber 
			,venueType
			,[description]
           )
     VALUES
           (
			 @venueName
			,@addressLine1
			,@addressLine2
			,@city
			,@region
			,@postalCode
			,@countryISO
			,@createdByUserID
			,@venueURL
			,@isEnabled
			,GETUTCDATE()
			,@latitude
			,@longitude
			,@phoneNumber 
			,@venueType
			,@description
           )
 select scope_identity()
GO
/****** Object:  StoredProcedure [dbo].[up_AddVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddVideo]

@videoKey varchar(150) 
,@providerKey varchar(50) 
,@providerUserKey varchar(50) 
,@providerCode char(2) 
,@createdByUserID int 
,@isHidden bit 
,@isEnabled bit 
,@statusID int 
,@duration float 
,@intro float 
,@lengthFromStart float 
,@volumeLevel	tinyint
,@enableTrim bit
,@publishDate datetime

           AS

INSERT INTO [Video]
           ([videoKey]
           ,[providerKey]
           ,[providerUserKey]
           ,[providerCode]
           ,[createDate]
           ,[createdByUserID]
           ,[isHidden]
           ,[isEnabled]
           ,[statusID]
           ,[duration]
           ,[intro]
           ,[lengthFromStart]
           ,volumeLevel
           ,enableTrim
           ,publishDate
           )
     VALUES
           (
           @videoKey
           ,@providerKey
           ,@providerUserKey
           ,@providerCode
           ,GETUTCDATE()
           ,@createdByUserID
           ,@isHidden
           ,@isEnabled
           ,@statusID
           ,@duration
           ,@intro
           ,@lengthFromStart
           ,@volumeLevel
           ,@enableTrim
           ,@publishDate
           )
SELECT SCOPE_IDENTITY()



GO
/****** Object:  StoredProcedure [dbo].[up_AddVideoLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddVideoLog]

@videoID int, @ipAddress varchar(25)

AS

INSERT INTO [VideoLog]
           ([createDate]
           ,[videoID]
           ,[ipAddress])
     VALUES
           (
            GETUTCDATE()
           ,@videoID
           ,@ipAddress
           )
           
           
           SELECT SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_AddVideoRequest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddVideoRequest]

 
@createdByUserID int
,@requestURL varchar(100)
,@statusType	char(1)
,@videoKey	varchar(20)

           AS


INSERT INTO  [VideoRequest]
           ( [createDate]
           
           ,[createdByUserID]
           ,[requestURL]
           ,statusType
		   ,videoKey
           )
     VALUES
           ( 
           getutcdate(),
          
           @createdByUserID
           ,@requestURL
           ,@statusType
		   ,@videoKey
)
select scope_identity()



GO
/****** Object:  StoredProcedure [dbo].[up_AddVideoSong]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_AddVideoSong]

@videoID int
,@songID int
,@rankOrder int

AS

INSERT INTO [VideoSong]
           (
           
           [videoID]
           ,[songID]
           ,rankOrder
           )
     VALUES
           (
           @videoID  
           ,@songID  
           ,@rankOrder
           )



GO
/****** Object:  StoredProcedure [dbo].[up_AddVote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_AddVote]

 @userAccountID int
,@videoID int
,@score int

 
AS

INSERT INTO [Vote]
           ([userAccountID]
           ,[createDate]
           ,[videoID]
           ,[score])
     VALUES
           (@userAccountID
           ,GETUTCDATE()
           ,@videoID
           ,@score)
           
           select scope_identity()
 

GO
/****** Object:  StoredProcedure [dbo].[up_AddWallMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

  CREATE proc [dbo].[up_AddWallMessage]

           @createdByUserID int
           ,@message varchar(max)
           ,@isRead bit
           ,@fromUserAccountID int
           ,@toUserAccountID int
   
   as

INSERT INTO  [WallMessage]
           ( 
           [createDate]
           ,[createdByUserID]
           ,[message]
           ,[isRead]
           ,[fromUserAccountID]
           ,[toUserAccountID])
     VALUES
           (GETUTCDATE()
           ,@createdByUserID 
           ,@message
           ,@isRead
           ,@fromUserAccountID
           ,@toUserAccountID
		   )
select SCOPE_IDENTITY()

GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_DeleteAcknowledgement]

 @acknowledgementID int 

 as

DELETE FROM  [Acknowledgement]
      WHERE  acknowledgementID = @acknowledgementID


GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAllAcknowledgements]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteAllAcknowledgements]

@userAccountID int

AS

DELETE FROM Acknowledgement
      WHERE  userAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAllCommentAcknowledgements]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteAllCommentAcknowledgements]

@userAccountID int 

AS

DELETE FROM  StatusCommentAcknowledgement
WHERE   userAccountID = @userAccountID  
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAllDirectMessages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteAllDirectMessages]

@userAccountID int

AS

DELETE FROM DirectMessage
      WHERE  fromUserAccountID = @userAccountID
      or toUserAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAllStatusUpdates]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteAllStatusUpdates]

@userAccountID int

AS

DELETE FROM StatusUpdate
      WHERE  userAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteAllUserContestVotes]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteAllUserContestVotes]

@userAccountID int

as

DELETE FROM [ContestVideoVote]
      WHERE userAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteArtistProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteArtistProperty]
 
 @artistPropertyID int

      as

DELETE FROM  [ArtistProperty]
   
 WHERE artistPropertyID = @artistPropertyID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteBlockedUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_DeleteBlockedUser]
 
 @userAccountIDBlocking int ,
 @userAccountIDBlocked int
 
 as 
 
 
 DELETE FROM BlockedUser
 WHERE 
 userAccountIDBlocking 
 = @userAccountIDBlocking
 AND
userAccountIDBlocked 
= 
@userAccountIDBlocked
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteChatRoomUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

 CREATE proc [dbo].[up_DeleteChatRoomUser]


 
           
		   @connectionCode varchar(50)
        


 as


 DELETE FROM [ChatRoomUser]
 WHERE   connectionCode = @connectionCode
  
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteContentComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_DeleteContentComment]

@contentCommentID int

AS
 
 DELETE
  FROM  [ContentComment]
  WHERE contentCommentID = @contentCommentID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteContestVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteContestVideo]

@videoID int

as

DELETE FROM [ContestVideo]
      WHERE videoID = @videoID

GO
/****** Object:  StoredProcedure [dbo].[up_DeleteEventByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteEventByID]

@eventID int

AS

DELETE FROM  [Event]
      WHERE eventID = @eventID
 
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteFromContentByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteFromContentByID]

@contentID int

as

DELETE FROM [Content]
      WHERE [contentID] = @contentID

GO
/****** Object:  StoredProcedure [dbo].[up_DeleteMultiPropertyByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
  CREATE PROC [dbo].[up_DeleteMultiPropertyByID]
  
  @multiPropertyID int
  
  AS
  
  DELETE FROM [MultiProperty] 
  WHERE [multiPropertyID] = @multiPropertyID
  
  
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteMultiPropertyVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteMultiPropertyVideo]


 @multiPropertyID int, @videoID int
 
 AS

DELETE FROM  [MultiPropertyVideo]
      WHERE  multiPropertyID = @multiPropertyID AND videoID = @videoID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteNotificationsForStatusUpdate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteNotificationsForStatusUpdate]

@statusUpdateID int
as
DELETE FROM  [StatusUpdateNotification]
      WHERE  statusUpdateID = @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_DeletePhotoItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeletePhotoItem]

@photoItemID int

as

DELETE FROM [PhotoItem] 
WHERE photoItemID = @photoItemID
GO
/****** Object:  StoredProcedure [dbo].[up_DeletePlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeletePlaylist]

@playlistID int

AS

DELETE FROM  [Playlist]
      WHERE playlistID = @playlistID
 



GO
/****** Object:  StoredProcedure [dbo].[up_DeletePlaylistVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  
  CREATE proc [dbo].[up_DeletePlaylistVideo]
  
   @playlistID int,
   @videoID int
  
  AS
  
  DELETE FROM [PlaylistVideo]
  WHERE [playlistID] = @playlistID AND [videoID] = @videoID
  
  
GO
/****** Object:  StoredProcedure [dbo].[up_DeletePlaylistVideoByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE proc [dbo].[up_DeletePlaylistVideoByID]
  
   @playlistVideoID int 
  
  AS
  
  DELETE FROM [PlaylistVideo]
  WHERE playlistVideoID =  @playlistVideoID
  
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteProfileLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/


CREATE proc [dbo].[up_DeleteProfileLog]

@userAccountID int

AS

DELETE FROM ProfileLog
      WHERE  lookingUserAccountID = @userAccountID
      OR lookedAtUserAccountID = @userAccountID
      
      



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteReview]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteReview]

@reviewID int

AS

DELETE FROM  [Review]
      WHERE  reviewID  = @reviewID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteSiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteSiteDomain]

	 
	 @siteDomainID int

as

 
DELETE FROM [dbo].[SiteDomain]
WHERE siteDomainID = @siteDomainID


GO
/****** Object:  StoredProcedure [dbo].[up_DeleteSongsForVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/


CREATE proc [dbo].[up_DeleteSongsForVideo]

@videoID int

AS

DELETE FROM [VideoSong]
WHERE videoID = @videoID   


GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusAcknowledgements]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_DeleteStatusAcknowledgements]
 
 @statusUpdateID int
 
 as
 
  delete FROM  [Acknowledgement]
  WHERE statusUpdateID = @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteStatusComment]

@statusCommentID int

AS

DELETE FROM  [StatusComment]
      WHERE  statusCommentID = @statusCommentID

GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusCommentAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_DeleteStatusCommentAcknowledgement]

 @statusCommentAcknowledgementID int 

 as

DELETE FROM  StatusCommentAcknowledgement
      WHERE  statusCommentAcknowledgementID = @statusCommentAcknowledgementID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusCommentAcknowledgements]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteStatusCommentAcknowledgements]
 
 @statusCommentID int
 
 as
 
  delete FROM  [StatusCommentAcknowledgement]
  WHERE statusCommentID = @statusCommentID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusComments]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteStatusComments]


@statusUpdateID int

as

DELETE FROM [StatusComment]
      WHERE  [statusUpdateID] = @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusCommentsForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc  [dbo].[up_DeleteStatusCommentsForUser]


@userAccountID int

as

DELETE FROM [StatusComment]
      WHERE  userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusUpdate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteStatusUpdate]

@statusUpdateID int

AS

DELETE FROM [StatusUpdate]
      WHERE statusUpdateID = @statusUpdateID




GO
/****** Object:  StoredProcedure [dbo].[up_DeleteStatusUpdateNotification]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_DeleteStatusUpdateNotification]
 
       @statusUpdateNotificationID int
 as
 
 delete from StatusUpdateNotification
  WHERE  statusUpdateNotificationID = @statusUpdateNotificationID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserAccount]

@userAccountID int

AS


DELETE FROM [UserAccount]
WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserAccountDetail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserAccountDetail]

@userAccountDetailID int

as

DELETE FROM [UserAccountDetail]
      WHERE userAccountDetailID = @userAccountDetailID




GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserAccountMessages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserAccountMessages]
@userAccountID int
as
-- delete user messages
  DELETE FROM [Messages]
  WHERE [UserId] = @userAccountID
  
  
  
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserAccountVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserAccountVideo]

@userAccountID int

AS

DELETE FROM UserAccountVideo
      WHERE  userAccountID = @userAccountID
   
      
      



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserAddress]

@userAccountID int

AS

DELETE FROM [UserAddress]
      WHERE  userAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserConnection]

@userAccountID int

as

DELETE FROM [UserConnection]
      WHERE toUserAccountID = @userAccountID
      OR
		fromUserAccountID = @userAccountID



GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserConnectionByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserConnectionByID]

@userConnectionID int

as

DELETE FROM  [UserConnection]
WHERE [userConnectionID] = @userConnectionID
 
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserPhotoByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/


CREATE proc [dbo].[up_DeleteUserPhotoByID]

@userPhotoID int

AS

delete 
  FROM [UserPhoto]
  WHERE userPhotoID = @userPhotoID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserRoles]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserRoles]

 @userAccountID int
 
 AS
      
  DELETE FROM  [UserAccountRole]
  where userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserVotes]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteUserVotes]

@userAccountID int

AS


DELETE FROM Vote
WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_DeleteUserWall]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  procedure [dbo].[up_DeleteUserWall]

 
 @userAccountID int 

 as
  DELETE FROM [WallMessage]
  WHERE [fromUserAccountID] = @userAccountID OR  toUserAccountID = @userAccountID

GO
/****** Object:  StoredProcedure [dbo].[up_DeleteVideoForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_DeleteVideoForUser]

  @videoID int
	 ,@userAccountID int



 as


DELETE FROM [UserAccountVideo]
      WHERE videoID = @videoID
	  AND userAccountID = @userAccountID


GO
/****** Object:  StoredProcedure [dbo].[up_DeleteWallMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_DeleteWallMessage]

@wallMessageID int

AS

delete from WallMessage
where wallMessageID = @wallMessageID

GO
/****** Object:  StoredProcedure [dbo].[up_GetAccountCloudByLetter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAccountCloudByLetter]   

@firstLetter varchar(5)

AS

SELECT  rank() 

 OVER (ORDER BY vid.providerUserKey, vid.providerUserKey)


  as 'keyword_id', count(vid.providerUserKey) as 'keyword_count', 
 vid.providerUserKey as 'keyword_value',
 '/' +  LOWER(vid.providerUserKey) as 'keyword_url'
  FROM Video vid 
   where vid.providerUserKey  like @firstLetter + '%' AND 
   vid.ishidden = 0 AND vid.isEnabled = 1
 --AND ART.releaseDate < GETUTCDATE()
   group by vid.providerUserKey
--having count(sng.artistid) > 20
  order by 'keyword_count' desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetAcknowledgement]

@userAccountID int,
@statusUpdateID int

AS

SELECT [acknowledgementID]
      ,[userAccountID]
      ,[statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[acknowledgementType]
  from [Acknowledgement]
WHERE statusUpdateID = @statusUpdateID AND userAccountID = @userAccountID  
GO
/****** Object:  StoredProcedure [dbo].[up_GetAcknowledgementCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAcknowledgementCount]

@statusUpdateID int, @acknowledgementType char(1)

AS

SELECT COUNT(*)
  FROM [Acknowledgement]
  WHERE statusUpdateID = @statusUpdateID and acknowledgementType = @acknowledgementType
GO
/****** Object:  StoredProcedure [dbo].[up_GetAcknowledgementsForStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAcknowledgementsForStatus]

 @statusUpdateID int
 as

SELECT [acknowledgementID]
      ,[userAccountID]
      ,[statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[acknowledgementType]
  FROM  [Acknowledgement]
	where [statusUpdateID]= @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_GetActiveUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetActiveUsers]

AS

SELECT TOP 100 [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount]
  where [isLockedOut] = 0
  ORDER BY [lastActivityDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetActiveUsersFilter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE proc [dbo].[up_GetActiveUsersFilter]

@gender char(1)

AS

SELECT TOP 100 
		ua.[userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,ua.[createDate]
      ,ua.[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount] ua INNER JOIN UserAccountDetail uad on ua.userAccountID = uad.userAccountID
  where [isLockedOut] = 0 AND uad.Gender = @gender
  ORDER BY [lastActivityDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllArtistEvents]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetAllArtistEvents]

AS

SELECT [artistID]
      ,[eventID]
      ,[rankOrder]
  FROM [ArtistEvent]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllArtists]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetAllArtists]

AS

SELECT  [artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
      ,altName
  FROM [Artist]
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllBrands]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllBrands]
 

AS


SELECT [brandID]
      ,[brandKey]
      ,[name]
      ,[description]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[brandImageMain]
      ,[brandImageThumb]
      ,[isEnabled]
      ,siteDomainID
      ,metaDescription
      ,userAccountID
  FROM [Brand]
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllColors]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetAllColors]
 
 AS
 
SELECT [colorID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[siteDomainID]
  FROM  [Color]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllContent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllContent]
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
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllContentTypes]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllContentTypes]
 
 AS
 
SELECT  [contentTypeID]
      ,[contentName]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[contentCode]
  FROM  [ContentType]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllContests]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllContests]

 

AS

SELECT [contestID]
      ,[name]
      ,[deadLine]
      ,[description]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[beginDate]
      ,contestKey
  FROM [Contest]
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllDepartments]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllDepartments]
 
 
 AS
 
SELECT  [departmentID]
      ,[departmentKey]
      ,[siteDomainID]
      ,[name]
      ,[description]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rankOrder]
  FROM  [Department]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllEventCycles]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllEventCycles]

AS

SELECT [eventCycleID]
      ,[cycleName]
      ,[eventCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
  FROM [EventCycle]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllEvents]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllEvents]
 
 AS
 
SELECT  [eventID]
      ,[name]
      ,[venueID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[localTimeBegin]
      ,[notes]
      ,[ticketURL]
      ,[localTimeEnd]
      ,[eventCycleID]
      ,[rsvpURL]
      ,[isReoccuring]
      ,[isEnabled]
      ,[eventDetailURL]
  FROM [Event]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllInterestedIn]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllInterestedIn]

as

SELECT [interestedInID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [InterestedIn]
 


GO
/****** Object:  StoredProcedure [dbo].[up_GetAllRelationshipStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllRelationshipStatus]


as

SELECT [relationshipStatusID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [RelationshipStatus]
 


GO
/****** Object:  StoredProcedure [dbo].[up_GetAllRssResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllRssResource]

AS

SELECT  [rssResourceID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rssResourceURL]
      ,[resourceName]
      ,isenabled 
  FROM [RssResource]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllSiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllSiteDomain]

 as


SELECT [siteDomainID]
      ,[propertyType]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[language]
      ,[description]
  FROM [SiteDomain]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllSizes]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllSizes]
 
 AS
 
SELECT [sizeID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[sizeName]
      ,rankOrder
  FROM [Size]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROC [dbo].[up_GetAllStatus]
 
 AS
 
 
SELECT [statusID]
      ,[statusDescription]
      ,[statusCode]
  FROM [Status]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllStatusCommentsForUpdate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetAllStatusCommentsForUpdate]
 
 @statusUpdateID int
 
 AS
 
SELECT [statusCommentID]
      ,[statusUpdateID]
      ,[userAccountID]
      ,[statusType]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[message]
  FROM  [StatusComment]
  WHERE statusUpdateID= @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllUserAccounts]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllUserAccounts]
 
 AS
 
SELECT [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount] WITH (NOLOCK)
  WHERE isApproved = 1
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllUserStatusUpdates]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetAllUserStatusUpdates]
 
 @userAccountID int
 As
 
SELECT [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
  FROM  [StatusUpdate]
  WHERE [userAccountID] = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllVenues]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllVenues]

AS

SELECT [venueID]
	,[venueName]
	,[addressLine1]
	,[addressLine2]
	,[city]
	,[region]
	,[postalCode]
	,[countryISO]
	,[updatedByUserID]
	,[createDate]
	,[updateDate]
	,[createdByUserID]
	,[venueURL]
	,[isEnabled]
	,latitude
	,longitude
	,phoneNumber
	,venueType
	,[description]
  FROM [Venue]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllVideos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetAllVideos]

AS

SELECT [videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,volumeLevel
      ,publishDate
  FROM [Video] WITH (NOLOCK)
  ORDER BY [createDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllVideosByUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROC [dbo].[up_GetAllVideosByUser]
 
 @providerUserKey varchar(50)
 
 AS
 
SELECT [videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,[enableTrim]
      ,publishDate
  FROM  [Video]
  where [providerUserKey] =@providerUserKey AND ishidden = 0 and [isEnabled] = 1
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllVotes]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllVotes]

AS

SELECT [voteID]
      ,[userAccountID]
      ,[createDate]
      ,[videoID]
      ,[score]
  FROM [Vote]
GO
/****** Object:  StoredProcedure [dbo].[up_GetAllYouAre]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetAllYouAre]

as
 
SELECT  [youAreID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [YouAre]
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistByAltname]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetArtistByAltname]

@altname nvarchar(50)

as

SELECT [artistID]
      ,[name]
      ,[altName]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
  FROM  [Artist]
  WHERE altName = @altname
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetArtistByID]

@artistID int

AS

SELECT [artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
      ,altName
  FROM [Artist]
  
  WHERE [artistID] = @artistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetArtistByName]

@name varchar(50)

AS

SELECT [artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
      ,altName
  FROM [Artist]
  WHERE name = @name
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistCloudByLetter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetArtistCloudByLetter]   

@firstLetter varchar(5)

AS

SELECT sng.artistid
as 'keyword_id'
, count(sng.artistid) as 'keyword_count', 
art.name as 'keyword_value',
 '/' + art.altName /*+ '/' + replace(sng.songname, ' ', '-')*/ as 'keyword_url'
  FROM  [Song] sng
  INNER JOIN [Artist]  art on sng.artistid = art.artistid
 where art.altName  like @firstLetter + '%' AND art.ishidden = 0
 --AND ART.releaseDate < GETUTCDATE()
  group by art.altName, sng.artistid , art.name
--having count(sng.artistid) > 20
  order by 'keyword_count' desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistCloudByNonLetter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetArtistCloudByNonLetter]   
 

AS

SELECT sng.artistid as 'keyword_id'
, count(sng.artistid) as 'keyword_count', 
art.name as 'keyword_value',
 '/' + LOWER(replace(art.name, ' ', '-')) /*+ '/' + replace(sng.songname, ' ', '-')*/ as 'keyword_url'
  FROM  [Song] sng
  INNER JOIN [Artist]  art on sng.artistid = art.artistid
 where 
 
 art.name  NOT like  + 'a%' AND
 art.name  NOT like  + 'b%' AND
 art.name  NOT like  + 'c%' AND
 art.name  NOT like  + 'd%' AND
 art.name  NOT like  + 'e%' AND
 art.name  NOT like  + 'f%' AND
 art.name  NOT like  + 'g%' AND
 art.name  NOT like  + 'h%' AND
 art.name  NOT like  + 'i%' AND
 art.name  NOT like  + 'j%' AND
 art.name  NOT like  + 'k%' AND
 art.name  NOT like  + 'l%' AND
 art.name  NOT like  + 'm%' AND
 art.name  NOT like  + 'n%' AND
 art.name  NOT like  + 'o%' AND
 art.name  NOT like  + 'p%' AND
 art.name  NOT like  + 'q%' AND
 art.name  NOT like  + 'r%' AND
 art.name  NOT like  + 's%' AND
 art.name  NOT like  + 't%' AND
 art.name  NOT like  + 'u%' AND
 art.name  NOT like  + 'v%' AND
 art.name  NOT like  + 'w%' AND
 art.name  NOT like  + 'x%' AND
 art.name  NOT like  + 'y%' AND
 art.name  NOT like  + 'z%'  
 
 AND art.ishidden = 0
 --AND ART.releaseDate < GETUTCDATE()
  group by art.name, sng.artistid 
--having count(sng.artistid) > 20
  order by 'keyword_count' desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistPropertyForTypeArtist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetArtistPropertyForTypeArtist]

@artistID int,
@propertyType char(2)

AS

SELECT [artistPropertyID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[artistID]
      ,[propertyContent]
      ,[propertyType]
  FROM [ArtistProperty]
  WHERE artistID  = @artistID 
  and propertyType  = @propertyType
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistRssResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetArtistRssResource]
 
 @artistID int
 
 AS
 
SELECT  [rssResourceID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rssResourceURL]
      ,[resourceName]
      ,[providerKey]
      ,[isEnabled]
      ,[artistID]
  FROM [RssResource]
  WHERE artistID = @artistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistsForEvent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetArtistsForEvent]

@eventID int

AS

SELECT [artistID]
      ,[eventID]
      ,[rankOrder]
  FROM Artistevent
  where eventID = @eventID
GO
/****** Object:  StoredProcedure [dbo].[up_GetArtistsPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetArtistsPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY [name] ASC
      )AS RowNumber
	 ,[artistID]
      ,[name]
      ,[altName]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
     INTO #Results
     FROM Artist

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetBlockedUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetBlockedUsers]

@userAccountIDBlocking int

as


SELECT [blockedUserID]
      ,[userAccountIDBlocking]
      ,[userAccountIDBlocked]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
  FROM [BlockedUser]
  WHERE userAccountIDBlocking = @userAccountIDBlocking
GO
/****** Object:  StoredProcedure [dbo].[up_GetChatRoomUserByConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetChatRoomUserByConnection]

 @connectionCode varchar(50)


 as


SELECT  [chatRoomUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[ipAddress]
      ,[roomID]
      ,[connectionCode]
  FROM  [ChatRoomUser]
  WHERE [connectionCode] = @connectionCode
GO
/****** Object:  StoredProcedure [dbo].[up_GetChatRoomUserByUserAccountID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

 CREATE proc [dbo].[up_GetChatRoomUserByUserAccountID]

 @createdByUserID int

 as

SELECT  [chatRoomUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[ipAddress]
      ,[roomID]
      ,[connectionCode]
     FROM  [ChatRoomUser]
	 WHERE createdByUserID = @createdByUserID
GO
/****** Object:  StoredProcedure [dbo].[up_GetChattingUserCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetChattingUserCount]

 as

SELECT count(*)
  FROM [ChatRoomUser]
GO
/****** Object:  StoredProcedure [dbo].[up_GetChattingUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetChattingUsers]


 as

SELECT  [chatRoomUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[ipAddress]
      ,[roomID]
      ,[connectionCode]
  FROM  [ChatRoomUser]
GO
/****** Object:  StoredProcedure [dbo].[up_GetCityForCountryPostalCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROCEDURE [dbo].[up_GetCityForCountryPostalCode]
 
 @postalcode varchar(10),
 @countrycode varchar(3)
 
 AS
 
SELECT  [placename]
  FROM  [GeoData]
  where postalcode = @postalcode AND  countrycode = @countrycode
GO
/****** Object:  StoredProcedure [dbo].[up_GetCityStateForCountryPostalCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetCityStateForCountryPostalCode]

@postalCode varchar(15),
@countryCode varchar(5)

AS

 
SELECT   [placename] + ', ' + [state] as 'cityState'
      
  FROM [GeoData]
  where postalcode = @postalCode AND countrycode = @countryCode
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetCityStateForCountryZip]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROCEDURE [dbo].[up_GetCityStateForCountryZip]
 
 @countrycode varchar(2),
 @postalcode varchar(15)
 
 
 AS
 
 
SELECT  TOP 1 [placename], [state]
  FROM  [GeoData]
  WHERE countrycode = @countrycode and postalcode = @postalcode
GO
/****** Object:  StoredProcedure [dbo].[up_GetColorByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetColorByID]
 
 @colorID int
 
 as
 
SELECT [colorID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[siteDomainID]
  FROM  [Color]
  WHERE colorID = @colorID
GO
/****** Object:  StoredProcedure [dbo].[up_GetColorByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetColorByName]


@name varchar(25)


as


SELECT [colorID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[siteDomainID]
  FROM [Color]
  WHERE name = @name
GO
/****** Object:  StoredProcedure [dbo].[up_GetCommentAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetCommentAcknowledgement]

@userAccountID int,
@statusCommentID int

AS

SELECT[statusCommentAcknowledgementID]
      ,[userAccountID]
      ,[statusCommentID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[acknowledgementType]
  from StatusCommentAcknowledgement
WHERE  statusCommentID = @statusCommentID AND userAccountID = @userAccountID  
GO
/****** Object:  StoredProcedure [dbo].[up_GetCommentAcknowledgementCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetCommentAcknowledgementCount]

@statusCommentID int, @acknowledgementType char(1)

AS

SELECT COUNT(*)
  FROM [StatusCommentAcknowledgement]
  WHERE statusCommentID = @statusCommentID and acknowledgementType = @acknowledgementType
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentAllPageWiseKey]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetContentAllPageWiseKey]
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
     WHERE metaKeywords like '%' + @key + '%'  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
    
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetContentByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContentByID]

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
 WHERE  contentID= @contentID



GO
/****** Object:  StoredProcedure [dbo].[up_GetContentByKey]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE  proc [dbo].[up_GetContentByKey]
 
            @contentKey nvarchar(150)
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
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContentComment]


@contentCommentID int 

AS

SELECT  [contentCommentID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[detail]
      ,[contentID]
      ,[fromName]
      ,[fromEmail]
      ,ipAddress
  FROM  [ContentComment]
  WHERE  contentCommentID = @contentCommentID
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentComments]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContentComments]

@contentID int
,@statusType char(1)

AS

SELECT [contentCommentID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[detail]
      ,[contentID]
      ,[fromName]
      ,[fromEmail]
      ,ipAddress
  fROM  [ContentComment]
  WHERE [contentID] = @contentID AND statusType = @statusType
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentCommentsPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetContentCommentsPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY createDate DESC
      )AS RowNumber
	,[contentCommentID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[detail]
      ,[contentID]
      ,[fromName]
      ,[fromEmail]
      ,[ipAddress]
     INTO #Results
     FROM [ContentComment]

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetContentForContentTypeID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetContentForContentTypeID]
 
 @contentTypeID int, 
 @siteDomainID int
 
 as
 
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
      ,[outboundURL]
      ,[isEnabled]
      ,[currentStatus]
  FROM  [Content]
  WHERE  contentTypeID = @contentTypeID
  and siteDomainID = @siteDomainID
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetContentForUser]
 
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
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetContentPageWise]
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
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetContentPageWiseAll]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetContentPageWiseAll]
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
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetContentPageWiseKey]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetContentPageWiseKey]
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
     WHERE metaKeywords like '%' + @key + '%'  

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     and [language] = @language
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetContentTags]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE proc [dbo].[up_GetContentTags]
  
  @language CHAR(2)
  
  as
   
SELECT [metaKeywords]
  FROM [Content]
  WHERE [language] = @language
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentTagsAll]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE proc [dbo].[up_GetContentTagsAll]
  
 
  
  as
   
SELECT [metaKeywords]
  FROM [Content]
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentTypeByContentCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetContentTypeByContentCode]
 
 @contentCode char(5)
 
 AS
 
SELECT  [contentTypeID]
      ,[contentName]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[contentCode]
  FROM  [ContentType]
  WHERE  contentCode = @contentCode
GO
/****** Object:  StoredProcedure [dbo].[up_GetContentTypeByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetContentTypeByID]
 
 @contentTypeID int
 
 AS
 
SELECT  [contentTypeID]
      ,[contentName]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[contentCode]
  FROM  [ContentType]
  WHERE  contentTypeID = @contentTypeID
GO
/****** Object:  StoredProcedure [dbo].[up_GetContestByKey]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContestByKey]

@contestKey varchar(100)

AS

SELECT [contestID]
      ,[name]
      ,[deadLine]
      ,[description]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[beginDate]
      ,contestKey
  FROM [Contest]
  WHERE contestKey = @contestKey
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetContestByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContestByName]

@name varchar(100)

AS

SELECT [contestID]
      ,[name]
      ,[deadLine]
      ,[description]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[beginDate]
      ,contestKey
  FROM [Contest]
  WHERE name = @name
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetContestVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetContestVideo]
 
 @videoID int
 
 as
 
SELECT [contestVideoID]
      ,[videoID]
      ,[contestID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,subContest
  FROM  [ContestVideo]
 WHERE videoID = @videoID 
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetContestVideoForContestAndVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContestVideoForContestAndVideo]

@videoID int,
@contestID int

as


SELECT [contestVideoID]
      ,vid.[videoID]
      ,[contestID]
      ,vid.[updatedByUserID]
      ,vid.[createDate]
      ,vid.[updateDate]
      ,vid.[createdByUserID]
      ,subContest
  FROM [ContestVideo] cv INNER JOIN Video vid on cv.videoID = vid.videoID 
  WHERE vid.[videoID] = @videoID and 
  [contestID] = @contestID and vid.isEnabled = 1 and isHidden = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetContestVideosForContest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetContestVideosForContest]


@contestID INT

AS

 
  
  SELECT [contestVideoID]
      ,vid.[videoID]
      ,[contestID]
      ,vid.[updatedByUserID]
      ,vid.[createDate]
      ,vid.[updateDate]
      ,vid.[createdByUserID]
      ,subContest
  FROM [ContestVideo] cv INNER JOIN Video vid on cv.videoID = vid.videoID 
  WHERE 
  [contestID] = @contestID and vid.isEnabled = 1 and isHidden = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetCountOfVideosInPlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetCountOfVideosInPlaylist]
 @playlistID int
 as
 
SELECT COUNT(plv.videoID)
  FROM  [PlaylistVideo] plv INNER JOIN Video vid on plv.videoid = vid.videoid
  where vid.isenabled = 1 and isHidden = 0 and playlistID = @playlistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetCountUnconfirmedConnections]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetCountUnconfirmedConnections]

@toUserAccountID int 

AS

SELECT COUNT(*)
  FROM [UserConnection] WITH (NOLock)
  WHERE toUserAccountID = @toUserAccountID AND isConfirmed = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetCurrentTime]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE proc [dbo].[up_GetCurrentTime]
  AS
  SELECT GETUTCDATE()
GO
/****** Object:  StoredProcedure [dbo].[up_GetDateVenues]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetDateVenues]

AS

SELECT dbo.fnFormatDate (localTimeBegin, 'YYYY-MM-DD')  + ' ' +  vu.[venueName] as 'datevenue', td.eventID
  FROM [event] td INNER JOIN Venue vu ON td.venueID = vu.venueID
  ORDER BY 'datevenue'
GO
/****** Object:  StoredProcedure [dbo].[up_GetDirectMessagesFromUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDirectMessagesFromUser]

@fromUserAccountID int

as

SELECT [directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
  FROM [DirectMessage]
  WHERE fromUserAccountID = @fromUserAccountID 
    ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetDirectMessagesToFrom]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDirectMessagesToFrom]
@fromUserAccountID int,
 @toUserAccountID int

as

SELECT TOP 50 [directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
  FROM DirectMessage
  WHERE fromUserAccountID = @fromUserAccountID AND toUserAccountID = @toUserAccountID
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetDirectMessagesToUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDirectMessagesToUser]

@toUserAccountID int

as

SELECT [directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
  FROM [DirectMessage]
  WHERE toUserAccountID = @toUserAccountID 
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetDirectMessagesToUserCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDirectMessagesToUserCount]

@toUserAccountID int

as

SELECT count(*)
  FROM [DirectMessage] WITH (NOLock)
  WHERE toUserAccountID = @toUserAccountID AND isRead = 0 AND isEnabled = 1
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctInterests]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE PROC [dbo].[up_GetDistinctInterests]

  as

  select distinct interestedInID FROM  UserAccountDetail
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctLocations]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDistinctLocations]

AS
 
SELECT DISTINCT countryISO, region, city
  FROM [Venue]
  ORDER BY countryISO, region, city
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctNewsLanguages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetDistinctNewsLanguages]

as

SELECT  DISTINCT [language]
  FROM [Content]
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctRelationshipStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE PROC [dbo].[up_GetDistinctRelationshipStatus]

  as

  select distinct relationshipStatusID FROM  UserAccountDetail
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctUserCountries]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetDistinctUserCountries]

 as

SELECT DISTINCT [country]
     
  FROM  [UserAccountDetail]
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctUserLanguages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

 CREATE proc [dbo].[up_GetDistinctUserLanguages]

 as

SELECT DISTINCT  defaultLanguage
     
  FROM  [UserAccountDetail]
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetDistinctUsers]

AS

SELECT DISTINCT providerUserKey
  FROM [Video] 
  WHERE providerUserKey IS NOT NULL and providerUserKey != ''
GO
/****** Object:  StoredProcedure [dbo].[up_GetDistinctYouAres]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE PROC [dbo].[up_GetDistinctYouAres]

  as

  select distinct youAreID FROM  UserAccountDetail
GO
/****** Object:  StoredProcedure [dbo].[up_GetEventByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetEventByID]

@eventID int

AS

SELECT [eventID]
      ,[name]
      ,[venueID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[localTimeBegin]
      ,[notes]
      ,[ticketURL]
      ,[localTimeEnd]
      ,[eventCycleID]
      ,[rsvpURL]
      ,[isReoccuring]
      ,[isEnabled]
      ,[eventDetailURL]
  FROM [Event]
  WHERE eventID = @eventID
GO
/****** Object:  StoredProcedure [dbo].[up_GetEventCycleByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetEventCycleByID]

@eventCycleID int

AS

SELECT  [eventCycleID]
      ,[cycleName]
      ,[eventCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
  FROM [EventCycle]
  WHERE eventCycleID = @eventCycleID
GO
/****** Object:  StoredProcedure [dbo].[up_GetEventsForLocation]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetEventsForLocation]

@beginDate datetime,
@endDate datetime,
@countryISO char(2),
@region nvarchar(20),
@city nvarchar(25)

AS

SELECT  [eventID]
      ,td.[venueID]
      ,td.[updatedByUserID]
      ,td.[createDate]
      ,td.[updateDate]
      ,td.[createdByUserID]
      ,[localTimeBegin]
      ,[notes]
      ,[ticketURL]
      ,[localTimeEnd]
      ,[eventCycleID]
      ,[rsvpURL]
      ,td.name
      ,td.isEnabled
      ,td.isReoccuring
	  ,td.[eventDetailURL]
  FROM [event] td INNER JOIN Venue vn ON td.venueID = vn.venueID
  WHERE [localTimeBegin] BETWEEN @beginDate 
  AND @endDate AND vn.countryISO = @countryISO AND vn.region = @region AND vn.city = @city 
  AND td.isEnabled = 1 AND td.isReoccuring = 0
  
  UNION 
  
SELECT  [eventID]
      ,td.[venueID]
      ,td.[updatedByUserID]
      ,td.[createDate]
      ,td.[updateDate]
      ,td.[createdByUserID]
      ,[localTimeBegin]
      ,[notes]
      ,[ticketURL]
      ,[localTimeEnd]
      ,[eventCycleID]
      ,[rsvpURL]
      ,td.name
      ,td.isEnabled
      ,td.isReoccuring
      ,td.[eventDetailURL]
  FROM [event] td INNER JOIN Venue vn ON td.venueID = vn.venueID
  WHERE vn.countryISO = @countryISO AND vn.region = @region AND vn.city = @city 
  AND td.isEnabled = 1 AND td.isReoccuring = 1
GO
/****** Object:  StoredProcedure [dbo].[up_GetInterestedIn]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetInterestedIn]

@interestedInID int

as

SELECT [interestedInID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [InterestedIn]
  WHERE [interestedInID] =  @interestedInID


GO
/****** Object:  StoredProcedure [dbo].[up_GetLatLongForCountryPostal]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetLatLongForCountryPostal]

@countryCode varchar(3),
@postalCode varchar(10)

AS

SELECT TOP 1 [latitude], [longitude]
  FROM [GeoData]
  WHERE countryCode = @countryCode AND replace(postalCode, ' ', '') = @postalCode
GO
/****** Object:  StoredProcedure [dbo].[up_GetMailPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
CREATE PROCEDURE [dbo].[up_GetMailPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@toUserAccountID int
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY createDate DESC
      )AS RowNumber
	,[directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
     INTO #Results
     FROM [DirectMessage]
WHERE  toUserAccountID = @toUserAccountID
	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetMailPageWiseSent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetMailPageWiseSent]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@fromUserAccountID int
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY createDate DESC
      )AS RowNumber
	,[directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
     INTO #Results
     FROM [DirectMessage]
WHERE   fromUserAccountID = @fromUserAccountID 
	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetMailPageWiseToUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetMailPageWiseToUser]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@toUserAccountID int
      ,@fromUserAccountID int
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY createDate DESC
      )AS RowNumber
	,[directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
     INTO #Results
     FROM [DirectMessage]
WHERE  (toUserAccountID = @toUserAccountID AND fromUserAccountID = @fromUserAccountID) OR (toUserAccountID = @fromUserAccountID  AND fromUserAccountID = @toUserAccountID)
	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetMappableUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMappableUsers]

as

SELECT TOP 200 ua.[userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,ua.[createDate]
      ,ua.[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM  [UserAccount] ua INNER JOIN UserAccountDetail uad on ua.[userAccountID] = uad.[userAccountID]
  WHERE DATEDIFF(year, uad.birthDate, GETUTCDATE()) >= 18 AND uad.showOnMap = 1
  ORDER BY lastActivityDate desc
  --
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostAcknowledgedStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetMostAcknowledgedStatus]

@daysBack int
,@acknowledgementType char(1)

AS

SELECT TOP 3 COUNT(ack.[statusUpdateID]) as 'cnt', ack.statusUpdateID,  su.createDate
     
  FROM [Acknowledgement] ack INNER JOIN StatusUpdate su on ack.statusupdateid = su.statusupdateid
  WHERE su.createDate BETWEEN DATEADD(day, -@daysBack, GETUTCDATE()) 
  AND GETUTCDATE() and acknowledgementType = @acknowledgementType
  GROUP BY ack.statusUpdateID ,  su.createDate
  ORDER BY 'cnt' DESC, su.createDate desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostCommentedOnStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetMostCommentedOnStatus]

 @beginDate datetime


 as

SELECT TOP 1 statcom.statusUpdateID, count(statcom.statusUpdateID) as 'total'
  FROM [StatusComment] statcom INNER JOIN StatusUpdate statup ON statcom.statusUpdateID = statup.statusUpdateID
  WHERE statup.createDate between @beginDate  and  GETUTCDATE()
  GROUP BY statcom.statusUpdateID ,  statup.createDate 
  order by   count(statusCommentID) desc, statup.createDate desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostDiscountedProducts]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/


CREATE proc [dbo].[up_GetMostDiscountedProducts]


AS

SELECT TOP 10  
      productid
--      ,( 1.00 - ([discountPrice] * 1.00)/[retailPrice]) as 'percent'
  FROM [SQL2008R2_805733_dasklub].[dbo].[Product]
  WHERE enableDiscount = 1
  order by  
  ( 1.00 - ([discountPrice] * 1.00)/[retailPrice])  
  desc
  
   
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostLookedAtUsersDays]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetMostLookedAtUsersDays]

@daysBack int

AS


SELECT ToP 5 COUNT([lookedAtUserAccountID]) 'count', [lookedAtUserAccountID]
  FROM  [ProfileLog]
  WHERE createDate BETWEEN DATEADD(day, -@daysBack, GETUTCDATE()) AND GETUTCDATE()
  GROUP BY [lookedAtUserAccountID] 
  ORDER BY 'COUNT' desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentFavoriteVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMostRecentFavoriteVideo]

as

SELECT TOP 1 [videoID]
  FROM [UserAccountVideo]
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentlyViewedProducts]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
CREATE PROC [dbo].[up_GetMostRecentlyViewedProducts] --'67.188.238.139'

@ipAddress varchar(25)

AS

    SELECT  top 5 cl.[productID]
    FROM [ClickLog] CL INNER JOIN Product prd on cl.productID = prd.productID
   WHERE cl.ipAddress = @ipAddress and cl.clickType = 'V'
     order by cl.createDate desc
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentRSS]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetMostRecentRSS]
 AS
SELECT TOP 3 [rssItemID]
      ,ri.[rssResourceID]
      ,ri.[updatedByUserID]
      ,ri.[createDate]
      ,ri.[updateDate]
      ,ri.[createdByUserID]
      ,[authorName]
      ,[commentsURL]
      ,[description]
      ,[pubDate]
      ,[title]
      ,[languageName]
      	   ,guidLink
			,link
  FROM  [RSSItem] ri INNER JOIN RssResource rr on ri.rssResourceID = rr.rssResourceID 
  where rr.isenabled = 1
  ORDER BY [pubDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentSentMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetMostRecentSentMessage]
 
 @fromUserAccountID int
 
 AS
 
SELECT [directMessageID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[isRead]
      ,[message]
      ,[isEnabled]
  FROM  [DirectMessage]
  WHERE [fromUserAccountID] = @fromUserAccountID
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentStatusUpdates]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMostRecentStatusUpdates]
AS
SELECT TOP 1 [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
      ,[photoItemID]
      ,[zoneID]
  FROM [StatusUpdate]
  ORDER BY createdate desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentUserStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMostRecentUserStatus]

@userAccountID int

AS

SELECT TOP 1 [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
      ,[photoItemID]
      ,[zoneID]
  FROM [StatusUpdate]
  WHERE userAccountID = @userAccountID
  ORDER BY CREATEDATE DESC
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostRecentVideos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetMostRecentVideos]
 
 AS
 
SELECT TOP 3
	   [videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,[enableTrim]
      ,publishDate
  FROM [Video]
  WHERE [isHidden] = 0 AND [isEnabled] = 1
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostViewedProducts]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMostViewedProducts]

@days int

as

SELECT TOP 15 COUNT(prod.name) as 'count', prod.productID
 FROM [ClickLog] cl 
 INNER JOIN Product prod on cl.productid = prod.productID
  where prod.isHidden = 0 and prod.isInStock = 1 and
   cl.createDate between DATEADD(DD, -@days, GETUTCDATE()) and GETUTCDATE()  AND CL.clickType = 'T' -- this actually click throughs
  group by prod.productID
  order by 'count' desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetMostWatchedVideos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMostWatchedVideos]

@daysBack int

as

SELECT Top 4 COUNT( vl.[videoID]) 'CNT', vl.[videoID]
  FROM  [VideoLog] vl INNER JOIN Video vid on vl.videoid = vid.videoid
  WHERE vl.createDate BETWEEN DATEADD(DAY, -@daysBack, GETUTCDATE()) AND GETUTCDATE() AND vl.videoID != 0
  AND vid.isEnabled = 1 and vid.isHidden = 0
  GROUP BY vl.[videoID]  
 ORDER BY 'CNT' DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetMultiPropertyByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMultiPropertyByID]

@multiPropertyID int

AS

SELECT  [multiPropertyID]
      ,[propertyTypeID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[propertyContent]
  FROM  [MultiProperty]
  WHERE multiPropertyID = @multiPropertyID
GO
/****** Object:  StoredProcedure [dbo].[up_GetMultiPropertyByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMultiPropertyByName]

@name varchar(50)

AS

SELECT [multiPropertyID]
      ,[propertyTypeID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[propertyContent]
  FROM [MultiProperty]
  WHERE name = @name
GO
/****** Object:  StoredProcedure [dbo].[up_GetMultiPropertyByPropertyTypeID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetMultiPropertyByPropertyTypeID]

@propertyTypeID int

AS

SELECT  [multiPropertyID]
      ,[propertyTypeID]
      ,[name]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[propertyContent]
  FROM  [MultiProperty]
  WHERE propertyTypeID = @propertyTypeID
GO
/****** Object:  StoredProcedure [dbo].[up_GetMultiPropertyVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 -- product id & product type id
 
 CREATE PROC [dbo].[up_GetMultiPropertyVideo] 
 
 @videoID int,
 @propertyTypeID int
 
 AS
 
SELECT mp.[multiPropertyID]
      ,mp.[propertyTypeID]
      ,mp.[name]
      ,mp.[createDate]
      ,mp.[updateDate]
      ,mp.[createdByUserID]
      ,mp.[updatedByUserID]
      ,mp.[propertyContent]
     
  FROM [MultiPropertyVideo] mpp
  INNER JOIN  MultiProperty mp on mpp.[multiPropertyID] = 
  mp.[multiPropertyID] 
  WHERE mpp.videoid = @videoID AND mp.propertyTypeID = @propertyTypeID
GO
/****** Object:  StoredProcedure [dbo].[up_GetNewestPhotoUploader]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetNewestPhotoUploader]

AS

SELECT TOP 1 [userAccountID]
  FROM [UserAccountDetail]
  ORDER BY lastPhotoUpdate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetNewestUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetNewestUsers]
 
 as
 
SELECT Top 3 [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM  [UserAccount]
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetNextNews]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetNextNews]
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
  order by [releaseDate]  asc


---

GO
/****** Object:  StoredProcedure [dbo].[up_GetNextNewsLang]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetNextNewsLang]
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
  where  [releaseDate] > @createDateCurrent and [releaseDate] < GETUTCDATE()
  order by [releaseDate]  asc

---

GO
/****** Object:  StoredProcedure [dbo].[up_GetNextPhoto]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetNextPhoto]
@createDateCurrent datetime
as

SELECT TOP 1 [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM  [PhotoItem]
  where createDate > @createDateCurrent
  order by createDate asc
GO
/****** Object:  StoredProcedure [dbo].[up_GetNextPhotoForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetNextPhotoForUser]
@createDateCurrent datetime
,@createdByUserID int
as

SELECT TOP 1 [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM  [PhotoItem]
  where createdByUserID = @createdByUserID and createDate > @createDateCurrent
  order by createDate asc
GO
/****** Object:  StoredProcedure [dbo].[up_GetOnlineUserCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetOnlineUserCount]

as

SELECT COUNT(*)
  FROM [UserAccount] WITH (nolock)
  WHERE isOnline = 1
GO
/****** Object:  StoredProcedure [dbo].[up_GetOnlineUsers]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetOnlineUsers]
 
 as
 
SELECT   [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount]
  WHERE isOnline = 1
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetPhotoItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetPhotoItem]
 
 @photoItemID int
 as
SELECT [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM [PhotoItem]
  WHERE photoItemID= @photoItemID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPhotoItemCountForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetPhotoItemCountForUser]
 
 @createdByUserID int
 
 as
 
SELECT COUNT(*)
FROM PhotoItem
  WHERE  createdByUserID = @createdByUserID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPhotoItemForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPhotoItemForUser]

@createdByUserID int

as


SELECT [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM [PhotoItem]
  WHERE  createdByUserID = @createdByUserID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPhotoItemsPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROCEDURE [dbo].[up_GetPhotoItemsPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY createDate DESC
      )AS RowNumber
     ,[photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
     INTO #Results
     FROM [PhotoItem]
      

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetPlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPlaylist]

@playlistID int

AS

SELECT [playlistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[playlistBegin]
      ,[playListName]
      ,userAccountID
      ,autoPlay
  FROM [Playlist]
  WHERE playlistID = @playlistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPlaylistVideoByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPlaylistVideoByID]

@playlistVideoID int

as

SELECT [playlistVideoID]
      ,[playlistID]
      ,[videoID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rankOrder]
  FROM [PlaylistVideo]
  WHERE playlistVideoID = @playlistVideoID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPlaylistVideoByIDs]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPlaylistVideoByIDs]

@videoID int,
 @playlistID int

AS

SELECT [playlistVideoID]
      ,[playlistID]
      ,[videoID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rankOrder]
  FROM  [PlaylistVideo]
  WHERE videoID = @videoID AND playlistID = @playlistID
 



GO
/****** Object:  StoredProcedure [dbo].[up_GetPlaylistVideosForPlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPlaylistVideosForPlaylist]

@playlistID int

AS

SELECT [playlistVideoID]
      ,[playlistID]
      ,[videoID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rankOrder]
  FROM [PlaylistVideo]
  WHERE playlistID = @playlistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetPreviousNews]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetPreviousNews]
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
  where   [releaseDate] < @createDateCurrent and [releaseDate] < GETUTCDATE()
  order by [releaseDate] desc


GO
/****** Object:  StoredProcedure [dbo].[up_GetPreviousNewsLang]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetPreviousNewsLang]
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
  order by [releaseDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetPreviousPhoto]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetPreviousPhoto]
@createDateCurrent datetime
as

SELECT TOP 1 [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM  [PhotoItem]
  where createDate < @createDateCurrent
  order by createDate desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetPreviousPhotoForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetPreviousPhotoForUser]
@createDateCurrent datetime
,@createdByUserID int
as

SELECT TOP 1 [photoItemID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[title]
      ,[filePathRaw]
      ,[filePathThumb]
      ,[filePathStandard]
  FROM  [PhotoItem]
  where createdByUserID = @createdByUserID and createDate < @createDateCurrent 
  order by createDate desc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetProductByCategoryID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetProductByCategoryID]

@categoryID int

AS


SELECT  prod.[productID]
      ,[productKey]
      ,[brandID]
      ,[name]
      ,[isInStock]
      ,[productCode]
      ,[couponCode]
      ,prod.[createDate]
      ,prod.[updateDate]
      ,prod.[createdByUserID]
      ,[metaKeywords]
      ,[metaDescription]
      ,prod.[updatedByUserID]
      ,[isHidden]
      ,[productGlobalID]
      ,prod.[shopID]
      ,prod.[condition]
      ,prod.[obtainOption]
  FROM [Product] prod INNER JOIN ProductCategory prodcat
  on prod.productID = prodcat.productID
  WHERE prodcat.categoryID = @categoryID AND prod.isInStock = 1 
  AND prod.isHidden = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetPropertyTypeByCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetPropertyTypeByCode]
 
 @propertyTypeCode  char(5)
 
 AS
 
 
SELECT [propertyTypeID]
      ,[propertyTypeCode]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[propertyTypeName]
  FROM  [PropertyType]
  WHERE  propertyTypeCode = @propertyTypeCode
GO
/****** Object:  StoredProcedure [dbo].[up_GetPropertyTypeByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROC [dbo].[up_GetPropertyTypeByID]
 
 @propertyTypeID int
 
 AS
 
SELECT [propertyTypeID]
      ,[propertyTypeCode]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[propertyTypeName]
  FROM [PropertyType]
  WHERE propertyTypeID = @propertyTypeID
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetRandomProductsInWishList]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROC [dbo].[up_GetRandomProductsInWishList]
 
 @createdByUserID int
 
 AS
  
    SELECT TOP 4 [productID] FROM  [WishList]
 WHERE [createdByUserID] = @createdByUserID
ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRandomUserAccountID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetRandomUserAccountID]

AS  
  
  SELECT TOP 1 ua.userAccountID 
  FROM  UserAccount ua INNER JOIN UserAccountDetail uad on ua.userAccountID = uad.userAccountID
  WHERE isLockedOut = 0 and uad.profilePicURL != ''
ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRandomVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetRandomVideo]
 
 AS

SELECT TOP 1  vid.[videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,vid.[updatedByUserID]
      ,vid.[createDate]
      ,vid.[updateDate]
      ,vid.[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,enableTrim
      ,publishdate
       FROM   [MultiProperty] mp INNER JOIN [MultiPropertyVideo] mpv ON 
       mp.multiPropertyID = mpv.multiPropertyID INNER JOIN Video vid on mpv.videoID = vid.VideoID
       WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT'
       
  
  and   mp.name = 'music' AND vid.isHidden = 0 AND vid.isEnabled = 1
  
  
       ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRandomVideoID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetRandomVideoID]

AS  
  
  SELECT TOP 1 videoID FROM  [Video]
  WHERE isEnabled = 1 AND isHidden = 0
ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentChatMessages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetRecentChatMessages]

 as

SELECT TOP 50 [chatRoomID]
      ,[userName]
      ,[chatMessage]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[ipAddress]
      ,[roomID]
  FROM [ChatRoom]
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentProfileViews]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetRecentProfileViews]

@lookedAtUserAccountID int

AS
 
  
  SELECT   *
FROM    (SELECT  [lookingUserAccountID], createDate,
                ROW_NUMBER() OVER (PARTITION BY [lookingUserAccountID] ORDER BY createDate desc) AS RowNumber
         FROM   [ProfileLog]
           WHERE lookedAtUserAccountID = @lookedAtUserAccountID) as a
WHERE   a.RowNumber = 1
  ORDER BY createDate desc 
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentRSS]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetRecentRSS]
 AS
SELECT TOP 100 [rssItemID]
      ,ri.[rssResourceID]
      ,ri.[updatedByUserID]
      ,ri.[createDate]
      ,ri.[updateDate]
      ,ri.[createdByUserID]
      ,[authorName]
      ,[commentsURL]
      ,[description]
      ,[pubDate]
      ,[title]
      ,[languageName]
      	   ,guidLink
			,link
  FROM  [RSSItem] ri INNER JOIN RssResource rr on ri.rssResourceID = rr.rssResourceID 
  where rr.isenabled = 1
  ORDER BY [pubDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentRSSFilter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetRecentRSSFilter]

@filter varchar(max)

 AS
 
SELECT TOP 100 [rssItemID]
      ,ri.[rssResourceID]
      ,ri.[updatedByUserID]
      ,ri.[createDate]
      ,ri.[updateDate]
      ,ri.[createdByUserID]
      ,[authorName]
      ,[commentsURL]
      ,[description]
      ,[pubDate]
      ,[title]
      ,[languageName]
      	   ,guidLink
			,link
  FROM  [RSSItem] ri INNER JOIN RssResource rr on ri.rssResourceID = rr.rssResourceID 
  where rr.isenabled = 1 AND [description] like '%' + @filter + '%'
  ORDER BY [pubDate] desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentStatusUpdates]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetRecentStatusUpdates]
AS
SELECT TOP 25 [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
  FROM [StatusUpdate]
  ORDER BY createdate desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetRecentUserAccountVideos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetRecentUserAccountVideos]

@userAccountID int,
@videoType char(1)

AS

SELECT TOP 7 [videoID]
      ,[userAccountID]
      ,[createDate]
      ,videoType
FROM [UserAccountVideo]
WHERE userAccountID = @userAccountID AND videoType = @videoType
ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetRelatedVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE  proc [dbo].[up_GetRelatedVideo]


@multiPropertyIDGenre int,
@multiPropertyIDVidType int,
@currentVideoID int

AS
 
SELECT Top 1 vid.[videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,vid.[updatedByUserID]
      ,vid.[createDate]
      ,vid.[updateDate]
      ,vid.[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,enableTrim
      ,publishdate
       FROM Video vid INNER JOIN [MultiPropertyVideo] mpv on  mpv.videoID = vid.VideoID
       WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and  vid.VideoID != @currentVideoID
         and mpv.multiPropertyID =@multiPropertyIDGenre  and  vid.videoID in (
          
         
SELECT   vid2.[videoID]
   
       FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID
 
       WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and  vid.VideoID != @currentVideoID
         and mpv2.multiPropertyID = @multiPropertyIDVidType 

      
      )
      
        ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRelatedVideos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetRelatedVideos]


@multiPropertyIDGenre int,
@multiPropertyIDVidType int,
@currentVideoID int

AS
 
SELECT Top 9  vid.[videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,vid.[updatedByUserID]
      ,vid.[createDate]
      ,vid.[updateDate]
      ,vid.[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,enableTrim
      ,publishdate
       FROM Video vid INNER JOIN [MultiPropertyVideo] mpv on  mpv.videoID = vid.VideoID
       WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and  vid.VideoID != @currentVideoID
         and mpv.multiPropertyID =@multiPropertyIDGenre  and  vid.videoID in (
          
         
SELECT   vid2.[videoID]
   
       FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID
 
       WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and  vid.VideoID != @currentVideoID
         and mpv2.multiPropertyID = @multiPropertyIDVidType 

      
      )
      
        ORDER BY NEWID()
GO
/****** Object:  StoredProcedure [dbo].[up_GetRelationshipStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetRelationshipStatus]

@relationshipStatusID int


as

SELECT [relationshipStatusID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [RelationshipStatus]
 WHERE relationshipStatusID = @relationshipStatusID


GO
/****** Object:  StoredProcedure [dbo].[up_GetReviewID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetReviewID]

@reviewID int 

AS

SELECT [reviewID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[productID]
      ,[authorName]
      ,[rating]
      ,[authorURL]
      ,[location]
      ,[eMail]
      ,[description]
      ,publishDate
      ,isApproved
  FROM [Review]
  WHERE reviewID = @reviewID
GO
/****** Object:  StoredProcedure [dbo].[up_GetReviews]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetReviews]
@productID  int

AS


SELECT  [reviewID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[productID]
      ,[authorName]
      ,[rating]
      ,[authorURL]
      ,[location]
      ,[eMail]
      ,[description]
      ,publishDate
      ,isApproved
  FROM [Review]
  WHERE productID = @productID
GO
/****** Object:  StoredProcedure [dbo].[up_GetRoleByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetRoleByName]

@roleName varchar(50)

AS

SELECT [roleID]
      ,[roleName]
      ,[createDate]
      ,[updatedDate]
      ,[createdByEndUserID]
      ,[updatedByEndUserID]
      ,[description]
  FROM [Role]
  WHERE roleName = @roleName
GO
/****** Object:  StoredProcedure [dbo].[up_GetRolesForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
  CREATE proc [dbo].[up_GetRolesForUser]
  
  @userAccountID int
  
  AS
  
  
  SELECT roleName FROM [Role] r INNER JOIN userAccountRole uar on r.roleID = uar.roleID WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetRssResourceByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetRssResourceByID]

@rssResourceID int

AS

SELECT  [rssResourceID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[rssResourceURL]
      ,[resourceName]
      ,isEnabled
		,artistID
		,providerKey
  FROM RssResource
  WHERE [rssResourceID] = @rssResourceID
GO
/****** Object:  StoredProcedure [dbo].[up_GetShippingCostProductCountry]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetShippingCostProductCountry]
 
 @productID int,
 @shippingCountry char(2)
 
 AS
 
 
SELECT  [shippingOptionID]
      ,[productID]
      ,[shippingCost]
      ,[shippingService]
      ,[shippingCountry]
      ,[shippingRegion]
      ,[taxPercent]
      ,[packageWeight]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
  FROM  [ShippingOption]
  WHERE productID = @productID AND 
  shippingCountry = @shippingCountry
GO
/****** Object:  StoredProcedure [dbo].[up_GetShopByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetShopByID]

@shopID int

as

SELECT [shopID]
      ,[userAccountID]
      ,[shopName]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[currencyType]
      ,[payPalEmail]
      ,[shopDetails]
  FROM  [Shop]
  WHERE shopID = @shopID
GO
/****** Object:  StoredProcedure [dbo].[up_GetShopForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetShopForUser]

@userAccountID int

AS
 
SELECT [shopID]
      ,[userAccountID]
      ,[shopName]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[currencyType]
      ,[payPalEmail]
      ,shopDetails
  FROM [Shop]
  WHERE userAccountID = @userAccountID 
GO
/****** Object:  StoredProcedure [dbo].[up_GetSiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetSiteDomain]

 @siteDomainID int

 as


SELECT [siteDomainID]
      ,[propertyType]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[language]
      ,[description]
  FROM [SiteDomain]
  WHERE  siteDomainID= @siteDomainID
GO
/****** Object:  StoredProcedure [dbo].[up_GetSiteDomainProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetSiteDomainProperty] 

@propertyType char(5)
 
as

SELECT [siteDomainID]
      ,[propertyType]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[language]
      ,[description]
  FROM [SiteDomain]
  WHERE propertyType = @propertyType 
GO
/****** Object:  StoredProcedure [dbo].[up_GetSiteDomainPropertyLanguage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetSiteDomainPropertyLanguage]

@propertyType char(5)
,@language char(2)
as

SELECT [siteDomainID]
      ,[propertyType]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[language]
      ,[description]
  FROM [SiteDomain]
  WHERE propertyType = @propertyType AND [language] = @language
GO
/****** Object:  StoredProcedure [dbo].[up_GetSizeByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetSizeByID]
 
 @sizeID int
 
 as
 
SELECT [sizeID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[sizeName]
      ,rankOrder
  FROM [Size]
  where sizeID = @sizeID
GO
/****** Object:  StoredProcedure [dbo].[up_GetSizeByName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetSizeByName]


@sizeName nvarchar(50)


as

SELECT [sizeID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[sizeName]
      ,[rankOrder]
  FROM [Size]
 WHERE sizeName = @sizeName
GO
/****** Object:  StoredProcedure [dbo].[up_GetSongByArtistIDName]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetSongByArtistIDName]

@artistID int
,@name nvarchar(150)

AS

SELECT  [songID]
      ,[artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
  FROM [Song]
  WHERE   artistID  = @artistID AND name = @name
GO
/****** Object:  StoredProcedure [dbo].[up_GetSongPropertySongIDTypeID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetSongPropertySongIDTypeID]
 
 @propertyType char(2),
 @songID int
 
 AS
 
SELECT [songPropertyID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[songID]
      ,[propertyContent]
      ,[propertyType]
  FROM [SongProperty]
  WHERE propertyType = @propertyType AND songID = @songID
GO
/****** Object:  StoredProcedure [dbo].[up_GetSongsForArtist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE PROC [dbo].[up_GetSongsForArtist]

@artistID int

AS

SELECT [songID]
      ,[artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
      ,songKey
  FROM  [Song]
  WHERE artistID = @artistID
GO
/****** Object:  StoredProcedure [dbo].[up_GetSongsForVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetSongsForVideo]

@videoID int

AS

SELECT sng.[songID]
      ,[artistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[name]
      ,songKey
      ,vs.rankOrder
  FROM [VideoSong] vs INNER JOIN Song sng on vs.songID = sng.songID
  WHERE vs.videoID = @videoID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStateForPostalCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROCEDURE [dbo].[up_GetStateForPostalCode]
 
 @postalcode varchar(10), @countrycode varchar(3)
 
 AS
 
SELECT top 1  [state]
  FROM  [GeoData]
  WHERE postalcode =  @postalcode AND countrycode = @countrycode
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetStatusComment]

@statusCommentID int

as

SELECT   [statusCommentID]
      ,[statusUpdateID]
      ,[userAccountID]
      ,[statusType]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[message]
    FROM  [StatusComment]
    WHERE statusCommentID  = @statusCommentID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusCommentCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetStatusCommentCount]
 
@statusUpdateID int
as
SELECT  COUNT(*)
  FROM StatusComment
  where statusUpdateID = @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusCommentMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetStatusCommentMessage]

@statusUpdateID int
,@message nvarchar(max)
,@userAccountID int
as
SELECT [statusCommentID]
      ,[statusUpdateID]
      ,[userAccountID]
      ,[statusType]
      ,[createdByUserID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[message]
  FROM [StatusComment]
  WHERE statusUpdateID = @statusUpdateID
  AND [Message] = @message
  AND userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdateByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetStatusUpdateByID]

@statusUpdateID int

As

SELECT [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
      ,[photoItemID]
      ,[zoneID]
      ,isMobile
  FROM [StatusUpdate]
  WHERE statusUpdateID = @statusUpdateID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdateByPhotoID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetStatusUpdateByPhotoID]

@photoItemID int

AS

SELECT  [statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
      ,[photoItemID]
      ,[zoneID]
      ,isMobile
  FROM  [StatusUpdate]
  WHERE photoItemID = @photoItemID
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdateNotificationCountForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetStatusUpdateNotificationCountForUser]
 
 @userAccountID int
 
 AS
 
SELECT  COUNT(*)
  FROM  [StatusUpdateNotification]
  WHERE userAccountID = @userAccountID AND isRead = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdateNotificationForUserStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
 CREATE proc [dbo].[up_GetStatusUpdateNotificationForUserStatus]
 
 @userAccountID int
 ,@statusUpdateID int
 ,@responseType char(1)                                     

 
 as
 
SELECT  statusUpdateNotificationID
      ,[statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isRead]
      ,[userAccountID]
      ,responseType
  FROM  StatusUpdateNotification
 WHERE userAccountID = @userAccountID AND statusUpdateID = @statusUpdateID AND responseType = @responseType
 
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdateNotificationsForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetStatusUpdateNotificationsForUser]
 
 @userAccountID int
 
 AS
 
SELECT  statusUpdateNotificationID
      ,[statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isRead]
      ,[userAccountID]
      ,responseType
  FROM  [StatusUpdateNotification]
  WHERE userAccountID = @userAccountID AND isRead = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetStatusUpdatesPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 
CREATE PROCEDURE [dbo].[up_GetStatusUpdatesPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY [createDate] DESC
      )AS RowNumber
   ,[statusUpdateID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[message]
      ,[statusType]
      ,[photoItemID]
      ,[zoneID]
      ,isMobile
    INTO #Results

  FROM  [StatusUpdate]

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END










GO
/****** Object:  StoredProcedure [dbo].[up_GetTopRssItemsForResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
 CREATE proc [dbo].[up_GetTopRssItemsForResource]
 
 @rssResourceID int
 
 AS
 
SELECT TOP 10 [rssItemID]
      ,[rssResourceID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[authorName]
      ,[commentsURL]
      ,[description]
      ,[pubDate]
      ,[title]
      ,[languageName]
      ,[artistID]
      ,[link]
      ,[guidLink]
  FROM [RSSItem]
  WHERE rssResourceID = @rssResourceID
  ORDER BY createDate DESc
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetUniqueProfileVisitorCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetUniqueProfileVisitorCount]
 
 @lookedAtUserAccountID int
 
 as
 
 
SELECT COUNT(distinct([lookingUserAccountID]))
  FROM  [ProfileLog] pl INNER JOIN UserAccountDetail uad on pl.lookedAtUserAccountID  = uad.userAccountID
  WHERE lookedAtUserAccountID = @lookedAtUserAccountID  and uad.enableProfileLogging = 1

GO
/****** Object:  StoredProcedure [dbo].[up_GetUnprocessedVideoRequests]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUnprocessedVideoRequests]

as
SELECT [videoRequestID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[requestURL]
      ,[statusType]
      ,[videoKey]
  FROM [VideoRequest]
  WHERE statusType IS NOT NULL AND statusType = 'W'
  ORDER BY createDate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountByEmail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserAccountByEmail]

@email varchar(75)

AS

SELECT [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount]
  WHERE email = @email
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountDetail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetUserAccountDetail]

@userAccountDetailID int

AS

SELECT  [userAccountDetailID]
      ,[userAccountID]
      ,[youAreID]
      ,[relationshipStatusID]
      ,[interestedInID]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserID]
      ,[createdByUserID]
      ,[country]
      ,[region]
      ,[city]
      ,[postalCode]
      ,[profilePicURL]
      ,[birthDate]
      ,[religion]
      ,[profileThumbPicURL]
      ,[ethnicity]
      ,[heightCM]
      ,[weightKG]
      ,[diet]
      ,[accountViews]
      ,[externalURL]
      ,[smokes]
      ,[drinks]
      ,[handed]
      ,[displayAge]
      ,[enableProfileLogging]
      ,[lastPhotoUpdate]
      ,[emailMessages]
      ,[showOnMap]
      ,[referringUserID]
      ,[browerType]
      ,[membersOnlyProfile]
      ,[messangerType]
      ,[messangerName]
      ,[aboutDesc]
      ,[bandsSeen]
      ,[bandsToSee]
      ,[hardwareSoftware]
      ,[firstName]
      ,[lastName]
      ,[defaultLanguage]
      ,[latitude]
      ,[longitude]
  FROM UserAccountDetail
  WHERE [userAccountDetailID] = @userAccountDetailID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountDetailForUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetUserAccountDetailForUser]

@userAccountID int

AS

SELECT  [userAccountDetailID]
      ,[userAccountID]
      ,[youAreID]
      ,[relationshipStatusID]
      ,[interestedInID]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserID]
      ,[createdByUserID]
      ,[country]
      ,[region]
      ,[city]
      ,[postalCode]
      ,[profilePicURL]
      ,[birthDate]
      ,[religion]
      ,[profileThumbPicURL]
      ,[ethnicity]
      ,[heightCM]
      ,[weightKG]
      ,[diet]
      ,[accountViews]
      ,[externalURL]
      ,[smokes]
      ,[drinks]
      ,[handed]
      ,[displayAge]
      ,[enableProfileLogging]
      ,[lastPhotoUpdate]
      ,[emailMessages]
      ,[showOnMap]
      ,[referringUserID]
      ,[browerType]
      ,[membersOnlyProfile]
      ,[messangerType]
      ,[messangerName]
      ,[aboutDesc]
      ,[bandsSeen]
      ,[bandsToSee]
      ,[hardwareSoftware]
      ,[firstName]
      ,[lastName]
      ,[defaultLanguage]
      ,[latitude]
      ,[longitude]
  FROM UserAccountDetail
  WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountFromID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
/*
	FROM: RMW
	DATE: 2009-04-15
	DESC: get the user's info from their id
*/

CREATE PROCEDURE [dbo].[up_GetUserAccountFromID]

	@userAccountID int

AS

SELECT [UserAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[createDate]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,ipAddress
FROM UserAccount eu WITH (NOLOCK)
WHERE eu.UserAccountID = @userAccountID
  



GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountFromUsername]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
/*
	FROM: RMW
	DATE: 2009-04-15
	DESC: get the user's info from their username
*/

CREATE PROCEDURE [dbo].[up_GetUserAccountFromUsername]

	@userName nvarchar(50)

AS

SELECT userAccountID
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[createDate]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,ipAddress
FROM UserAccount eu WITH (NOLOCK)
WHERE eu.userName = @userName
  



GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAccountnameFromEMail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserAccountnameFromEMail]
@eMail  varchar(50)
AS

SELECT [userName]
      
  FROM  [UserAccount]
  WHERE  eMail = @eMail
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserAddress]

@userAccountID int

AS

SELECT [userAddressID]
      ,[firstName]
      ,[middleName]
      ,[lastName]
      ,[addressLine1]
      ,[addressLine2]
      ,[addressLine3]
      ,[city]
      ,[region]
      ,[postalCode]
      ,[countryISO]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[addressStatus]
      ,[choice1]
      ,[choice2]
  FROM [UserAddress]
  WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAddressByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserAddressByID]

@userAddressID int

AS

SELECT [userAddressID]
      ,[firstName]
      ,[middleName]
      ,[lastName]
      ,[addressLine1]
      ,[addressLine2]
      ,[addressLine3]
      ,[city]
      ,[region]
      ,[postalCode]
      ,[countryISO]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[addressStatus]
      ,[choice1]
      ,[choice2]
  FROM  [UserAddress]
  where userAddressID = @userAddressID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAddressesByStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserAddressesByStatus]

@addressStatus char(1)

as

SELECT  [userAddressID]
      ,[firstName]
      ,[middleName]
      ,[lastName]
      ,[addressLine1]
      ,[addressLine2]
      ,[addressLine3]
      ,[city]
      ,[region]
      ,[postalCode]
      ,[countryISO]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[userAccountID]
      ,[addressStatus]
      ,[choice1]
      ,[choice2]
  FROM  [UserAddress]
  WHERE addressStatus = @addressStatus
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserAffReport]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetUserAffReport]
 
 @referringUserID int
 
 AS
 
SELECT 
      ua.createDate
      ,ua.lastactivitydate
      ,ua.username
  FROM [UserAccountDetail] uad INNER JOIN UserAccount ua on uad.useraccountid = ua.useraccountid
  WHERE [referringUserID] = @referringUserID
  ORDER BY ua.createDate Desc
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserConnection]

  @fromUserAccountID INT 

as

SELECT [userConnectionID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,isConfirmed
  FROM [UserConnection]
  WHERE fromUserAccountID = @fromUserAccountID
  UNION
  SELECT [userConnectionID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,isConfirmed
  FROM [UserConnection]
  WHERE [toUserAccountID] = @fromUserAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserConnectionByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserConnectionByID]

@userConnectionID int

AS

SELECT   [userConnectionID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[isConfirmed]
  FROM  [UserConnection]
  WHERE userConnectionID = @userConnectionID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserContentComments]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserContentComments]

@createdByUserID int

as

SELECT  [contentCommentID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[detail]
      ,[contentID]
      ,[fromName]
      ,[fromEmail]
      ,[ipAddress]
  FROM [ContentComment]
  WHERE createdByUserID= @createdByUserID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserCount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserCount]
as

SELECT COUNT(*)
  FROM [UserAccount] WITH (NOLock)
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserGroupID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetUserGroupID]

@userAccountID int

AS

SELECT [userGroupId]
  FROM [UserAccountUserGroup]
  WHERE userAccountID = @userAccountID
  
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserPhotoByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetUserPhotoByID]

@userPhotoID int

AS

SELECT [userPhotoID]
      ,[userAccountID]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserID]
      ,[createdByUserID]
      ,[picURL]
      ,[thumbPicURL]
      ,[description]
      ,[rankOrder]
  FROM [UserPhoto]
  WHERE userPhotoID = @userPhotoID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserPhotos]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetUserPhotos]
 
 @userAccountID int
 
 AS
 
SELECT [userPhotoID]
      ,[userAccountID]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserID]
      ,[createdByUserID]
      ,[picURL]
      ,[thumbPicURL]
      ,[description]
      ,[rankOrder]
  FROM [UserPhoto]
  WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserPlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserPlaylist]

@userAccountID int

AS

SELECT [playlistID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[playlistBegin]
      ,[playListName]
      ,[userAccountID]
      ,[autoPlay]
  FROM [Playlist]
  WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUsersInRole]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetUsersInRole]
 
 @roleID int
 as
SELECT  ua.[userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]
  FROM [UserAccount] ua INNER JOIN [UserAccountRole] uar ON ua.userAccountID = uar.userAccountID
  AND uar.roleID = @roleID
GO
/****** Object:  StoredProcedure [dbo].[up_GetUserToUserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetUserToUserConnection]

    @fromUserAccountID int,
    @toUserAccountID int

AS

SELECT [userConnectionID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[isConfirmed]
  FROM [UserConnection]
  WHERE fromUserAccountID = @fromUserAccountID
  AND toUserAccountID = @toUserAccountID
  UNION 
  SELECT [userConnectionID]
      ,[fromUserAccountID]
      ,[toUserAccountID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[statusType]
      ,[isConfirmed]
  FROM [UserConnection]
  WHERE fromUserAccountID = @toUserAccountID
  AND toUserAccountID = @fromUserAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_GetVenueByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVenueByID]

@venueID int

AS

SELECT [venueID]
		,[venueName]
		,[addressLine1]
		,[addressLine2]
		,[city]
		,[region]
		,[postalCode]
		,[countryISO]
		,[updatedByUserID]
		,[createDate]
		,[updateDate]
		,[createdByUserID]
		,[venueURL]
		,[isEnabled]
		,latitude
		,longitude
		,phoneNumber
		,venueType
		,[description]

  FROM [Venue]
  WHERE venueID = @venueID
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideoByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideoByID]

@videoID int

AS
 
SELECT  [videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,publishDate
  FROM  [Video]
  WHERE videoID = @videoID
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideoByProviderKeyCode]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_GetVideoByProviderKeyCode]


@providerKey varchar(50)
, @providerCode char(2)

AS

SELECT [videoID]
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,volumeLevel
      ,publishDate
  FROM  [Video]
  WHERE providerKey = @providerKey AND providerCode = @providerCode
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideoRequest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideoRequest]

@videoKey varchar(20)

AS

SELECT [videoRequestID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[requestURL]
      ,[statusType]
      ,[videoKey]
  FROM [VideoRequest]
  where videoKey = @videoKey
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideoRequestByID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_GetVideoRequestByID]
 
 @videoRequestID int
 
 AS
 
SELECT [videoRequestID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[requestURL]
      ,[statusType]
      ,[videoKey]
  FROM [VideoRequest]
  WHERE videoRequestID = @videoRequestID
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideosForPropertyType]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideosForPropertyType]

@multiPropertyID int

AS

SELECT vid.videoID 
  FROM [MultiProperty] mp INNER JOIN [MultiPropertyVideo] mpv ON 
  mp.multiPropertyID = mpv.multiPropertyID INNER JOIN Video vid on mpv.videoID = vid.VideoID
  WHERE   mp.multiPropertyID = @multiPropertyID AND vid.isHidden = 0 AND vid.isEnabled = 1
     ORDER BY vid.publishdate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideosForPropertyType2]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideosForPropertyType2]

@multiPropertyID1 int,
@multiPropertyID2 int

AS

SELECT vid.videoID 
  FROM [MultiProperty] mp INNER JOIN [MultiPropertyVideo] mpv ON 
  mp.multiPropertyID = mpv.multiPropertyID INNER JOIN Video vid on mpv.videoID = vid.VideoID
  WHERE   mp.multiPropertyID = @multiPropertyID1 AND vid.isHidden = 0 AND vid.isEnabled = 1
 
   AND vid.videoID IN 
  (
  SELECT vid.videoID 
  FROM [MultiProperty] mp INNER JOIN [MultiPropertyVideo] mpv ON 
  mp.multiPropertyID = mpv.multiPropertyID INNER JOIN Video vid on mpv.videoID = vid.VideoID
  WHERE   mp.multiPropertyID = @multiPropertyID2 AND vid.isHidden = 0 AND vid.isEnabled = 1 
   
  )
  
   ORDER BY vid.publishdate DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideosForSong]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE PROC [dbo].[up_GetVideosForSong]

@songID int

AS

SELECT distinct vid.videoID
      ,[videoKey]
      ,[providerKey]
      ,[providerUserKey]
      ,[providerCode]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[isHidden]
      ,[isEnabled]
      ,[statusID]
      ,[duration]
      ,[intro]
      ,[lengthFromStart]
      ,[volumeLevel]
      ,[enableTrim]
      ,publishDate
  FROM [Video] vid INNER JOIN VideoSong vsong ON vid.videoID = vsong.videoID
  WHERE songID = @songID AND vid.isEnabled = 1 AND vid.isHidden = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideosForUserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideosForUserAccount]

@userAccountID int,
@videoType char(1)

AS

SELECT [videoID]
      ,[userAccountID]
      ,[createDate]
      ,videoType
  FROM  [UserAccountVideo]
  WHERE userAccountID = @userAccountID AND videoType = @videoType
GO
/****** Object:  StoredProcedure [dbo].[up_GetVideoViews]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetVideoViews]

@videoID int

AS

SELECT COUNT(videologid)
  FROM [VideoLog]
  WHERE videoID = @videoID
GO
/****** Object:  StoredProcedure [dbo].[up_GetWallMessagessPageWise]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 
CREATE PROCEDURE [dbo].[up_GetWallMessagessPageWise]
      @PageIndex INT = 1
      ,@PageSize INT = 10
	  ,@toUserAccountID int
      ,@RecordCount INT OUTPUT
AS

BEGIN

      SET NOCOUNT ON;
      SELECT ROW_NUMBER() OVER
      (
ORDER BY [createDate] DESC
      )AS RowNumber
	,[wallMessageID]
	,[updatedByUserID]
	,[createDate]
	,[updateDate]
	,[createdByUserID]
	,[message]
	,[isRead]
	,[fromUserAccountID]
	,[toUserAccountID]
    INTO #Results

  FROM  [WallMessage]
  WHERE toUserAccountID = @toUserAccountID

	SELECT @RecordCount = COUNT(*)
    FROM #Results
           
      SELECT * FROM #Results
      WHERE RowNumber BETWEEN(@PageIndex -1) * @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
     
      DROP TABLE #Results
END



 
GO
/****** Object:  StoredProcedure [dbo].[up_GetWallMessagesUserCountUnread]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetWallMessagesUserCountUnread]

@toUserAccountID int

AS

select count ([wallMessageID])
  FROM  [WallMessage]
  WHERE toUserAccountID  = @toUserAccountID and isRead = 0
GO
/****** Object:  StoredProcedure [dbo].[up_GetWhoIsOffline]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetWhoIsOffline]
 
as
SELECT [userAccountID]
      ,[userName]
      ,[password]
      ,[passwordFormat]
      ,[passwordSalt]
      ,[eMail]
      ,[passwordQuestion]
      ,[passwordAnswer]
      ,[isApproved]
      ,[lastLoginDate]
      ,[lastPasswordChangeDate]
      ,[lastLockoutDate]
      ,[failedPasswordAttemptCount]
      ,[failedPasswordAttemptWindowStart]
      ,[failedPasswordAnswerAttemptCount]
      ,[failedPasswordAnswerAttemptWindowStart]
      ,[comment]
      ,[createDate]
      ,[updateDate]
      ,[updatedByUserAccountID]
      ,[createdByUserAccountID]
      ,[isOnline]
      ,[isLockedOut]
      ,[lastActivityDate]
      ,[ipAddress]

FROM [UserAccount]

WHERE isOnline = 1 AND  lastActivityDate < DATEADD(minute, -10, GETUTCDATE()) 

GO
/****** Object:  StoredProcedure [dbo].[up_GetWishListProducts]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_GetWishListProducts]

@createdByUserID int

AS

SELECT [productID]
      ,[createdByUserID]
      ,[createDate]
  FROM [WishList]
  WHERE [createdByUserID] = @createdByUserID
  ORDER BY CREATEDATE DESC
GO
/****** Object:  StoredProcedure [dbo].[up_GetYouAre]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_GetYouAre]

@youAreID int

as
 
SELECT  [youAreID]
      ,[updatedByUserID]
      ,[createDate]
      ,[updateDate]
      ,[createdByUserID]
      ,[typeLetter]
      ,[name]
  FROM  [YouAre]
  WHERE youAreID = @youAreID
GO
/****** Object:  StoredProcedure [dbo].[up_IsAccountIPTaken]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE PROC [dbo].[up_IsAccountIPTaken]

 @ipAddress  varchar(25)

 as

 IF EXISTS (SELECT * FROM UserAccount WHERE ipAddress = @ipAddress)
 BEGIN SELECT 1 END
 ELSE BEGIN SELECT 0 END
  
GO
/****** Object:  StoredProcedure [dbo].[up_IsBlockedUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_IsBlockedUser]


@userAccountIDBlocking int
,@userAccountIDBlocked int
           
as           


IF EXISTS(SELECT *  FROM BlockedUser
  WHERE  userAccountIDBlocking = @userAccountIDBlocking AND userAccountIDBlocked = @userAccountIDBlocked)
  BEGIN 
  SELECT 1
  END
  ELSE SELECT 0
 
GO
/****** Object:  StoredProcedure [dbo].[up_IsBlockingUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_IsBlockingUser]


@userAccountIDBlocking int
,@userAccountIDBlocked int
           
as           


IF EXISTS(SELECT *  FROM BlockedUser
  WHERE  (userAccountIDBlocking = @userAccountIDBlocking AND userAccountIDBlocked = @userAccountIDBlocked) OR
  (userAccountIDBlocking = @userAccountIDBlocked AND userAccountIDBlocked =  @userAccountIDBlocking)
  )
  BEGIN 
  SELECT 1
  END
  ELSE SELECT 0
 
GO
/****** Object:  StoredProcedure [dbo].[up_IsIPBlocked]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_IsIPBlocked]
 
    @ipAddress varchar(255)
 
 AS
 
 IF EXISTS(
SELECT  [ipAddress]
  FROM  [BlackIPID]
  WHERE ipAddress = @ipAddress)
  BEGIN SELECT 1 END
  ELSE BEGIN SELECT 0 END
GO
/****** Object:  StoredProcedure [dbo].[up_IsPlaylistVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_IsPlaylistVideo]


@playlistID int
,@videoID int

as

IF EXISTS(
SELECT *
  FROM [PlaylistVideo]
  WHERE playlistID = @playlistID AND videoID = @videoID)
  BEGIN SELECT 1 END
  ELSE BEGIN SELECT 0 END
GO
/****** Object:  StoredProcedure [dbo].[up_IsRssItemExist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_IsRssItemExist]
 
  @rssResourceID  int,
  @pubDate datetime
 
 AS


IF EXISTS(SELECT * FROM RssItem WHERE rssResourceID = @rssResourceID AND pubDate = @pubDate)
BEGIN SELECT 1 END
ELSE BEGIN SELECT 0 END

GO
/****** Object:  StoredProcedure [dbo].[up_IsUserAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_IsUserAcknowledgement]

@statusUpdateID int, @userAccountID int

AS

IF EXISTS(SELECT *  FROM [Acknowledgement]
  WHERE statusUpdateID = @statusUpdateID AND userAccountID = @userAccountID)
  BEGIN 
  SELECT 1
  END
  ELSE SELECT 0
GO
/****** Object:  StoredProcedure [dbo].[up_IsUserCommentAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_IsUserCommentAcknowledgement]

@statusCommentID int, @userAccountID int

AS

IF EXISTS(SELECT *  FROM [StatusCommentAcknowledgement]
  WHERE  statusCommentID = @statusCommentID AND userAccountID = @userAccountID)
  BEGIN 
  SELECT 1
  END
  ELSE SELECT 0
GO
/****** Object:  StoredProcedure [dbo].[up_IsUserContestVoted]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC		[dbo].[up_IsUserContestVoted]

@userAccountID int,
@contestID int

as

IF EXISTS ( 
SELECT * 
  FROM [ContestVideo] cv INNER JOIN ContestVideoVote cvv 
	ON cv.contestVideoID = cvv.contestVideoID
WHERE cv.contestID = @contestID AND cvv.userAccountID = @userAccountID
)
BEGIN SELECT 1 END
ELSE BEGIN SELECT 0 END
GO
/****** Object:  StoredProcedure [dbo].[up_IsUserOnline]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_IsUserOnline]
 
 @userAccountID int 
 
 as
 
SELECT isOnline
  FROM [UserAccount]
  WHERE userAccountID = @userAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_IsUserVideoVote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_IsUserVideoVote]

@videoID int, @userAccountID int

AS

IF EXISTS(SELECT * FROM [Vote] WHERE videoID = @videoID AND userAccountID = @userAccountID)
BEGIN

select 1

END
ELSE 

BEGIN

SELECT 0

END
GO
/****** Object:  StoredProcedure [dbo].[up_MostFrequentStatusMessages]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

 CREATE proc [dbo].[up_MostFrequentStatusMessages]

 as

  SELECT Tbl1.message
FROM ( SELECT message, createDate FROM [StatusUpdate]
     --WHERE createDate > GETUTCDATE() 
       UNION
       SELECT message, createDate FROM StatusComment
	   --   WHERE createDate > GETUTCDATE() - 7
	   
     ) Tbl1
	
ORDER BY Tbl1.createDate DESC

GO
/****** Object:  StoredProcedure [dbo].[up_SetContent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
 CREATE proc [dbo].[up_SetContent]
 
           @title nvarchar(150) 
           ,@updatedByUserID int 
           ,@createdByUserID int 
           ,@detail nvarchar (max)
           ,@metaDescription nvarchar(500) 
           ,@metaKeywords nvarchar(500) 
           ,@contentTypeID int 
           ,@releaseDate datetime 
           ,@contentID  int
           ,@contentKey nvarchar(150)
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

GO
/****** Object:  StoredProcedure [dbo].[up_UpdateArtist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_UpdateArtist]

 
 @updatedByUserID int 
,@isHidden bit 
,@name nvarchar(50) 
,@artistID int 
,@altName nvarchar(50) 
AS

UPDATE  [Artist]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[isHidden] = @isHidden
      ,[name] = @name
      ,altName = @altName
 WHERE  artistID = @artistID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdateArtistProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateArtistProperty]

@updatedByUserID int
,@artistID int
,@propertyContent varchar(max)
,@propertyType char(2)
,@artistPropertyID int

      as

UPDATE [ArtistProperty]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[artistID] = @artistID
      ,[propertyContent] = @propertyContent
      ,[propertyType] = @propertyType
 WHERE artistPropertyID = @artistPropertyID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateBrand]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateBrand]

 @brandKey nvarchar(75)
,@name nvarchar(50)
,@description nvarchar(900)
,@updatedByUserID int
,@brandImageMain varchar(255)
,@brandImageThumb varchar(255)
,@isEnabled bit
,@brandID int
,@siteDomainID int
,@metaDescription nvarchar(155)
,@userAccountID int

      AS
      
UPDATE [Brand]
   SET [brandKey] = @brandKey
      ,[name] = @name
      ,[description] = @description
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[brandImageMain] = @brandImageMain
      ,[brandImageThumb] = @brandImageThumb
      ,[isEnabled] = @isEnabled 
      ,siteDomainID = @siteDomainID
      ,metaDescription  = @metaDescription
      ,userAccountID = @userAccountID
 WHERE brandID = @brandID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateCategory]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateCategory]

 @departmentID int
,@name nvarchar(50)
,@description nvarchar(255)
,@updatedByUserID int
,@categoryID int
,@categoryKey nvarchar(75)
as

UPDATE [Category]
   SET [departmentID] = @departmentID
      ,[name] = @name
      ,[description] = @description
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,categoryKey= @categoryKey
 WHERE categoryID = @categoryID

GO
/****** Object:  StoredProcedure [dbo].[up_UpdateChatRoomUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateChatRoomUser]

    @updatedByUserID int
      ,@ipAddress varchar(50)
      ,@roomID int
	  ,@connectionCode varchar(50)

as

UPDATE [dbo].[ChatRoomUser]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[ipAddress] = @ipAddress
      ,[roomID] = @roomID
      ,[connectionCode] = @connectionCode
 WHERE  createdByUserID = @updatedByUserID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdateContentComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateContentComment] 


@updatedByUserID int
,@statusType char(1)
,@detail varchar(max)
,@contentID int
,@fromName varchar(50)
,@fromEmail varchar(50)
,@ipAddress varchar(50)
,@contentCommentID int
      
      as

UPDATE  [ContentComment]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[statusType] = @statusType
      ,[detail] = @detail
      ,[contentID] = @contentID
      ,[fromName] = @fromName
      ,[fromEmail] = @fromEmail
      ,[ipAddress] = @ipAddress
 WHERE contentCommentID = @contentCommentID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateDirectMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_UpdateDirectMessage]

	   @directMessageID int
	   ,@updatedByUserID int
      ,@fromUserAccountID int
      ,@toUserAccountID int
      ,@isRead bit
      ,@message nvarchar(max)
      ,@isEnabled bit

AS

UPDATE  [DirectMessage]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[fromUserAccountID] = @fromUserAccountID
      ,[toUserAccountID] = @toUserAccountID
      ,[isRead] = @isRead
      ,[message] = @message
      ,[isEnabled] = @isEnabled
 WHERE directMessageID = @directMessageID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateEvent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_UpdateEvent]


	@name nvarchar(50)
	,@venueID int 
	,@updatedByUserID int 
	,@localTimeBegin datetime 
	,@notes nvarchar(max) 
	,@ticketURL nvarchar(max) 
	,@localTimeEnd datetime 
	,@eventCycleID int 
	,@rsvpURL nvarchar(max) 
	,@isReoccuring bit 
	,@isEnabled bit 
	,@eventDetailURL nvarchar(max)
	,@eventID int

      AS

UPDATE [Event]
   SET [name] = @name
      ,[venueID] = @venueID
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[localTimeBegin] = @localTimeBegin
      ,[notes] = @notes
      ,[ticketURL] = @ticketURL
      ,[localTimeEnd] = @localTimeEnd
      ,[eventCycleID] = @eventCycleID
      ,[rsvpURL] = @rsvpURL
      ,[isReoccuring] = @isReoccuring
      ,[isEnabled] = @isEnabled
      ,[eventDetailURL] = @eventDetailURL
 WHERE  eventID = @eventID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdateMultiProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_UpdateMultiProperty]



	 @propertyTypeID int
	,@name varchar(50)
	,@updatedByUserID int
	,@propertyContent varchar(max)
	,@multiPropertyID int

      AS

UPDATE [MultiProperty]
   SET [propertyTypeID] = @propertyTypeID
      ,[name] = @name
      ,[updateDate] = GETUTCDATE()
      ,[updatedByUserID] = @updatedByUserID
      ,[propertyContent] = @propertyContent
 WHERE multiPropertyID = @multiPropertyID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateOrder]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateOrder]

@updatedByUserID int
,@status char(1)
,@totalCost smallmoney
,@transactionResponse varchar(max)
,@countryTo char(2)
,@currency char(3)
,@contactEmail varchar(50)
,@orderID int
,@IpAddress varchar(25)
,@sessionGUID	uniqueidentifier
 
 as


UPDATE  [Order]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[status] = @status
      ,[totalCost] = @totalCost
      ,[transactionResponse] = @transactionResponse
      ,[countryTo] = @countryTo
      ,[currency] = @currency
      ,[contactEmail] = @contactEmail
      ,IpAddress = @IpAddress
      ,sessionGUID = @sessionGUID
 WHERE  orderID = @orderID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdatePhotoItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdatePhotoItem]

@photoItemID int
,@updatedByUserID int 
      ,@title varchar(100)
      ,@filePathRaw varchar(255)
      ,@filePathThumb varchar(255)
      ,@filePathStandard varchar(255)
      
      
AS


UPDATE  [PhotoItem]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[title] = @title
      ,[filePathRaw] = @filePathRaw
      ,[filePathThumb] = @filePathThumb
      ,[filePathStandard] = @filePathStandard
 WHERE photoItemID = @photoItemID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdatePlaylist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdatePlaylist]

	@updatedByUserID int
	,@playlistBegin datetime
	,@playListName varchar(50)
	,@playlistID int
	,@userAccountID int
	,@autoPlay bit
      
      AS

UPDATE [Playlist]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[playlistBegin] = @playlistBegin
      ,[playListName] = @playListName
      ,userAccountID = @userAccountID
      ,autoPlay = @autoPlay
 WHERE  playlistID = @playlistID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdatePlaylistVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[up_UpdatePlaylistVideo]

@playlistID int
,@videoID int
,@updatedByUserID int
,@rankOrder smallint
,@playlistVideoID int
           
AS           

UPDATE [PlaylistVideo]
   SET [playlistID] = @playlistID
      ,[videoID] = @videoID
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[rankOrder] = @rankOrder
 WHERE playlistVideoID = @playlistVideoID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateRssResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/

CREATE proc [dbo].[up_UpdateRssResource]

@rssResourceID int
,@updatedByUserID int
,@rssResourceURL varchar(400)
,@resourceName varchar(150)
,@providerKey char(2)
,@isEnabled bit
,@artistID int
      
      AS


UPDATE  [RssResource]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[rssResourceURL] = @rssResourceURL
      ,[resourceName] = @resourceName
      ,[providerKey] = @providerKey
      ,[isEnabled] = @isEnabled
      ,[artistID] = @artistID
 WHERE  rssResourceID = @rssResourceID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateSiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateSiteDomain]

		@propertyType char(5)
      ,@updatedByUserID int
      ,@language char(2)
      ,@description nvarchar(max)
	  ,@siteDomainID int

as

 
UPDATE [dbo].[SiteDomain]
   SET [propertyType] = @propertyType
      ,updateDate = GETUTCDATE()
      ,updatedByUserID = @updatedByUserID
      ,[language] = @language
      ,[description] = @description
 WHERE siteDomainID = @siteDomainID

GO
/****** Object:  StoredProcedure [dbo].[up_UpdateSong]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_UpdateSong]

@artistID int
,@updatedByUserID int
 
,@isHidden bit
,@name nvarchar(150)
,@songID int
,@songKey  nvarchar(150)
      
      AS


UPDATE [Song]
   SET [artistID] = @artistID
      ,updateDate = GETUTCDATE()
      ,updatedByUserID = @updatedByUserID
      ,[isHidden] = @isHidden
      ,[name] = @name
      ,songKey = @songKey
 WHERE  songID = @songID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateStatusUpdateNotification]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateStatusUpdateNotification]

	   @statusUpdateID int
      ,@updatedByUserID int
      ,@isRead bit
      ,@userAccountID int
      ,@statusUpdateNotificationID int
      ,@responseType char(1)
      
      as

UPDATE [StatusUpdateNotification]
   SET [statusUpdateID] = @statusUpdateID
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[isRead] = @isRead
      ,[userAccountID] = @userAccountID
      ,responseType = @responseType
 WHERE  statusUpdateNotificationID = @statusUpdateNotificationID
 
 
 
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateUserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
/*
	FROM: RMW
	DATE: 2009-04-15
	DESC: update a user
	UPDT: 2012-06-05
*/


CREATE PROCEDURE [dbo].[up_UpdateUserAccount]
	
	 @UserAccountID int
	,@username nvarchar(30)
	,@email nvarchar(75)
	,@password varchar(50)
	,@passwordFormat varchar(50)
	,@passwordSalt varchar(50)
	,@passwordQuestion varchar(50)
	,@passwordAnswer  varchar(50)
	,@failedPasswordAttemptCount int
	,@failedPasswordAttemptWindowStart DATETIME
	,@isOnLine bit
	,@isApproved bit
	,@comment varchar(255)
	,@isLockedOut  bit
	,@failedPasswordAnswerAttemptCount int
	,@failedPasswordAnswerAttemptWindowStart DATETIME
	,@lastLoginDate datetime
	,@ipAddress varchar(25)
	
	AS

UPDATE UserAccount
   SET Username = @username
      ,Email = @email
      ,[Password] = @password
      ,PasswordFormat = @passwordFormat
      ,PasswordQuestion = @passwordQuestion
      ,PasswordAnswer = @passwordAnswer
      ,lastLoginDate = @lastLoginDate
      --,LastLockoutDate = GETUTCDATE()
      ,isOnLine = @isOnLine
      ,IsApproved = @isApproved
      ,Comment = @comment
      --,LastPasswordChangedDate = GETUTCDATE()
      ,isLockedOut = @isLockedOut
      ,lastActivityDate = GETUTCDATE()
      ,failedPasswordAttemptCount = @failedPasswordAttemptCount
      ,failedPasswordAttemptWindowStart = @failedPasswordAttemptWindowStart
	,updateDate = GETUTCDATE()
    
      /*
      -- if this is the first invalid attempt and they aren't online, get the time
      -- if they are online, set it to null
      -- if they their invalid attempt is greater then 1, keep the time 
      ,FailedPasswordAttemptWindowStart = 
		CASE WHEN @FailedPasswordAttemptCount = 1 AND @isOnLine = 0 THEN GETUTCDATE()
			 WHEN @isOnLine = 1 THEN NULL ELSE 
		(SELECT FailedPasswordAttemptWindowStart FROM end_user WHERE end_user_ID = @end_user_ID)
		 END
	*/
      ,failedPasswordAnswerAttemptCount = @FailedPasswordAnswerAttemptCount
      ,failedPasswordAnswerAttemptWindowStart = @failedPasswordAnswerAttemptWindowStart
      -- if this is the first invalid attempt and they aren't online, get the time
      -- if they are online, set it to null
      -- if they their invalid attempt is greater then 1, keep the time 
	/*
      ,FailedPasswordAnswerAttemptWindowStart = 
		CASE WHEN @failedPasswordAnswerAttemptCount = 1 AND @isOnLine = 0 THEN GETUTCDATE()
			 WHEN @isOnLine = 1 THEN NULL ELSE 
		(SELECT FailedPasswordAnswerAttemptWindowStart FROM end_user WHERE end_user_ID = @end_user_ID)
		 END
	*/
		,ipAddress = @ipAddress

 WHERE UserAccountID = @UserAccountID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateUserAccountDetail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateUserAccountDetail]

 @userAccountDetailID int 
,@userAccountID int 
,@updatedByUserID int 
,@country char(2) 
,@region varchar(25) 
,@city varchar(25) 
,@postalCode varchar(15) 
 
,@profilePicURL varchar(75) 
,@aboutDesc nvarchar(max)

 ,@interestedInID	int	 
,@youAreID	int	 
,@relationshipStatusID	int

 
,@birthDate date 
,@religion char(1) 
,@ethnicity char(1) 
,@heightCM float 
,@weightKG float 
,@diet char(1) 
,@accountViews int 
,@externalURL varchar(100) 
,@smokes char(1) 
,@drinks char(1) 
,@handed char(1)
,@displayAge bit
,@profileThumbPicURL varchar(75)
,@bandsSeen nvarchar(max)
,@bandsToSee nvarchar(max)
,@enableProfileLogging bit
,@lastPhotoUpdate datetime
,@emailMessages	bit
,@showOnMap bit
,@referringUserID int
,@browerType	varchar(15)
,@membersOnlyProfile	bit
,@messangerName varchar(25)
,@messangerType char(2)
,@hardwareSoftware nvarchar(max)
,@firstName	nvarchar(20)	 
,@lastName	nvarchar(20)
,@defaultLanguage char(2)
,@latitude decimal(9,6)
,@longitude decimal(9,6)

AS

UPDATE [UserAccountDetail]
 SET 
 userAccountID = @userAccountID
,updatedByUserID = @updatedByUserID
,country = @country
,region = @region
,city = @city
,postalCode  = @postalCode
 ,interestedInID = @interestedInID
,youAreID = @youAreID
,relationshipStatusID = @relationshipStatusID

,profilePicURL = @profilePicURL
,aboutDesc = @aboutDesc
 
,birthDate = @birthDate
,religion  = @religion
,ethnicity = @ethnicity
,heightCM = @heightCM
,weightKG = @weightKG
,diet = @diet
,accountViews = @accountViews
,externalURL = @externalURL
,smokes = @smokes
,drinks = @drinks
,handed = @handed
,updateDate= GETUTCDATE()
,displayAge = @displayAge
,profileThumbPicURL = @profileThumbPicURL
 ,bandsSeen  = @bandsSeen 
,bandsToSee  = @bandsToSee 
,enableProfileLogging = @enableProfileLogging
,lastPhotoUpdate = @lastPhotoUpdate
,emailMessages = @emailMessages
,showOnMap = @showOnMap
,referringUserID = @referringUserID
,browerType = @browerType
,membersOnlyProfile = @membersOnlyProfile
,messangerName  = @messangerName
,messangerType = @messangerType
,hardwareSoftware = @hardwareSoftware
,firstName = @firstName
,lastName = @lastName
,defaultLanguage = @defaultLanguage
,latitude = @latitude
,longitude = @longitude

 WHERE userAccountDetailID  = @userAccountDetailID 
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateUserAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateUserAddress]

 @userAddressID int
,@firstName nvarchar(50)
,@middleName nvarchar(50)
,@lastName nvarchar(50)
,@addressLine1 nvarchar(75)
,@addressLine2 nvarchar(50)
,@addressLine3 nvarchar(50)
,@city nvarchar(50)
,@region nvarchar(50)
,@postalCode nvarchar(50)
,@countryISO char(2)
,@updatedByUserID int
,@userAccountID int
,@addressStatus char(1)
,@choice1 varchar(max)
,@choice2 varchar(max)

As

UPDATE [UserAddress]
   SET [firstName] = @firstName
      ,[middleName] = @middleName
      ,[lastName] = @lastName
      ,[addressLine1] = @addressLine1
      ,[addressLine2] = @addressLine2
      ,[addressLine3] = @addressLine3
      ,[city] = @city
      ,[region] = @region
      ,[postalCode] = @postalCode
      ,[countryISO] = @countryISO
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[userAccountID] = @userAccountID
      ,[addressStatus] = @addressStatus
      ,[choice1] = @choice1
      ,[choice2] = @choice2
 WHERE  userAddressID = @userAddressID

GO
/****** Object:  StoredProcedure [dbo].[up_UpdateUserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateUserConnection]

@fromUserAccountID int
,@toUserAccountID int
,@updatedByUserID int
,@statusType char(1)
,@userConnectionID int
,@isConfirmed	bit

AS

UPDATE [UserConnection]
   SET [fromUserAccountID] = @fromUserAccountID
      ,[toUserAccountID] = @toUserAccountID
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[statusType] = @statusType
      ,isConfirmed = @isConfirmed
 WHERE  userConnectionID = @userConnectionID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdateUserPhoto]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateUserPhoto]

@userPhotoID int
,@userAccountID int
,@updatedByUserID int
,@picURL varchar(75)
,@thumbPicURL varchar(75)
,@description varchar(255)
,@rankOrder tinyint
 
      as

UPDATE [UserPhoto]
   SET [userAccountID] = @userAccountID
      ,[updateDate] = GETUTCDATE()
      ,[updatedByUserID] = @updatedByUserID
      ,[picURL] = @picURL
      ,[thumbPicURL] = @thumbPicURL
      ,[description] = @description
      ,[rankOrder] = @rankOrder
 WHERE userPhotoID = @userPhotoID




GO
/****** Object:  StoredProcedure [dbo].[up_UpdateVenue]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
  /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateVenue]
           
 @venueName	nvarchar(50)
,@addressLine1	nvarchar(50)
,@addressLine2	nvarchar(25)
,@city	nvarchar(25)
,@region	nvarchar(20)
,@postalCode	nvarchar(15)
,@countryISO	char(2)
,@updatedByUserID	int
,@venueURL	nvarchar(MAX)
,@isEnabled	bit
,@venueID int
,@latitude	decimal(10, 7)
,@longitude	decimal(10, 7)
,@phoneNumber	varchar(15)
,@venueType	char(1)
,@description nvarchar(max)

           AS

UPDATE Venue
   SET [venueName] = @venueName
      ,[addressLine1] = @addressLine1
      ,[addressLine2] = @addressLine2
      ,[city] = @city
      ,[region] = @region
      ,[postalCode] = @postalCode
      ,[countryISO] = @countryISO
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[venueURL] = @venueURL
      ,[isEnabled] = @isEnabled
      ,latitude = @latitude
	  ,longitude = @longitude
	  ,phoneNumber = @phoneNumber
	  ,venueType = @venueType
	  ,[description] = @description
 WHERE  venueID = @venueID
GO
/****** Object:  StoredProcedure [dbo].[up_UpdateVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE PROC [dbo].[up_UpdateVideo]

@videoKey varchar(150) 
,@providerKey varchar(50) 
,@providerUserKey varchar(50) 
,@providerCode char(2) 
,@updatedByUserID int 
,@isHidden bit 
,@isEnabled bit 
,@statusID int 
,@duration float 
,@intro float 
,@lengthFromStart float 
,@videoID int
,@volumeLevel	tinyint
,@enableTrim bit
,@publishDate datetime


           AS

UPDATE [Video]
   SET [videoKey] = @videoKey
      ,[providerKey] = @providerKey
      ,[providerUserKey] = @providerUserKey
      ,[providerCode] = @providerCode
      ,[updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[isHidden] = @isHidden
      ,[isEnabled] = @isEnabled
      ,[statusID] = @statusID
      ,[duration] = @duration
      ,[intro] = @intro
      ,[lengthFromStart] = @lengthFromStart
      ,volumeLevel = @volumeLevel
      ,enableTrim = @enableTrim
      ,publishDate = @publishDate
 WHERE videoID = @videoID



GO
/****** Object:  StoredProcedure [dbo].[up_UpdateVideoRequest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateVideoRequest]

 @videoRequestID int
,@updatedByUserID int
,@requestURL varchar(100)
,@statusType char(1)
,@videoKey varchar(20)
      
AS

UPDATE [VideoRequest]
   SET [updatedByUserID] = @updatedByUserID
      ,[updateDate] = GETUTCDATE()
      ,[requestURL] = @requestURL
      ,[statusType] = @statusType
      ,[videoKey] = @videoKey
 WHERE  videoRequestID = @videoRequestID


GO
/****** Object:  StoredProcedure [dbo].[up_UpdateWhoIsOnline]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 /*
//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
*/
CREATE proc [dbo].[up_UpdateWhoIsOnline]

AS

UPDATE [UserAccount]
   SET isOnline = 0
WHERE lastActivityDate < DATEADD(minute, -10, GETUTCDATE()) 

GO
/****** Object:  UserDefinedFunction [dbo].[DistanceBetween]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 

  CREATE FUNCTION [dbo].[DistanceBetween] (@Lat1 as real, 
                @Long1 as real, @Lat2 as real, @Long2 as real)
RETURNS real
AS
BEGIN

DECLARE @dLat1InRad as float(53);
SET @dLat1InRad = @Lat1 * (PI()/180.0);
DECLARE @dLong1InRad as float(53);
SET @dLong1InRad = @Long1 * (PI()/180.0);
DECLARE @dLat2InRad as float(53);
SET @dLat2InRad = @Lat2 * (PI()/180.0);
DECLARE @dLong2InRad as float(53);
SET @dLong2InRad = @Long2 * (PI()/180.0);

DECLARE @dLongitude as float(53);
SET @dLongitude = @dLong2InRad - @dLong1InRad;
DECLARE @dLatitude as float(53);
SET @dLatitude = @dLat2InRad - @dLat1InRad;
/* Intermediate result a. */
DECLARE @a as float(53);
SET @a = SQUARE (SIN (@dLatitude / 2.0)) + COS (@dLat1InRad) 
                 * COS (@dLat2InRad) 
                 * SQUARE(SIN (@dLongitude / 2.0));
/* Intermediate result c (great circle distance in Radians). */
DECLARE @c as real;
SET @c = 2.0 * ATN2 (SQRT (@a), SQRT (1.0 - @a));
DECLARE @kEarthRadius as real;
/* SET kEarthRadius = 3956.0 miles */
SET @kEarthRadius = 6376.5;        /* kms */

DECLARE @dDistance as real;
SET @dDistance = @kEarthRadius * @c;
return (@dDistance);
END
GO
/****** Object:  UserDefinedFunction [dbo].[fnFormatDate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnFormatDate] (@Datetime DATETIME, @FormatMask VARCHAR(32))
RETURNS VARCHAR(32)
AS
BEGIN
    DECLARE @StringDate VARCHAR(32)
    SET @StringDate = @FormatMask
    IF (CHARINDEX ('YYYY',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'YYYY',
                         DATENAME(YY, @Datetime))
    IF (CHARINDEX ('YY',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'YY',
                         RIGHT(DATENAME(YY, @Datetime),2))
    IF (CHARINDEX ('Month',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'Month',
                         DATENAME(MM, @Datetime))
    IF (CHARINDEX ('MON',@StringDate COLLATE SQL_Latin1_General_CP1_CS_AS)>0)
       SET @StringDate = REPLACE(@StringDate, 'MON',
                         LEFT(UPPER(DATENAME(MM, @Datetime)),3))
    IF (CHARINDEX ('Mon',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'Mon',
                                     LEFT(DATENAME(MM, @Datetime),3))
    IF (CHARINDEX ('MM',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'MM',
                  RIGHT('0'+CONVERT(VARCHAR,DATEPART(MM, @Datetime)),2))
    IF (CHARINDEX ('M',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'M',
                         CONVERT(VARCHAR,DATEPART(MM, @Datetime)))
    IF (CHARINDEX ('DD',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'DD',
                         RIGHT('0'+DATENAME(DD, @Datetime),2))
    IF (CHARINDEX ('D',@StringDate) > 0)
       SET @StringDate = REPLACE(@StringDate, 'D',
                                     DATENAME(DD, @Datetime))   
RETURN @StringDate
END

GO
/****** Object:  Table [dbo].[Acknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Acknowledgement](
	[acknowledgementID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[statusUpdateID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[acknowledgementType] [char](1) NOT NULL,
 CONSTRAINT [PK_Acknowledgement] PRIMARY KEY CLUSTERED 
(
	[acknowledgementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Artist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artist](
	[artistID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NOT NULL,
	[altName] [nvarchar](50) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[isHidden] [bit] NOT NULL,
 CONSTRAINT [PK_Artist] PRIMARY KEY CLUSTERED 
(
	[artistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArtistEvent]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArtistEvent](
	[artistID] [int] NOT NULL,
	[eventID] [int] NOT NULL,
	[rankOrder] [tinyint] NULL,
 CONSTRAINT [PK_ArtistTourDate] PRIMARY KEY CLUSTERED 
(
	[artistID] ASC,
	[eventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ArtistProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ArtistProperty](
	[artistPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[artistID] [int] NOT NULL,
	[propertyContent] [varchar](max) NULL,
	[propertyType] [char](2) NULL,
 CONSTRAINT [PK_ArtistProperty] PRIMARY KEY CLUSTERED 
(
	[artistPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_artistID_propertyType] UNIQUE NONCLUSTERED 
(
	[artistID] ASC,
	[propertyType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BlackIPID]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BlackIPID](
	[blackIPID] [int] IDENTITY(1,1) NOT NULL,
	[ipAddress] [varchar](255) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_BlackIPID] PRIMARY KEY CLUSTERED 
(
	[blackIPID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BlockedUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlockedUser](
	[blockedUserID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountIDBlocking] [int] NULL,
	[userAccountIDBlocked] [int] NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_BlockedUser] PRIMARY KEY CLUSTERED 
(
	[blockedUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Category]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[categoryID] [int] IDENTITY(1,1) NOT NULL,
	[categoryKey] [nvarchar](75) NOT NULL,
	[departmentID] [int] NOT NULL,
	[name] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[categoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChatRoom]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChatRoom](
	[chatRoomID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [varchar](25) NULL,
	[chatMessage] [nvarchar](max) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[ipAddress] [varchar](50) NULL,
	[roomID] [int] NULL,
 CONSTRAINT [PK_ChatRoom] PRIMARY KEY CLUSTERED 
(
	[chatRoomID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChatRoomUser]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChatRoomUser](
	[chatRoomUserID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[ipAddress] [varchar](50) NULL,
	[roomID] [int] NULL,
	[connectionCode] [varchar](50) NULL,
 CONSTRAINT [PK_chatRoomUser] PRIMARY KEY CLUSTERED 
(
	[chatRoomUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_chatRoomUser_createdByUserID] UNIQUE NONCLUSTERED 
(
	[createdByUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ClickAudit]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ClickAudit](
	[clickAuditID] [int] NULL,
	[clickLogID] [int] NOT NULL,
	[ipAddress] [varchar](25) NULL,
	[clickType] [char](1) NULL,
	[referringURL] [varchar](255) NULL,
	[currentURL] [varchar](255) NULL,
	[productID] [int] NULL,
	[createdByUserID] [int] NULL,
	[createDate] [smalldatetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ClickLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ClickLog](
	[clickLogID] [int] IDENTITY(1,1) NOT NULL,
	[ipAddress] [varchar](25) NULL,
	[clickType] [char](1) NULL,
	[referringURL] [varchar](255) NULL,
	[currentURL] [varchar](255) NULL,
	[productID] [int] NULL,
	[createdByUserID] [int] NULL,
	[createDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_ClickLog] PRIMARY KEY CLUSTERED 
(
	[clickLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Color]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Color](
	[colorID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](25) NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[siteDomainID] [int] NULL,
 CONSTRAINT [PK_Color] PRIMARY KEY CLUSTERED 
(
	[colorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Content]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Content](
	[contentID] [int] IDENTITY(1,1) NOT NULL,
	[siteDomainID] [int] NULL,
	[contentKey] [varchar](150) NULL,
	[title] [varchar](150) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[detail] [varchar](max) NULL,
	[metaDescription] [varchar](500) NULL,
	[metaKeywords] [varchar](500) NULL,
	[contentTypeID] [int] NULL,
	[releaseDate] [datetime] NULL,
	[rating] [float] NULL,
	[contentPhotoURL] [varchar](150) NULL,
	[contentPhotoThumbURL] [varchar](150) NULL,
	[contentVideoURL] [varchar](150) NULL,
	[outboundURL] [varchar](max) NULL,
	[isEnabled] [bit] NOT NULL,
	[currentStatus] [char](1) NULL,
	[language] [char](2) NOT NULL,
 CONSTRAINT [PK_Content] PRIMARY KEY CLUSTERED 
(
	[contentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContentComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentComment](
	[contentCommentID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[statusType] [char](1) NOT NULL,
	[detail] [nvarchar](max) NULL,
	[contentID] [int] NULL,
	[fromName] [varchar](50) NULL,
	[fromEmail] [varchar](50) NULL,
	[ipAddress] [varchar](50) NULL,
 CONSTRAINT [PK_ContentComment] PRIMARY KEY CLUSTERED 
(
	[contentCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContentType]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentType](
	[contentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[contentName] [varchar](50) NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[contentCode] [char](5) NULL,
 CONSTRAINT [PK_ContentType] PRIMARY KEY CLUSTERED 
(
	[contentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [IX_content_Code] UNIQUE NONCLUSTERED 
(
	[contentCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contest](
	[contestID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NULL,
	[deadLine] [datetime] NOT NULL,
	[description] [varchar](max) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[beginDate] [datetime] NULL,
	[contestKey] [varchar](100) NULL,
 CONSTRAINT [PK_Contest] PRIMARY KEY CLUSTERED 
(
	[contestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContestVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContestVideo](
	[contestVideoID] [int] IDENTITY(1,1) NOT NULL,
	[videoID] [int] NOT NULL,
	[contestID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[contestRank] [tinyint] NULL,
	[subContest] [char](1) NULL,
 CONSTRAINT [PK_ContestVideo] PRIMARY KEY CLUSTERED 
(
	[contestVideoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContestVideoVote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ContestVideoVote](
	[contestVideoVoteID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NULL,
	[contestVideoID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_ContestVideoVote] PRIMARY KEY CLUSTERED 
(
	[contestVideoVoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DirectMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DirectMessage](
	[directMessageID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[fromUserAccountID] [int] NOT NULL,
	[toUserAccountID] [int] NOT NULL,
	[isRead] [bit] NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[isEnabled] [bit] NOT NULL,
 CONSTRAINT [PK_DirectMessage] PRIMARY KEY CLUSTERED 
(
	[directMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ErrorLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorLog](
	[errorLogID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[message] [varchar](max) NULL,
	[url] [varchar](255) NULL,
	[responseCode] [int] NULL,
 CONSTRAINT [PK_ErrorLog] PRIMARY KEY CLUSTERED 
(
	[errorLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Event]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Event](
	[eventID] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](50) NULL,
	[venueID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[localTimeBegin] [datetime] NULL,
	[notes] [nvarchar](max) NULL,
	[ticketURL] [nvarchar](max) NULL,
	[localTimeEnd] [datetime] NULL,
	[eventCycleID] [int] NOT NULL,
	[rsvpURL] [nvarchar](max) NULL,
	[isReoccuring] [bit] NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[eventDetailURL] [nvarchar](max) NULL,
 CONSTRAINT [PK_TourDate] PRIMARY KEY CLUSTERED 
(
	[eventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventCycle]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventCycle](
	[eventCycleID] [int] IDENTITY(1,1) NOT NULL,
	[cycleName] [varchar](50) NULL,
	[eventCode] [char](3) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_EventCycle] PRIMARY KEY CLUSTERED 
(
	[eventCycleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GeoData]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GeoData](
	[countrycode] [varchar](3) NULL,
	[postalcode] [varchar](10) NULL,
	[placename] [varchar](180) NULL,
	[state] [varchar](100) NULL,
	[county] [varchar](100) NULL,
	[community] [varchar](100) NULL,
	[latitude] [varchar](25) NULL,
	[longitude] [varchar](25) NULL,
	[accuracy] [varchar](5) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HostedVideoLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HostedVideoLog](
	[videoLogID] [int] IDENTITY(1,1) NOT NULL,
	[createDate] [datetime] NULL,
	[viewURL] [varchar](255) NOT NULL,
	[ipAddress] [varchar](25) NULL,
	[secondsElapsed] [int] NULL,
	[videoType] [char](2) NULL,
 CONSTRAINT [PK_HostedVideoLogID] PRIMARY KEY CLUSTERED 
(
	[videoLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InterestedIn]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[InterestedIn](
	[interestedInID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[typeLetter] [char](1) NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_InterestedIn] PRIMARY KEY CLUSTERED 
(
	[interestedInID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Language]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Language](
	[languageID] [int] IDENTITY(1,1) NOT NULL,
	[languageType] [char](5) NOT NULL,
	[languageName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Language] PRIMARY KEY CLUSTERED 
(
	[languageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LanguageUserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LanguageUserAccount](
	[languageID] [int] NOT NULL,
	[userAccountID] [int] NOT NULL,
 CONSTRAINT [PK_LanguageUserAccount] PRIMARY KEY CLUSTERED 
(
	[languageID] ASC,
	[userAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[MultiProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MultiProperty](
	[multiPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[propertyTypeID] [int] NOT NULL,
	[name] [varchar](50) NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[propertyContent] [varchar](max) NULL,
 CONSTRAINT [PK_MultiProperty] PRIMARY KEY CLUSTERED 
(
	[multiPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MultiPropertyVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MultiPropertyVideo](
	[multiPropertyID] [int] NOT NULL,
	[videoID] [int] NOT NULL,
 CONSTRAINT [PK_MultiPropertyVideo] PRIMARY KEY CLUSTERED 
(
	[multiPropertyID] ASC,
	[videoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PhotoItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoItem](
	[photoItemID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[title] [varchar](100) NULL,
	[filePathRaw] [varchar](255) NOT NULL,
	[filePathThumb] [varchar](255) NOT NULL,
	[filePathStandard] [varchar](255) NOT NULL,
 CONSTRAINT [PK_PhotoItem] PRIMARY KEY CLUSTERED 
(
	[photoItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Playlist]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Playlist](
	[playlistID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[playlistBegin] [datetime] NULL,
	[playListName] [varchar](50) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[autoPlay] [bit] NOT NULL,
 CONSTRAINT [PK_Playlist] PRIMARY KEY CLUSTERED 
(
	[playlistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PlaylistVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlaylistVideo](
	[playlistVideoID] [int] IDENTITY(1,1) NOT NULL,
	[playlistID] [int] NOT NULL,
	[videoID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[rankOrder] [smallint] NULL,
 CONSTRAINT [PK_PlaylistVideo] PRIMARY KEY CLUSTERED 
(
	[playlistVideoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ProfileLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileLog](
	[profileLogID] [int] IDENTITY(1,1) NOT NULL,
	[lookingUserAccountID] [int] NOT NULL,
	[lookedAtUserAccountID] [int] NULL,
	[createDate] [smalldatetime] NOT NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_ProfileLog] PRIMARY KEY CLUSTERED 
(
	[profileLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[PropertyType]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PropertyType](
	[propertyTypeID] [int] IDENTITY(1,1) NOT NULL,
	[propertyTypeCode] [char](5) NOT NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[propertyTypeName] [varchar](25) NULL,
 CONSTRAINT [PK_MultiPropertyType] PRIMARY KEY CLUSTERED 
(
	[propertyTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RelationshipStatus]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RelationshipStatus](
	[relationshipStatusID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[typeLetter] [char](1) NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_RelationshipStatus] PRIMARY KEY CLUSTERED 
(
	[relationshipStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Role]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Role](
	[roleID] [int] IDENTITY(1,1) NOT NULL,
	[roleName] [varchar](50) NULL,
	[createDate] [datetime] NULL,
	[updatedDate] [datetime] NULL,
	[createdByEndUserID] [int] NULL,
	[updatedByEndUserID] [int] NULL,
	[description] [varchar](255) NULL,
 CONSTRAINT [PK_Role] PRIMARY KEY CLUSTERED 
(
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RSSItem]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RSSItem](
	[rssItemID] [int] IDENTITY(1,1) NOT NULL,
	[rssResourceID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[authorName] [nvarchar](max) NULL,
	[commentsURL] [nvarchar](max) NULL,
	[description] [nvarchar](max) NULL,
	[pubDate] [datetime] NULL,
	[title] [nvarchar](max) NOT NULL,
	[languageName] [varchar](5) NULL,
	[artistID] [int] NULL,
	[link] [nvarchar](max) NULL,
	[guidLink] [nvarchar](max) NULL,
 CONSTRAINT [PK_RSSItem] PRIMARY KEY CLUSTERED 
(
	[rssItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[RssResource]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RssResource](
	[rssResourceID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[rssResourceURL] [varchar](400) NULL,
	[resourceName] [varchar](150) NULL,
	[providerKey] [char](2) NULL,
	[isEnabled] [bit] NOT NULL,
	[artistID] [int] NULL,
 CONSTRAINT [PK_RssResource] PRIMARY KEY CLUSTERED 
(
	[rssResourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SiteComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SiteComment](
	[siteCommentID] [int] IDENTITY(1,1) NOT NULL,
	[detail] [nvarchar](max) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
 CONSTRAINT [PK_SiteComment] PRIMARY KEY CLUSTERED 
(
	[siteCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SiteDomain]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SiteDomain](
	[siteDomainID] [int] IDENTITY(1,1) NOT NULL,
	[propertyType] [char](5) NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[language] [char](2) NULL,
	[description] [nvarchar](max) NULL,
 CONSTRAINT [PK_SiteDomain] PRIMARY KEY CLUSTERED 
(
	[siteDomainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Size]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Size](
	[sizeID] [int] IDENTITY(1,1) NOT NULL,
	[sizeName] [nvarchar](50) NULL,
	[sizeTypeID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[rankOrder] [tinyint] NULL,
 CONSTRAINT [PK_Size] PRIMARY KEY CLUSTERED 
(
	[sizeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SizeType]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SizeType](
	[sizeTypeID] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
 CONSTRAINT [PK_SizeType] PRIMARY KEY CLUSTERED 
(
	[sizeTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Song]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Song](
	[songID] [int] IDENTITY(1,1) NOT NULL,
	[artistID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[isHidden] [bit] NOT NULL,
	[name] [nvarchar](150) NULL,
	[songKey] [nvarchar](150) NULL,
 CONSTRAINT [PK_Song] PRIMARY KEY CLUSTERED 
(
	[songID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SongProperty]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SongProperty](
	[songPropertyID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[songID] [int] NOT NULL,
	[propertyContent] [varchar](max) NULL,
	[propertyType] [char](2) NULL,
 CONSTRAINT [PK_SongProperty] PRIMARY KEY CLUSTERED 
(
	[songPropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Status]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[statusID] [int] IDENTITY(1,1) NOT NULL,
	[statusDescription] [varchar](250) NULL,
	[statusCode] [char](5) NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[statusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StatusComment]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StatusComment](
	[statusCommentID] [int] IDENTITY(1,1) NOT NULL,
	[statusUpdateID] [int] NOT NULL,
	[userAccountID] [int] NOT NULL,
	[statusType] [char](1) NOT NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[message] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_StatusComment] PRIMARY KEY CLUSTERED 
(
	[statusCommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StatusCommentAcknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StatusCommentAcknowledgement](
	[statusCommentAcknowledgementID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[statusCommentID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[acknowledgementType] [char](1) NOT NULL,
 CONSTRAINT [PK_StatusCommentAcknowledgement] PRIMARY KEY CLUSTERED 
(
	[statusCommentAcknowledgementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StatusUpdate]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StatusUpdate](
	[statusUpdateID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[userAccountID] [int] NOT NULL,
	[message] [nvarchar](max) NOT NULL,
	[statusType] [char](1) NOT NULL,
	[photoItemID] [int] NULL,
	[zoneID] [int] NULL,
	[isMobile] [bit] NOT NULL,
 CONSTRAINT [PK_StatusUpdate] PRIMARY KEY CLUSTERED 
(
	[statusUpdateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[StatusUpdateNotification]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StatusUpdateNotification](
	[statusUpdateNotificationID] [int] IDENTITY(1,1) NOT NULL,
	[statusUpdateID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[isRead] [bit] NOT NULL,
	[userAccountID] [int] NULL,
	[responseType] [char](1) NOT NULL,
 CONSTRAINT [PK_StatusUpdateNotification] PRIMARY KEY CLUSTERED 
(
	[statusUpdateNotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccount]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccount](
	[userAccountID] [int] IDENTITY(1,1) NOT NULL,
	[userName] [nvarchar](50) NOT NULL,
	[password] [varchar](75) NULL,
	[passwordFormat] [varchar](50) NULL,
	[passwordSalt] [varchar](75) NULL,
	[eMail] [nvarchar](75) NOT NULL,
	[passwordQuestion] [varchar](100) NULL,
	[passwordAnswer] [varchar](100) NULL,
	[isApproved] [bit] NULL,
	[lastLoginDate] [datetime] NULL,
	[lastPasswordChangeDate] [datetime] NULL,
	[lastLockoutDate] [datetime] NULL,
	[failedPasswordAttemptCount] [smallint] NULL,
	[failedPasswordAttemptWindowStart] [datetime] NULL,
	[failedPasswordAnswerAttemptCount] [smallint] NULL,
	[failedPasswordAnswerAttemptWindowStart] [datetime] NULL,
	[comment] [varchar](50) NULL,
	[createDate] [datetime] NULL,
	[updateDate] [datetime] NULL,
	[updatedByUserAccountID] [int] NULL,
	[createdByUserAccountID] [int] NULL,
	[isOnline] [bit] NULL,
	[isLockedOut] [bit] NULL,
	[lastActivityDate] [datetime] NULL,
	[ipAddress] [varchar](25) NULL,
 CONSTRAINT [PK_UserAccount] PRIMARY KEY CLUSTERED 
(
	[userAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccountDetail]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccountDetail](
	[userAccountDetailID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[youAreID] [int] NULL,
	[relationshipStatusID] [int] NULL,
	[interestedInID] [int] NULL,
	[createDate] [datetime] NULL,
	[updateDate] [datetime] NULL,
	[updatedByUserID] [int] NULL,
	[createdByUserID] [int] NULL,
	[country] [char](2) NULL,
	[region] [varchar](25) NULL,
	[city] [varchar](25) NULL,
	[postalCode] [varchar](15) NULL,
	[profilePicURL] [varchar](75) NULL,
	[birthDate] [date] NULL,
	[religion] [char](1) NULL,
	[profileThumbPicURL] [varchar](75) NULL,
	[ethnicity] [char](1) NULL,
	[heightCM] [float] NULL,
	[weightKG] [float] NULL,
	[diet] [char](1) NULL,
	[accountViews] [int] NULL,
	[externalURL] [nvarchar](100) NULL,
	[smokes] [char](1) NULL,
	[drinks] [char](1) NULL,
	[handed] [char](1) NULL,
	[displayAge] [bit] NOT NULL,
	[enableProfileLogging] [bit] NOT NULL,
	[lastPhotoUpdate] [datetime] NULL,
	[emailMessages] [bit] NOT NULL,
	[showOnMap] [bit] NOT NULL,
	[referringUserID] [int] NULL,
	[browerType] [varchar](15) NULL,
	[membersOnlyProfile] [bit] NOT NULL,
	[messangerType] [char](2) NULL,
	[messangerName] [varchar](25) NULL,
	[aboutDesc] [nvarchar](max) NULL,
	[bandsSeen] [nvarchar](max) NULL,
	[bandsToSee] [nvarchar](max) NULL,
	[hardwareSoftware] [nvarchar](max) NULL,
	[firstName] [nvarchar](20) NULL,
	[lastName] [nvarchar](20) NULL,
	[defaultLanguage] [char](2) NOT NULL,
	[latitude] [decimal](9, 6) NULL,
	[longitude] [decimal](9, 6) NULL,
 CONSTRAINT [PK_UserAccountDetail] PRIMARY KEY CLUSTERED 
(
	[userAccountDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAccountMet]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountMet](
	[userAccountRequester] [int] NOT NULL,
	[userAccounted] [int] NOT NULL,
	[haveMet] [bit] NOT NULL,
 CONSTRAINT [PK_UserAccountMet] PRIMARY KEY CLUSTERED 
(
	[userAccountRequester] ASC,
	[userAccounted] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccountRole]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccountRole](
	[userAccountID] [int] NOT NULL,
	[roleID] [int] NOT NULL,
 CONSTRAINT [PK_UserAccountRole] PRIMARY KEY CLUSTERED 
(
	[userAccountID] ASC,
	[roleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserAccountVideo]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAccountVideo](
	[videoID] [int] NOT NULL,
	[userAccountID] [int] NOT NULL,
	[createDate] [datetime] NOT NULL,
	[videoType] [char](1) NOT NULL,
 CONSTRAINT [PK_UserAccountVideo_1] PRIMARY KEY CLUSTERED 
(
	[videoID] ASC,
	[userAccountID] ASC,
	[videoType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAddress](
	[userAddressID] [int] IDENTITY(1,1) NOT NULL,
	[firstName] [nvarchar](50) NULL,
	[middleName] [nvarchar](50) NULL,
	[lastName] [nvarchar](50) NULL,
	[addressLine1] [nvarchar](75) NULL,
	[addressLine2] [nvarchar](50) NULL,
	[addressLine3] [nvarchar](50) NULL,
	[city] [nvarchar](50) NULL,
	[region] [nvarchar](50) NULL,
	[postalCode] [nvarchar](50) NULL,
	[countryISO] [char](2) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[userAccountID] [int] NULL,
	[addressStatus] [char](1) NULL,
	[choice1] [varchar](max) NULL,
	[choice2] [varchar](max) NULL,
 CONSTRAINT [PK_UserAddress] PRIMARY KEY CLUSTERED 
(
	[userAddressID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserConnection]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserConnection](
	[userConnectionID] [int] IDENTITY(1,1) NOT NULL,
	[fromUserAccountID] [int] NOT NULL,
	[toUserAccountID] [int] NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[statusType] [char](1) NULL,
	[isConfirmed] [bit] NOT NULL,
 CONSTRAINT [PK_UserConnection] PRIMARY KEY CLUSTERED 
(
	[userConnectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserPhoto]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserPhoto](
	[userPhotoID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[updatedByUserID] [int] NULL,
	[createdByUserID] [int] NULL,
	[picURL] [varchar](75) NULL,
	[thumbPicURL] [varchar](75) NULL,
	[description] [varchar](255) NULL,
	[rankOrder] [tinyint] NULL,
 CONSTRAINT [PK_UserPhoto] PRIMARY KEY CLUSTERED 
(
	[userPhotoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Venue]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Venue](
	[venueID] [int] IDENTITY(1,1) NOT NULL,
	[venueName] [nvarchar](50) NULL,
	[addressLine1] [nvarchar](50) NULL,
	[addressLine2] [nvarchar](25) NULL,
	[city] [nvarchar](25) NULL,
	[region] [nvarchar](20) NULL,
	[postalCode] [nvarchar](15) NULL,
	[countryISO] [char](2) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[venueURL] [nvarchar](max) NULL,
	[isEnabled] [bit] NOT NULL,
	[latitude] [decimal](10, 7) NULL,
	[longitude] [decimal](10, 7) NULL,
	[phoneNumber] [varchar](15) NULL,
	[venueType] [char](1) NULL,
	[description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Venue] PRIMARY KEY CLUSTERED 
(
	[venueID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Video]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Video](
	[videoID] [int] IDENTITY(1,1) NOT NULL,
	[videoKey] [varchar](150) NULL,
	[providerKey] [varchar](50) NULL,
	[providerUserKey] [varchar](50) NULL,
	[providerCode] [char](2) NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[isHidden] [bit] NOT NULL,
	[isEnabled] [bit] NOT NULL,
	[statusID] [int] NULL,
	[duration] [float] NULL,
	[intro] [float] NULL,
	[lengthFromStart] [float] NULL,
	[volumeLevel] [tinyint] NULL,
	[enableTrim] [bit] NOT NULL,
	[publishDate] [datetime] NULL,
 CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[videoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoLog]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoLog](
	[videoLogID] [int] IDENTITY(1,1) NOT NULL,
	[createDate] [datetime] NULL,
	[videoID] [int] NOT NULL,
	[ipAddress] [varchar](25) NULL,
 CONSTRAINT [PK_VideoLogID] PRIMARY KEY CLUSTERED 
(
	[videoLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoRequest]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoRequest](
	[videoRequestID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[requestURL] [varchar](100) NULL,
	[statusType] [char](1) NULL,
	[videoKey] [varchar](20) NULL,
 CONSTRAINT [PK_VideoRequest] PRIMARY KEY CLUSTERED 
(
	[videoRequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoSong]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VideoSong](
	[videoID] [int] NOT NULL,
	[songID] [int] NOT NULL,
	[rankOrder] [tinyint] NULL,
 CONSTRAINT [PK_VideoSong] PRIMARY KEY CLUSTERED 
(
	[videoID] ASC,
	[songID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[VideoVote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoVote](
	[videoVoteID] [int] IDENTITY(1,1) NOT NULL,
	[ipAddress] [varchar](50) NULL,
	[createDate] [datetime] NULL,
	[singlePick1] [varchar](50) NULL,
	[singlePick2] [varchar](50) NULL,
	[singlePick3] [varchar](50) NULL,
	[groupPick1] [varchar](50) NULL,
	[groupPick2] [varchar](50) NULL,
	[groupPick3] [varchar](50) NULL,
 CONSTRAINT [PK_VideoVote] PRIMARY KEY CLUSTERED 
(
	[videoVoteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Vote]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vote](
	[voteID] [int] IDENTITY(1,1) NOT NULL,
	[userAccountID] [int] NOT NULL,
	[createDate] [datetime] NOT NULL,
	[videoID] [int] NOT NULL,
	[score] [int] NOT NULL,
 CONSTRAINT [PK_Vote] PRIMARY KEY CLUSTERED 
(
	[voteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WallMessage]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WallMessage](
	[wallMessageID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[message] [varchar](max) NOT NULL,
	[isRead] [bit] NOT NULL,
	[fromUserAccountID] [int] NOT NULL,
	[toUserAccountID] [int] NOT NULL,
 CONSTRAINT [PK_WallMessage] PRIMARY KEY CLUSTERED 
(
	[wallMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WishList]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WishList](
	[productID] [int] NOT NULL,
	[createdByUserID] [int] NOT NULL,
	[createDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_WishList] PRIMARY KEY CLUSTERED 
(
	[productID] ASC,
	[createdByUserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[YouAre]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[YouAre](
	[youAreID] [int] IDENTITY(1,1) NOT NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[createdByUserID] [int] NULL,
	[typeLetter] [char](1) NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_YouAre] PRIMARY KEY CLUSTERED 
(
	[youAreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Zone]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Zone](
	[zoneID] [int] IDENTITY(1,1) NOT NULL,
	[createdByUserID] [int] NULL,
	[updatedByUserID] [int] NULL,
	[createDate] [datetime] NOT NULL,
	[updateDate] [datetime] NULL,
	[name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Zone] PRIMARY KEY CLUSTERED 
(
	[zoneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Split] (@s varchar(512), @sep char(1))
RETURNS table
AS
RETURN (
    WITH Pieces(pn, start, stop) AS (
      SELECT 1, 1, CHARINDEX(@sep, @s)
      UNION ALL
      SELECT pn + 1, stop + 1, CHARINDEX(@sep, @s, stop + 1)
      FROM Pieces
      WHERE stop > 0
    )
    SELECT pn AS position,
      SUBSTRING(@s, start, CASE WHEN stop > 0 THEN stop-start ELSE 512 END) AS part
    FROM Pieces
  )
GO 
/****** Object:  View [dbo].[vwUserSearchFilter]    Script Date: 1/20/2013 4:00:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[vwUserSearchFilter]


as

SELECT DISTINCT  ua.[userAccountID]
      ,uad.[youAreID]
      ,uad.[relationshipStatusID]
      ,uad.[interestedInID]
	  ,uad.country
      ,latitude
      ,longitude
      ,uad.[birthDate]
      ,uad.[defaultLanguage]
      ,ua.[isOnline]
      ,ua.[lastActivityDate]
	  ,uad.showOnMap 
	
  FROM [UserAccount] ua INNER JOIN UserAccountDetail uad ON ua.userAccountID = uad.userAccountID
 WHERE uad.displayAge = 1



GO
/****** Object:  Index [IX_Acknowledgement]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Acknowledgement] ON [dbo].[Acknowledgement]
(
	[statusUpdateID] ASC,
	[userAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_altName]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_altName] ON [dbo].[Artist]
(
	[altName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_name]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_name] ON [dbo].[Artist]
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_userAccountIDBlocked_userAccountIDBlocking]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_userAccountIDBlocked_userAccountIDBlocking] ON [dbo].[BlockedUser]
(
	[userAccountIDBlocked] ASC,
	[userAccountIDBlocking] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_categoryKey_departmentID]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_categoryKey_departmentID] ON [dbo].[Category]
(
	[categoryKey] ASC,
	[departmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_ChatRoom_createDate]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ChatRoom_createDate] ON [dbo].[ChatRoom]
(
	[createDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ipAddress]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_ipAddress] ON [dbo].[ClickLog]
(
	[ipAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Content_title]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Content_title] ON [dbo].[Content]
(
	[title] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_contentKey]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_contentKey] ON [dbo].[Content]
(
	[contentKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_ContentType_contentName]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ContentType_contentName] ON [dbo].[ContentType]
(
	[contentName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_videoID_contestID]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_videoID_contestID] ON [dbo].[ContestVideo]
(
	[videoID] ASC,
	[contestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_directMessage_toUserAccountID_createDate]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_directMessage_toUserAccountID_createDate] ON [dbo].[DirectMessage]
(
	[toUserAccountID] ASC,
	[createDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_directMessage_toUserAccountID_isRead]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_directMessage_toUserAccountID_isRead] ON [dbo].[DirectMessage]
(
	[toUserAccountID] ASC,
	[isRead] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_countrycode_postalcode]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_countrycode_postalcode] ON [dbo].[GeoData]
(
	[countrycode] ASC,
	[postalcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_playlistID_videoID]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_playlistID_videoID] ON [dbo].[PlaylistVideo]
(
	[playlistID] ASC,
	[videoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_lookedAtUserAccountID_createDate]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_lookedAtUserAccountID_createDate] ON [dbo].[ProfileLog]
(
	[lookedAtUserAccountID] ASC,
	[createDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_MultiPropertyType]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_MultiPropertyType] ON [dbo].[PropertyType]
(
	[propertyTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_rssResourceID_pubDate]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_rssResourceID_pubDate] ON [dbo].[RSSItem]
(
	[rssResourceID] ASC,
	[pubDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_rssResourceURL]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_rssResourceURL] ON [dbo].[RssResource]
(
	[rssResourceURL] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_propertyType_language]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_propertyType_language] ON [dbo].[SiteDomain]
(
	[language] ASC,
	[propertyType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_artistID_name]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_artistID_name] ON [dbo].[Song]
(
	[artistID] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_songID_propertyType]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_songID_propertyType] ON [dbo].[SongProperty]
(
	[songID] ASC,
	[propertyType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_status_update_create]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_status_update_create] ON [dbo].[StatusUpdate]
(
	[createDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_StatusUpdateNotification]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_StatusUpdateNotification] ON [dbo].[StatusUpdateNotification]
(
	[responseType] ASC,
	[statusUpdateID] ASC,
	[userAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [eMail]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [eMail] ON [dbo].[UserAccount]
(
	[eMail] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [userName]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [userName] ON [dbo].[UserAccount]
(
	[userName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_userAccountID_createDate]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_userAccountID_createDate] ON [dbo].[UserAccountVideo]
(
	[userAccountID] ASC,
	[createDate] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_fromUserAccountID_toUserAccountID]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_fromUserAccountID_toUserAccountID] ON [dbo].[UserConnection]
(
	[fromUserAccountID] ASC,
	[toUserAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_toUserAccountID_fromUserAccountID]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_toUserAccountID_fromUserAccountID] ON [dbo].[UserConnection]
(
	[toUserAccountID] ASC,
	[fromUserAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_isEnabled_isHidden_providerCode]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_isEnabled_isHidden_providerCode] ON [dbo].[Video]
(
	[isEnabled] ASC,
	[isHidden] ASC,
	[providerCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_providerKey_videoKey]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_providerKey_videoKey] ON [dbo].[Video]
(
	[videoKey] ASC,
	[providerKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vote]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_Vote] ON [dbo].[Vote]
(
	[userAccountID] ASC,
	[videoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_read_wall_messages]    Script Date: 1/20/2013 4:00:03 PM ******/
CREATE NONCLUSTERED INDEX [IX_read_wall_messages] ON [dbo].[WallMessage]
(
	[isRead] ASC,
	[toUserAccountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Acknowledgement] ADD  CONSTRAINT [DF_Acknowledgement_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Artist] ADD  CONSTRAINT [DF_Artist_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Artist] ADD  CONSTRAINT [DF_Artist_isHidden]  DEFAULT ((0)) FOR [isHidden]
GO
ALTER TABLE [dbo].[ArtistProperty] ADD  CONSTRAINT [DF_ArtistProperty_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[BlackIPID] ADD  CONSTRAINT [DF_BlackIPID_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[BlockedUser] ADD  CONSTRAINT [DF_BlockedUser_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [DF_Category_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ChatRoom] ADD  CONSTRAINT [DF_ChatRoom_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ChatRoomUser] ADD  CONSTRAINT [DF_chatRoomUser_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ClickAudit] ADD  CONSTRAINT [DF_ClickAudit_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ClickLog] ADD  CONSTRAINT [DF_ClickLog_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Color] ADD  CONSTRAINT [DF_Color_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[Content] ADD  CONSTRAINT [DF_Content_language]  DEFAULT ('en') FOR [language]
GO
ALTER TABLE [dbo].[ContentComment] ADD  CONSTRAINT [DF_ContentComment_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ContentType] ADD  CONSTRAINT [DF_ContentType\_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Contest] ADD  CONSTRAINT [DF_Contest_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ContestVideo] ADD  CONSTRAINT [DF_ContestVideo_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ContestVideoVote] ADD  CONSTRAINT [DF_ContestVideoVote_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[DirectMessage] ADD  CONSTRAINT [DF_DirectMessage_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[DirectMessage] ADD  CONSTRAINT [DF_DirectMessage_isRead]  DEFAULT ((0)) FOR [isRead]
GO
ALTER TABLE [dbo].[DirectMessage] ADD  CONSTRAINT [DF_DirectMessage_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[ErrorLog] ADD  CONSTRAINT [DF_ErrorLog_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_TourDate_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_TourDate_reoccurs]  DEFAULT ((0)) FOR [isReoccuring]
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[EventCycle] ADD  CONSTRAINT [DF_EventCycle_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[InterestedIn] ADD  CONSTRAINT [DF_InterestedIn_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[MultiProperty] ADD  CONSTRAINT [DF_MultiProperty_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[PhotoItem] ADD  CONSTRAINT [DF_PhotoItem_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Playlist] ADD  CONSTRAINT [DF_Playlist_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Playlist] ADD  CONSTRAINT [DF_Playlist_autoPlay]  DEFAULT ((1)) FOR [autoPlay]
GO
ALTER TABLE [dbo].[PlaylistVideo] ADD  CONSTRAINT [DF_PlaylistVideo_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[ProfileLog] ADD  CONSTRAINT [DF_ProfileLog_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[PropertyType] ADD  CONSTRAINT [DF_MultiPropertyType_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[RelationshipStatus] ADD  CONSTRAINT [DF_RelationshipStatus_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[RSSItem] ADD  CONSTRAINT [DF_RSSItem_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[RssResource] ADD  CONSTRAINT [DF_RssResource_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[RssResource] ADD  CONSTRAINT [DF_RssResource_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[SiteComment] ADD  CONSTRAINT [DF_SiteComment_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[SiteDomain] ADD  CONSTRAINT [DF_SiteDomain_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Size] ADD  CONSTRAINT [DF_Size_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[SizeType] ADD  CONSTRAINT [DF_SizeType_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Song] ADD  CONSTRAINT [DF_Song_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Song] ADD  CONSTRAINT [DF_Song_isHidden]  DEFAULT ((0)) FOR [isHidden]
GO
ALTER TABLE [dbo].[SongProperty] ADD  CONSTRAINT [DF_SongProperty_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[StatusComment] ADD  CONSTRAINT [DF_StatusComment_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[StatusCommentAcknowledgement] ADD  CONSTRAINT [DF_StatusCommentAcknowledgement_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[StatusUpdate] ADD  CONSTRAINT [DF_StatusUpdate_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[StatusUpdate] ADD  CONSTRAINT [DF_StatusUpdate_isMobile]  DEFAULT ((0)) FOR [isMobile]
GO
ALTER TABLE [dbo].[StatusUpdateNotification] ADD  CONSTRAINT [DF_StatusUpdateNotifcation_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[UserAccount] ADD  CONSTRAINT [DF_UserAccount_isApproved]  DEFAULT ((0)) FOR [isApproved]
GO
ALTER TABLE [dbo].[UserAccount] ADD  CONSTRAINT [DF_UserAccount_isOnline]  DEFAULT ((0)) FOR [isOnline]
GO
ALTER TABLE [dbo].[UserAccount] ADD  CONSTRAINT [DF_UserAccount_isLockedOut]  DEFAULT ((0)) FOR [isLockedOut]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_displayAge]  DEFAULT ((1)) FOR [displayAge]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_enableProfileLogging]  DEFAULT ((1)) FOR [enableProfileLogging]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_emailMessages]  DEFAULT ((1)) FOR [emailMessages]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_showOnMap]  DEFAULT ((1)) FOR [showOnMap]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_membersOnlyProfile]  DEFAULT ((0)) FOR [membersOnlyProfile]
GO
ALTER TABLE [dbo].[UserAccountDetail] ADD  CONSTRAINT [DF_UserAccountDetail_defaultLanguage]  DEFAULT ('EN') FOR [defaultLanguage]
GO
ALTER TABLE [dbo].[UserAccountMet] ADD  CONSTRAINT [DF_UserAccountMet_haveMet]  DEFAULT ((0)) FOR [haveMet]
GO
ALTER TABLE [dbo].[UserAccountVideo] ADD  CONSTRAINT [DF_UserAccountVideo_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[UserAccountVideo] ADD  CONSTRAINT [DF_UserAccountVideo_videoType]  DEFAULT ('F') FOR [videoType]
GO
ALTER TABLE [dbo].[UserAddress] ADD  CONSTRAINT [DF_UserAddress_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[UserConnection] ADD  CONSTRAINT [DF_UserConnection_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[UserConnection] ADD  CONSTRAINT [DF_UserConnection_isConfirmed]  DEFAULT ((0)) FOR [isConfirmed]
GO
ALTER TABLE [dbo].[UserPhoto] ADD  CONSTRAINT [DF_UserPhoto_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Venue] ADD  CONSTRAINT [DF_Venue_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Venue] ADD  CONSTRAINT [DF_Venue_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_isHidden]  DEFAULT ((0)) FOR [isHidden]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_isEnabled]  DEFAULT ((1)) FOR [isEnabled]
GO
ALTER TABLE [dbo].[Video] ADD  CONSTRAINT [DF_Video_enableTrim]  DEFAULT ((0)) FOR [enableTrim]
GO
ALTER TABLE [dbo].[VideoRequest] ADD  CONSTRAINT [DF_VideoRequest_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[WallMessage] ADD  CONSTRAINT [DF_WallMessage_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[WallMessage] ADD  CONSTRAINT [DF_WallMessage_isRead]  DEFAULT ((0)) FOR [isRead]
GO
ALTER TABLE [dbo].[YouAre] ADD  CONSTRAINT [DF_YouAre_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Zone] ADD  CONSTRAINT [DF_Zone_createDate]  DEFAULT (getutcdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[Acknowledgement]  WITH CHECK ADD  CONSTRAINT [FK_Acknowledgement_StatusUpdate] FOREIGN KEY([statusUpdateID])
REFERENCES [dbo].[StatusUpdate] ([statusUpdateID])
GO
ALTER TABLE [dbo].[Acknowledgement] CHECK CONSTRAINT [FK_Acknowledgement_StatusUpdate]
GO
ALTER TABLE [dbo].[ArtistEvent]  WITH CHECK ADD  CONSTRAINT [FK_ArtistTourDate_Artist] FOREIGN KEY([artistID])
REFERENCES [dbo].[Artist] ([artistID])
GO
ALTER TABLE [dbo].[ArtistEvent] CHECK CONSTRAINT [FK_ArtistTourDate_Artist]
GO
ALTER TABLE [dbo].[ArtistEvent]  WITH CHECK ADD  CONSTRAINT [FK_ArtistTourDate_TourDate] FOREIGN KEY([eventID])
REFERENCES [dbo].[Event] ([eventID])
GO
ALTER TABLE [dbo].[ArtistEvent] CHECK CONSTRAINT [FK_ArtistTourDate_TourDate]
GO
ALTER TABLE [dbo].[ArtistProperty]  WITH CHECK ADD  CONSTRAINT [FK_ArtistProperty_Artist] FOREIGN KEY([artistID])
REFERENCES [dbo].[Artist] ([artistID])
GO
ALTER TABLE [dbo].[ArtistProperty] CHECK CONSTRAINT [FK_ArtistProperty_Artist]
GO
ALTER TABLE [dbo].[BlockedUser]  WITH CHECK ADD  CONSTRAINT [FK_BlockedUser_UserAccount] FOREIGN KEY([userAccountIDBlocking])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[BlockedUser] CHECK CONSTRAINT [FK_BlockedUser_UserAccount]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_ContentType] FOREIGN KEY([contentTypeID])
REFERENCES [dbo].[ContentType] ([contentTypeID])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Content_ContentType]
GO
ALTER TABLE [dbo].[Content]  WITH CHECK ADD  CONSTRAINT [FK_Content_UserAccount] FOREIGN KEY([createdByUserID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[Content] CHECK CONSTRAINT [FK_Content_UserAccount]
GO
ALTER TABLE [dbo].[ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_ContentComment_Content] FOREIGN KEY([contentID])
REFERENCES [dbo].[Content] ([contentID])
GO
ALTER TABLE [dbo].[ContentComment] CHECK CONSTRAINT [FK_ContentComment_Content]
GO
ALTER TABLE [dbo].[ContentComment]  WITH CHECK ADD  CONSTRAINT [FK_ContentComment_UserAccount] FOREIGN KEY([createdByUserID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[ContentComment] CHECK CONSTRAINT [FK_ContentComment_UserAccount]
GO
ALTER TABLE [dbo].[ContestVideo]  WITH CHECK ADD  CONSTRAINT [FK_ContestVideo_Contest] FOREIGN KEY([contestID])
REFERENCES [dbo].[Contest] ([contestID])
GO
ALTER TABLE [dbo].[ContestVideo] CHECK CONSTRAINT [FK_ContestVideo_Contest]
GO
ALTER TABLE [dbo].[ContestVideo]  WITH CHECK ADD  CONSTRAINT [FK_ContestVideo_Video] FOREIGN KEY([videoID])
REFERENCES [dbo].[Video] ([videoID])
GO
ALTER TABLE [dbo].[ContestVideo] CHECK CONSTRAINT [FK_ContestVideo_Video]
GO
ALTER TABLE [dbo].[ContestVideoVote]  WITH CHECK ADD  CONSTRAINT [FK_ContestVideoVote_ContestVideo] FOREIGN KEY([contestVideoID])
REFERENCES [dbo].[ContestVideo] ([contestVideoID])
GO
ALTER TABLE [dbo].[ContestVideoVote] CHECK CONSTRAINT [FK_ContestVideoVote_ContestVideo]
GO
ALTER TABLE [dbo].[ContestVideoVote]  WITH CHECK ADD  CONSTRAINT [FK_ContestVideoVote_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[ContestVideoVote] CHECK CONSTRAINT [FK_ContestVideoVote_UserAccount]
GO
ALTER TABLE [dbo].[DirectMessage]  WITH CHECK ADD  CONSTRAINT [FK_DirectMessage_UserAccount] FOREIGN KEY([fromUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[DirectMessage] CHECK CONSTRAINT [FK_DirectMessage_UserAccount]
GO
ALTER TABLE [dbo].[DirectMessage]  WITH CHECK ADD  CONSTRAINT [FK_DirectMessage_UserAccount1] FOREIGN KEY([toUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[DirectMessage] CHECK CONSTRAINT [FK_DirectMessage_UserAccount1]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_TourDate_EventCycle] FOREIGN KEY([eventCycleID])
REFERENCES [dbo].[EventCycle] ([eventCycleID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_TourDate_EventCycle]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_TourDate_Venue] FOREIGN KEY([venueID])
REFERENCES [dbo].[Venue] ([venueID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_TourDate_Venue]
GO
ALTER TABLE [dbo].[LanguageUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_LanguageUserAccount_Language] FOREIGN KEY([languageID])
REFERENCES [dbo].[Language] ([languageID])
GO
ALTER TABLE [dbo].[LanguageUserAccount] CHECK CONSTRAINT [FK_LanguageUserAccount_Language]
GO
ALTER TABLE [dbo].[LanguageUserAccount]  WITH CHECK ADD  CONSTRAINT [FK_LanguageUserAccount_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[LanguageUserAccount] CHECK CONSTRAINT [FK_LanguageUserAccount_UserAccount]
GO
ALTER TABLE [dbo].[MultiProperty]  WITH CHECK ADD  CONSTRAINT [FK_MultiProperty_MultiPropertyType] FOREIGN KEY([propertyTypeID])
REFERENCES [dbo].[PropertyType] ([propertyTypeID])
GO
ALTER TABLE [dbo].[MultiProperty] CHECK CONSTRAINT [FK_MultiProperty_MultiPropertyType]
GO
ALTER TABLE [dbo].[MultiPropertyVideo]  WITH CHECK ADD  CONSTRAINT [FK_MultiPropertyVideo_MultiProperty] FOREIGN KEY([multiPropertyID])
REFERENCES [dbo].[MultiProperty] ([multiPropertyID])
GO
ALTER TABLE [dbo].[MultiPropertyVideo] CHECK CONSTRAINT [FK_MultiPropertyVideo_MultiProperty]
GO
ALTER TABLE [dbo].[MultiPropertyVideo]  WITH CHECK ADD  CONSTRAINT [FK_MultiPropertyVideo_Video] FOREIGN KEY([videoID])
REFERENCES [dbo].[Video] ([videoID])
GO
ALTER TABLE [dbo].[MultiPropertyVideo] CHECK CONSTRAINT [FK_MultiPropertyVideo_Video]
GO
ALTER TABLE [dbo].[PhotoItem]  WITH CHECK ADD  CONSTRAINT [FK_PhotoItem_UserAccount] FOREIGN KEY([createdByUserID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[PhotoItem] CHECK CONSTRAINT [FK_PhotoItem_UserAccount]
GO
ALTER TABLE [dbo].[Playlist]  WITH CHECK ADD  CONSTRAINT [FK_Playlist_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[Playlist] CHECK CONSTRAINT [FK_Playlist_UserAccount]
GO
ALTER TABLE [dbo].[PlaylistVideo]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistVideo_Playlist] FOREIGN KEY([playlistID])
REFERENCES [dbo].[Playlist] ([playlistID])
GO
ALTER TABLE [dbo].[PlaylistVideo] CHECK CONSTRAINT [FK_PlaylistVideo_Playlist]
GO
ALTER TABLE [dbo].[PlaylistVideo]  WITH CHECK ADD  CONSTRAINT [FK_PlaylistVideo_Video] FOREIGN KEY([videoID])
REFERENCES [dbo].[Video] ([videoID])
GO
ALTER TABLE [dbo].[PlaylistVideo] CHECK CONSTRAINT [FK_PlaylistVideo_Video]
GO
ALTER TABLE [dbo].[ProfileLog]  WITH CHECK ADD  CONSTRAINT [FK_ProfileLog_UserAccount] FOREIGN KEY([lookingUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[ProfileLog] CHECK CONSTRAINT [FK_ProfileLog_UserAccount]
GO
ALTER TABLE [dbo].[ProfileLog]  WITH CHECK ADD  CONSTRAINT [FK_ProfileLog_UserAccount1] FOREIGN KEY([lookedAtUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[ProfileLog] CHECK CONSTRAINT [FK_ProfileLog_UserAccount1]
GO
ALTER TABLE [dbo].[RSSItem]  WITH CHECK ADD  CONSTRAINT [FK_RSSItem_RssResource] FOREIGN KEY([rssResourceID])
REFERENCES [dbo].[RssResource] ([rssResourceID])
GO
ALTER TABLE [dbo].[RSSItem] CHECK CONSTRAINT [FK_RSSItem_RssResource]
GO
ALTER TABLE [dbo].[Size]  WITH CHECK ADD  CONSTRAINT [FK_Size_SizeType] FOREIGN KEY([sizeTypeID])
REFERENCES [dbo].[SizeType] ([sizeTypeID])
GO
ALTER TABLE [dbo].[Size] CHECK CONSTRAINT [FK_Size_SizeType]
GO
ALTER TABLE [dbo].[Song]  WITH CHECK ADD  CONSTRAINT [FK_Song_Artist] FOREIGN KEY([artistID])
REFERENCES [dbo].[Artist] ([artistID])
GO
ALTER TABLE [dbo].[Song] CHECK CONSTRAINT [FK_Song_Artist]
GO
ALTER TABLE [dbo].[SongProperty]  WITH CHECK ADD  CONSTRAINT [FK_SongProperty_Song] FOREIGN KEY([songID])
REFERENCES [dbo].[Song] ([songID])
GO
ALTER TABLE [dbo].[SongProperty] CHECK CONSTRAINT [FK_SongProperty_Song]
GO
ALTER TABLE [dbo].[StatusComment]  WITH CHECK ADD  CONSTRAINT [FK_StatusComment_StatusUpdate] FOREIGN KEY([statusUpdateID])
REFERENCES [dbo].[StatusUpdate] ([statusUpdateID])
GO
ALTER TABLE [dbo].[StatusComment] CHECK CONSTRAINT [FK_StatusComment_StatusUpdate]
GO
ALTER TABLE [dbo].[StatusCommentAcknowledgement]  WITH CHECK ADD  CONSTRAINT [FK_StatusCommentAcknowledgement_StatusComment] FOREIGN KEY([statusCommentID])
REFERENCES [dbo].[StatusComment] ([statusCommentID])
GO
ALTER TABLE [dbo].[StatusCommentAcknowledgement] CHECK CONSTRAINT [FK_StatusCommentAcknowledgement_StatusComment]
GO
ALTER TABLE [dbo].[StatusUpdate]  WITH CHECK ADD  CONSTRAINT [FK_StatusUpdate_PhotoItem1] FOREIGN KEY([photoItemID])
REFERENCES [dbo].[PhotoItem] ([photoItemID])
GO
ALTER TABLE [dbo].[StatusUpdate] CHECK CONSTRAINT [FK_StatusUpdate_PhotoItem1]
GO
ALTER TABLE [dbo].[StatusUpdate]  WITH CHECK ADD  CONSTRAINT [FK_StatusUpdate_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[StatusUpdate] CHECK CONSTRAINT [FK_StatusUpdate_UserAccount]
GO
ALTER TABLE [dbo].[StatusUpdate]  WITH CHECK ADD  CONSTRAINT [FK_StatusUpdate_Zone] FOREIGN KEY([zoneID])
REFERENCES [dbo].[Zone] ([zoneID])
GO
ALTER TABLE [dbo].[StatusUpdate] CHECK CONSTRAINT [FK_StatusUpdate_Zone]
GO
ALTER TABLE [dbo].[StatusUpdateNotification]  WITH CHECK ADD  CONSTRAINT [FK_StatusUpdateNotification_StatusUpdate] FOREIGN KEY([statusUpdateID])
REFERENCES [dbo].[StatusUpdate] ([statusUpdateID])
GO
ALTER TABLE [dbo].[StatusUpdateNotification] CHECK CONSTRAINT [FK_StatusUpdateNotification_StatusUpdate]
GO
ALTER TABLE [dbo].[UserAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountDetail_InterestedIn] FOREIGN KEY([interestedInID])
REFERENCES [dbo].[InterestedIn] ([interestedInID])
GO
ALTER TABLE [dbo].[UserAccountDetail] CHECK CONSTRAINT [FK_UserAccountDetail_InterestedIn]
GO
ALTER TABLE [dbo].[UserAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountDetail_RelationshipStatus] FOREIGN KEY([relationshipStatusID])
REFERENCES [dbo].[RelationshipStatus] ([relationshipStatusID])
GO
ALTER TABLE [dbo].[UserAccountDetail] CHECK CONSTRAINT [FK_UserAccountDetail_RelationshipStatus]
GO
ALTER TABLE [dbo].[UserAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountDetail_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAccountDetail] CHECK CONSTRAINT [FK_UserAccountDetail_UserAccount]
GO
ALTER TABLE [dbo].[UserAccountDetail]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountDetail_YouAre] FOREIGN KEY([youAreID])
REFERENCES [dbo].[YouAre] ([youAreID])
GO
ALTER TABLE [dbo].[UserAccountDetail] CHECK CONSTRAINT [FK_UserAccountDetail_YouAre]
GO
ALTER TABLE [dbo].[UserAccountMet]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountMet_UserAccount] FOREIGN KEY([userAccounted])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAccountMet] CHECK CONSTRAINT [FK_UserAccountMet_UserAccount]
GO
ALTER TABLE [dbo].[UserAccountMet]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountMet_UserAccount1] FOREIGN KEY([userAccountRequester])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAccountMet] CHECK CONSTRAINT [FK_UserAccountMet_UserAccount1]
GO
ALTER TABLE [dbo].[UserAccountRole]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountRole_Role] FOREIGN KEY([roleID])
REFERENCES [dbo].[Role] ([roleID])
GO
ALTER TABLE [dbo].[UserAccountRole] CHECK CONSTRAINT [FK_UserAccountRole_Role]
GO
ALTER TABLE [dbo].[UserAccountRole]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountRole_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAccountRole] CHECK CONSTRAINT [FK_UserAccountRole_UserAccount]
GO
ALTER TABLE [dbo].[UserAccountVideo]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountVideo_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAccountVideo] CHECK CONSTRAINT [FK_UserAccountVideo_UserAccount]
GO
ALTER TABLE [dbo].[UserAccountVideo]  WITH CHECK ADD  CONSTRAINT [FK_UserAccountVideo_Video] FOREIGN KEY([videoID])
REFERENCES [dbo].[Video] ([videoID])
GO
ALTER TABLE [dbo].[UserAccountVideo] CHECK CONSTRAINT [FK_UserAccountVideo_Video]
GO
ALTER TABLE [dbo].[UserAddress]  WITH CHECK ADD  CONSTRAINT [FK_UserAddress_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserAddress] CHECK CONSTRAINT [FK_UserAddress_UserAccount]
GO
ALTER TABLE [dbo].[UserConnection]  WITH CHECK ADD  CONSTRAINT [FK_UserConnection_UserAccount] FOREIGN KEY([fromUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserConnection] CHECK CONSTRAINT [FK_UserConnection_UserAccount]
GO
ALTER TABLE [dbo].[UserConnection]  WITH CHECK ADD  CONSTRAINT [FK_UserConnection_UserAccount1] FOREIGN KEY([toUserAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserConnection] CHECK CONSTRAINT [FK_UserConnection_UserAccount1]
GO
ALTER TABLE [dbo].[UserPhoto]  WITH CHECK ADD  CONSTRAINT [FK_UserPhoto_UserAccount] FOREIGN KEY([userAccountID])
REFERENCES [dbo].[UserAccount] ([userAccountID])
GO
ALTER TABLE [dbo].[UserPhoto] CHECK CONSTRAINT [FK_UserPhoto_UserAccount]
GO
ALTER TABLE [dbo].[VideoSong]  WITH CHECK ADD  CONSTRAINT [FK_VideoSong_Song] FOREIGN KEY([songID])
REFERENCES [dbo].[Song] ([songID])
GO
ALTER TABLE [dbo].[VideoSong] CHECK CONSTRAINT [FK_VideoSong_Song]
GO
ALTER TABLE [dbo].[VideoSong]  WITH CHECK ADD  CONSTRAINT [FK_VideoSong_Video] FOREIGN KEY([videoID])
REFERENCES [dbo].[Video] ([videoID])
GO
ALTER TABLE [dbo].[VideoSong] CHECK CONSTRAINT [FK_VideoSong_Video]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The unique ID of the role' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'roleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The name of the role' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'roleName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time the role was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'createDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time this role was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'updatedDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User that created this role' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'createdByEndUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User that updated this role' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Role', @level2type=N'COLUMN',@level2name=N'updatedByEndUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The unique username of the user in the system' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'userName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The password for the End User required for login (currently this is stored as a hash)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The format that the password is stored as (currently this is hash)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'passwordFormat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The salt that is used to unencrypt a password (currently this is not used)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'passwordSalt'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The unique e-mail address for the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'eMail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The question that the End User must respond to retrieve a reset password' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'passwordQuestion'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The answer to a question that the End User must give to retrieve a reset password  (currently this is hashed)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'passwordAnswer'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sets if the user is approved beyond the registration process (currently they are sent to their last step in the registration if this is not true)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'isApproved'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last point in time when the End User logged into the system' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'lastLoginDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last point in time when the End User changed their password' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'lastPasswordChangeDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last point in time when the End User was locked out (most likely due to a invalid login and/ or password recovery attempts within a time window)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'lastLockoutDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The amount of times that the End User failed to enter the correct password in the time window' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'failedPasswordAttemptCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User failed to enter the correct password during login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'failedPasswordAttemptWindowStart'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The amount of times that the End User failed to enter the correct password answer in the time window' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'failedPasswordAnswerAttemptCount'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User failed to enter the correct password answer during login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'failedPasswordAnswerAttemptWindowStart'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A comment about the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'comment'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'createDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'updateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that updated the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'updatedByUserAccountID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that created the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'createdByUserAccountID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A best attempt to know if an End User is online (this is set to true when the login and false when they log out, if their session expires this is not updated)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'isOnline'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Specifies if the End User is locked out' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'isLockedOut'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The last point in time that a any request from the End User was received' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccount', @level2type=N'COLUMN',@level2name=N'lastActivityDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccountDetail', @level2type=N'COLUMN',@level2name=N'createDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccountDetail', @level2type=N'COLUMN',@level2name=N'updateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that updated the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccountDetail', @level2type=N'COLUMN',@level2name=N'updatedByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that created the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAccountDetail', @level2type=N'COLUMN',@level2name=N'createdByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPhoto', @level2type=N'COLUMN',@level2name=N'createDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The point in time when the End User was updated' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPhoto', @level2type=N'COLUMN',@level2name=N'updateDate'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that updated the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPhoto', @level2type=N'COLUMN',@level2name=N'updatedByUserID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The End User ID that created the End User' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserPhoto', @level2type=N'COLUMN',@level2name=N'createdByUserID'
GO
USE [master]
GO
ALTER DATABASE DasKlubDB SET  READ_WRITE 
GO
