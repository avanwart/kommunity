using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using DBMigrator.Migrations;
using Configuration = DBMigrator.Migrations.Configuration;

namespace DasKlub.DBMigrator
{
    internal class Program
    {
        public static bool RunSeed = false;
        public static int ExitCode = -1;
        public static string dbName = ConfigurationManager.AppSettings["DatabaseName"];

 
        private static void Main(string[] args)
        {
 
            Database.SetInitializer(new DropCreateDatabaseTables());

            if (!Database.Exists(dbName))
            {
                Console.WriteLine("DATABASE DOES NOT EXIST, RUN 'dk_script.sql.sql' ON SQL SERVER 2012");
                Environment.Exit(ExitCode);
                Console.Read();
                return;
            }

            RunUpdate(dbName);
        }

        private static void RunUpdate(string connectionName)
        {
            Console.WriteLine("RUNNING MIGRATIONS FOR CONNECTION NAME: " + connectionName);

            var configuration = new  Configuration();

            configuration.TargetDatabase =
                new DbConnectionInfo(connectionName);

            try
            {
                Console.WriteLine("TRYING...");
                var migrator = new DbMigrator(configuration);

                migrator.Update();
                Console.WriteLine("SUCCESS!");
                ExitCode = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("FAILURE!");
            }

            Console.WriteLine("DONE!");
            Environment.Exit(ExitCode);
        }
    }
}