web
===

IDK = Industrial Dance Kommunity;


----------------------
Installing
----------------------

1. Run the .sql script against a SQL Server 2012 DB to create the database and initial schema.

2. Create a configs.ps1 file at the root with all the variables in the Psake script default1.ps1. Run: "Invoke-psake .\default.ps1" to see all the variables. 

3. Run the Psake script task to migrate the database to the latest version (Invoke-psake .\default.ps1 MigrateDB) 

4. Enable NuGet package restore on the solution to get all the packages, set Visual Studio to use them.

5. Run the web application against the SQL Sever 2012 DB.


----------------------
Coding
----------------------

Commit your changes into a new branch for your work. 


----------------------
Database Migrations
----------------------


To add a database migration, the first thing that needs to be done is to ensure migrations are enabled. This should be done already. 

Next, open up Package Manager Console in Visual Studio and run: 

<pre>
Add-Migration -Name NAMEOFMIGRATION -ProjectName DBMigrator -StartUpProjectName DBMigrator -ConnectionStringName DasKlubContext
</pre>

If the message: 'Unable to generate an explicit migration because the following explicit migrations are pending:' comes up then the previous migrations were not run. To run those:

Update-Database -ProjectName Migrations -StartUpProjectName Migrations -ConnectionStringName SQLDatabaseConnection

This will update the database to the newest state. 

From here the Up and Down methods are generated. In there the migration can be added with raw SQL. When completed, the migration can be run with the previous Update-Database call. 


It's best to wrap the SQL in a snippet like this:
<pre>
    Sql(@"
    
          EXEC sp_executesql N'
            
            --THE_SQL_CODE
            
          ';

    ");
</pre>
