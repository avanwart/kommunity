//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

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
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DBMigrator.Migrations;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;
using Configuration = DBMigrator.Migrations.Configuration;

namespace DBMigrator
{
    internal class Program
    {
        public static bool RunSeed = false;
        public static int exitCode = -1;
        public static string dbName = ConfigurationManager.AppSettings["DatabaseName"];

 
        private static void Main(string[] args)
        {
 
            Database.SetInitializer(new DropCreateDatabaseTables());

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

            var configuration = new  Configuration();

            configuration.TargetDatabase =
                new DbConnectionInfo(connectionName);

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