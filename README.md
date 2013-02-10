web
===

IDK = Industrial Dance Kommunity;


----------------------
Installing
----------------------

1. Run the .sql script against a SQL Server 2012 DB to install the initial schema.

2. Open the solution in Visual Studio 2012 and set the connection string to that DB in the console migrator and web probject.

3. Enable NuGet package restore on the solution to get all the packages, set Visual Studio to use them.

4. Run the console migrator to update the schema to the latest version and add starter data.

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

Add-Migration -Name NAMEOFMIGRATION -ProjectName Migrations -StartUpProjectName Migrations -ConnectionStringName SQLDatabaseConnection

If the message: 'Unable to generate an explicit migration because the following explicit migrations are pending:' comes up then the previous migrations were not run. To run those:

Update-Database -ProjectName Migrations -StartUpProjectName Migrations -ConnectionStringName SQLDatabaseConnection

This will update the database to the newest state. 

From here the Up and Down methods are generated. In there the migration can be added with raw SQL. When completed, the migration can be run with the previous Update-Database call. 
