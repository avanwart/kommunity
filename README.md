#[Das Klub](http://dasklub.com)
##A Kommunity For All And None.

---
### Development Software

- [Visual Studio 2012](http://www.microsoft.com/visualstudio/eng/downloads)
- [SQL Server 2012](http://www.microsoft.com/en-ca/download/details.aspx?id=29062) Get the service and the management studio: ENU\x64\SQLManagementStudio_x64_ENU.exe 
- [PowerShell 3.0](http://www.microsoft.com/web/downloads/platform.aspx)
- [Amazon Storage](http://www.cloudberrylab.com/)
- [Git Extensions](http://sourceforge.net/projects/gitextensions/)
- Git Source Control Provider. In Visual Studio go to: Tools > Extensions and Updates > Search online.

---
### Amazon
- You will need an Amazon account and to set up a bucket for content storage. You will also need to set up SES for email.

---
### Database Setup

- You will need to run the dk_script.sql file in the project against a SQL 2012 database you created.
- You'll need to update the database from its initial state: <Pre> Update-Database -ProjectName DasKlub.DBMigrator -StartUpProjectName DasKlub.DBMigrator -ConnectionStringName DasKlubDBContext -Verbose</pre>


###Database Migrations
---

To add a database migration, the first thing that needs to be done is to ensure migrations are enabled. This should be done already. 

Next, open up Package Manager Console in Visual Studio and run: 

<pre>
Add-Migration -Name NAMEOFMIGRATION -ProjectName DasKlub.DBMigrator -StartUpProjectName DasKlub.DBMigrator -ConnectionStringName DasKlubDBContext

Update-Database -ProjectName DasKlub.DBMigrator -StartUpProjectName DasKlub.DBMigrator -ConnectionStringName DasKlubDBContext -Verbose
</pre>
 
This will update the database to the newest state. 

From here the Up and Down methods are generated. In there the migration can be added with raw SQL. When completed, the migration can be run with the previous Update-Database call.

It's best to wrap the SQL in a snippet like this if it's raw SQL:
<pre>
    Sql(@"                        
            --THE_SQL_CODE
    ");
</pre>


---
### Deployment

- Get a Windows Server 2012 instance
- Set up Web Deploy ('Web Deploy for Hosting Servers' from Web Platform Installer)
- Set up the application directory with the correct permissions to deploy to it
- Run the deployemt script located at: .\deployment\ENVIRONMENTS\TARGET_ENVIRONMENT.ps1 (you will need to configure the parameters for the psake script) Example:

<pre>

 $msBuildConfig = 'release';

Invoke-psake $PSScriptRoot\..\default.ps1 deploy -properties @{
    #msbuild
    'msBuildConfig' =   $msBuildConfig;
    'msBuildVerbosity' = 'q';
    'solutionLocation' = '..\DasKlub.sln';

    #tests
    'unitTestDLLLocation' = "..\DasKlub.UnitTests\bin\$msBuildConfig\DasKlub.UnitTests.dll";
    'integrationTestDLLLocation'  = "..\DasKlub.IntegrationTests\bin\$msBuildConfig\DasKlub.IntegrationTests.dll";
    'MSTestLocation' = '..\thirdparty\tools\MSTest\MSTest.exe';
    'testSettings' = "..\TestSettings1.testsettings";
    'unitTestResultsFile'= "..\buildartifacts\unitTestResults.trx";
    'integrationTestResultsFile'  = "..\buildartifacts\integrationTestResults.trx";

    #package
    'packageOutputDir' =  '..\buildartifacts\';
    'webProjectLocation' = '..\DasKlub.Web\DasKlub.Web.csproj';
    'webprojectBinLocation' = '..\DasKlub.Web\bin';

    #db migrate
    'migrateConnectionString' = 'DATABASE_CONNECTION_STRING';
    'removeMigrateLocation' = "..\DasKlub.DBMigrator\bin\$msBuildConfig\Migrate.exe";
    'migrateApplicationDLL' = "DasKlub.DBMigrator.dll";
    'migrateExeLocation' = '..\packages\EntityFramework.6.0.0-rc1\tools\Migrate.exe'; #or whatever is more current
    'migrationProjectBinLocation' = "..\DasKlub.DBMigrator\bin\$msBuildConfig";
    'migrateDBParams' = "/verbose";

    #deploy
    'msDeployURL' = 'URL_OF_MSDEPLOY';
    'msDeployUserName' = 'USERNAME';
    'msDeployPassword' = 'PASSWORD';
    'statusCheckURL' = 'URL_TO_CONFIRM';
    'deployIisAppPath' = 'APPLICATION_NAME';

    #display
    'displayTaskStartStopTimes' = $true;
    'showConfigsAtStart' = $true;
}

</pre>



---
### Workflow

To get your code included into the master repository, fork the repository, create a new branch, commit your changes there and push.
Each commit should be small and consise, representing a small unit of work that can be rolled back.
Each commit should be written in present tense ex: 'Updates jQuery to version 2.0'.
Once all your changes have been pushed to your branch, go to your forked repository and open a pull request on the master repository (compare and review).
Include the purpose of your pull request and a summary of all the changes that are pending.
