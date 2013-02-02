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

namespace Migrations
{
    class Program
    {
        public static string dbName = "SQLDatabaseConnection";

        static void Main(string[] args)
        {
            Database.SetInitializer<DasKlubContext>(new DropCreateDatabaseTables());

            if (!Database.Exists(dbName))
            {
                Console.WriteLine("DATABASE DOES NOT EXIST, RUN 'dk_script.sql.sql' ON SQL SERVER 2012");
                Console.Read();
                return;
                //string dk_script = Console.ReadLine();

                //using (var myContext = new DasKlubContext())
                //{
                //    string line = null;


                //    try
                //    {
                       
                //        using (StreamReader sr = new StreamReader(dk_script))
                //        {
                //            line = sr.ReadToEnd();

                //            if (string.IsNullOrWhiteSpace(line))
                //            {
                //                Console.WriteLine("NO FILE CONTENTS");
                //            }
                //        }

                //        Console.WriteLine("STARTING DB CREATION...");

                //        line = @"EXEC sp_executesql N' " + line.Replace(@"'", @"''") + @" ';";

                //        myContext.Database.Initialize(true);
                //        myContext.Database.Create();

                //        int outrslt = myContext.Database.ExecuteSqlCommand(line);

                //        Console.WriteLine("DATABASE CREATED, " + outrslt.ToString());
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine("EXCEPTION:");
                //        Console.WriteLine(ex.Message);
                //        Console.WriteLine(line);
                //        Console.WriteLine("FAILURE!");
                //    }
                //}
            }

            RunUpdate(dbName);
        }

        private static void RunUpdate(string connectionName)
        {
            Console.WriteLine("RUNNING MIGRATIONS FOR CONNECTION NAME: " + connectionName);

            var configuration = new Configuration();
            
            configuration.TargetDatabase =
                new System.Data.Entity.Infrastructure.DbConnectionInfo(connectionName);

            try
            {
                Console.WriteLine("TRYING...");
                var migrator = new DbMigrator(configuration);

                migrator.Update();
                Console.WriteLine("SUCCESS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("EXCEPTION:");
                Console.WriteLine(ex.Message);
                Console.WriteLine("FAILURE!");
            }

            Console.Write("DONE!");
            Console.ReadKey();
        }
    }
}

