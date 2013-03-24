namespace Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FindUserFilter : DbMigration
    {
        public override void Up()
        {
            Sql(@"

EXEC sp_executesql N'
 ALTER TABLE UserAccountDetail
ADD findUserFilter Varchar(max)';


-----------------------------------


EXEC sp_executesql N'
ALTER proc [dbo].[up_AddUserAccountDetail]

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
,@findUserFilter varchar(max)

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
,findUserFilter
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
,@findUserFilter
 )

SELECT SCOPE_IDENTITY()
';

-------------------------------
EXEC sp_executesql N'

ALTER proc [dbo].[up_GetUserAccountDetail]

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
     ,findUserFilter
  FROM UserAccountDetail
  WHERE [userAccountDetailID] = @userAccountDetailID
';
--------------------------------



EXEC sp_executesql N'
ALTER proc [dbo].[up_GetUserAccountDetailForUser]

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
    ,findUserFilter
  FROM UserAccountDetail
  WHERE userAccountID = @userAccountID
';

---------------------------



EXEC sp_executesql N'

ALTER proc [dbo].[up_UpdateUserAccountDetail]

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
,@findUserFilter varchar(max)

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
,findUserFilter  = @findUserFilter  
 WHERE userAccountDetailID  = @userAccountDetailID 

';


");
        }
        
        public override void Down()
        {
            Sql(@"

EXEC sp_executesql N'
ALTER TABLE UserAccountDetail
drop column findUserFilter  
';

-----------------------------------


EXEC sp_executesql N'
ALTER proc [dbo].[up_AddUserAccountDetail]

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
';

-------------------------------
EXEC sp_executesql N'

ALTER proc [dbo].[up_GetUserAccountDetail]

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
';
-----------------



EXEC sp_executesql N'
ALTER proc [dbo].[up_GetUserAccountDetailForUser]

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
';


----------------------------


EXEC sp_executesql N'

ALTER proc [dbo].[up_UpdateUserAccountDetail]

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

';


");
        }
    }
}
