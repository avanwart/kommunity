//  Copyright 2012 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

 
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace DBMigrator
{

    class Program
    {
        public static bool RunSeed = false;
        public static int exitCode = -1;



        public static string dbName = "DasKlubContext";

        static void Main(string[] args)
        {
            Database.SetInitializer<Migrations.DasKlubContext>(new Migrations.DropCreateDatabaseTables());

            if (!Database.Exists(dbName))
            {
                Console.WriteLine("DATABASE DOES NOT EXIST, RUN 'dk_script.sql.sql' ON SQL SERVER 2012");
                Environment.Exit(exitCode);
                Console.Read();
                return;
            }

            RunUpdate(dbName);
        }

        private static void RunUpdate(string connectionName)
        {

            Console.WriteLine("RUNNING MIGRATIONS FOR CONNECTION NAME: " + connectionName);

            var configuration = new Migrations.Configuration();

            configuration.TargetDatabase =
                     new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionName);

            try
            {
                Console.WriteLine("TRYING...");
                var migrator = new DbMigrator(configuration);

                migrator.Update();
                Console.WriteLine("SUCCESS!");
                exitCode = 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("FAILURE!");
            }

            Console.WriteLine("DONE!");
            Environment.Exit(exitCode);
        }
    }
}
