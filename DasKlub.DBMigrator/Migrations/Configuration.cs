using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Transactions;
using DasKlub.Models;

namespace DBMigrator.Migrations
{
    public class Configuration : DbMigrationsConfiguration<DasKlubDbContext>
    {
        private const bool RunSeed = false;

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DasKlubDbContext context)
        {
            Console.WriteLine("RUNNING SEED METHOD");

            // inserts mandatory values

            #region sql

            try
            {
                context.Database.ExecuteSqlCommand(@"EXEC sp_executesql N'
             


  IF NOT EXISTS(SELECT * FROM YouAre WHERE typeLetter = ''M'')
  BEGIN
  INSERT INTO  [YouAre]
           ( [createDate]
           ,[typeLetter]
           ,[name])
     VALUES
           (GETUTCDATE()
		   ,''M''
		   ,''Male'')

  END

    IF NOT EXISTS(SELECT * FROM YouAre WHERE typeLetter = ''F'')
  BEGIN
  INSERT INTO  [YouAre]
           ( [createDate]
           ,[typeLetter]
           ,[name])
     VALUES
           (GETUTCDATE()
		   ,''F''
		   ,''Female'')

  END



IF NOT EXISTS(SELECT * FROM [Role] WHERE roleName = ''admin'')
  BEGIN
  INSERT INTO  [Role]
           ( [createDate]
           ,[roleName]
           ,[description])
     VALUES
           (GETUTCDATE()
		   ,''admin''
		   ,''Landrat'')

  END
 

';
            ");

                context.Database.ExecuteSqlCommand(@"EXEC sp_executesql N'
 
IF NOT EXISTS(SELECT * FROM [PropertyType] WHERE propertyTypeCode = ''HUMAN'')
  BEGIN
  INSERT INTO  [PropertyType]
           ( [createDate]
           ,[propertyTypeCode]
           ,[propertyTypeName])
     VALUES
           (GETUTCDATE()
		   ,''HUMAN''
		   ,''Type of Human'')

  END


      


             

IF NOT EXISTS(SELECT * FROM [PropertyType] WHERE propertyTypeCode = ''VIDTP'')
  BEGIN
  INSERT INTO  [PropertyType]
           ( [createDate]
           ,[propertyTypeCode]
           ,[propertyTypeName])
     VALUES
           (GETUTCDATE()
		   ,''VIDTP''
		   ,''Type of Video'')

  END
 



                 

IF NOT EXISTS(SELECT * FROM [PropertyType] WHERE propertyTypeCode = ''FOOTG'')
  BEGIN
  INSERT INTO  [PropertyType]
           ( [createDate]
           ,[propertyTypeCode]
           ,[propertyTypeName])
     VALUES
           (GETUTCDATE()
		   ,''FOOTG''
		   ,''Type of Footage'') 
  END

  ';
            ");

                context.Database.ExecuteSqlCommand(@"EXEC sp_executesql N'
 

IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Male'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Male''
		   ,1)  

  END



IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Female'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Female''
		   ,1)  

  END



IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''MaleDuo'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''MaleDuo''
		   ,1) 

  END




IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''FemaleDuo'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''FemaleDuo''
		   ,1)  

  END


IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Group'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Group''
		   ,1)  

  END


IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''MaleAndFemale'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''MaleAndFemale''
		   ,1)  

  END



 

IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Music'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Music''
		   ,2)  

  END



IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Interview'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Interview''
		   ,2)  

  END
 


IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Raw'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Raw''
		   ,3)  

  END


IF NOT EXISTS(SELECT * FROM [MultiProperty] WHERE name = ''Edited'')
  BEGIN
  INSERT INTO  [MultiProperty]
           ( [createDate]
           ,[name]
           ,[propertyTypeID])
     VALUES
           (GETUTCDATE()
		   ,''Edited''
		   ,3)  

  END



IF NOT EXISTS(SELECT * FROM [RelationshipStatus] WHERE name = ''SingleAndLooking'')
  BEGIN
  INSERT INTO  [RelationshipStatus]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''SingleAndLooking''
		   ,''G'')  

  END


IF NOT EXISTS(SELECT * FROM [RelationshipStatus] WHERE name = ''LookingForFriends'')
  BEGIN
  INSERT INTO  [RelationshipStatus]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''LookingForFriends''
		   ,''Y'')  

  END


IF NOT EXISTS(SELECT * FROM [RelationshipStatus] WHERE name = ''TakenOrNotInterested'')
  BEGIN
  INSERT INTO  [RelationshipStatus]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''TakenOrNotInterested''
		   ,''R'')  

  END



IF NOT EXISTS(SELECT * FROM [RelationshipStatus] WHERE name = ''Unknown'')
  BEGIN
  INSERT INTO  [RelationshipStatus]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''Unknown''
		   ,''B'')  

  END






IF NOT EXISTS(SELECT * FROM [InterestedIn] WHERE name = ''Male'')
  BEGIN
  INSERT INTO  [InterestedIn]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''Male''
		   ,''M'')  

  END

 


IF NOT EXISTS(SELECT * FROM [InterestedIn] WHERE name = ''Female'')
  BEGIN
  INSERT INTO  [InterestedIn]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''Female''
		   ,''F'')  

  END

 



IF NOT EXISTS(SELECT * FROM [InterestedIn] WHERE name = ''MaleAndFemale'')
  BEGIN
  INSERT INTO  [InterestedIn]
           ( [createDate]
           ,[name]
           ,[typeLetter])
     VALUES
           (GETUTCDATE()
		   ,''MaleAndFemale''
		   ,''B'')  

  END
                                   


';

            ");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #endregion
        }
    }

    public class DropCreateDatabaseTables : IDatabaseInitializer<DasKlubDbContext>
    {
        #region IDatabaseInitializer<Context> Members

        public void InitializeDatabase(DasKlubDbContext context)
        {
            bool dbExists;

            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                dbExists = context.Database.Exists();
            }


            if (dbExists)
            {
                // remove all tables
                //context.Database.ExecuteSqlCommand("EXEC sp_MSforeachtable @command1 = \"DROP TABLE ?\"");

                //// create all tables
                //var dbCreationScript = ((IObjectContextAdapter)context).ObjectContext.CreateDatabaseScript();
                //context.Database.ExecuteSqlCommand(dbCreationScript);

                //Seed(context);
                //context.SaveChanges();
            }
        }

        #endregion

        #region Methods

        private void Seed(DasKlubDbContext context)
        {
            /// TODO: put here your seed creation
        }

        #endregion
    }
}