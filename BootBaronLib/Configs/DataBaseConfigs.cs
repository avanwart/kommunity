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
using System.Configuration;

namespace BootBaronLib.Configs
{
    public class DataBaseConfigs
    {
        #region constructors

        static DataBaseConfigs()
        {
            // database
            _dbConnectionString =
                ConfigurationManager.ConnectionStrings["SQLDatabaseConnection"].ConnectionString.ToString();
            _dbProviderName =
                ConfigurationManager.ConnectionStrings["SQLDatabaseConnection"].ProviderName.ToString();
        }

        #endregion

        #region variables

        // database
        private static string _dbConnectionString = string.Empty;
        private readonly static string _dbProviderName = string.Empty;

        #endregion

        #region database

        /// <summary>
        /// The connectin string to the database
        /// </summary>
        public static string DbConnectionString
        {
            set
            {
                _dbConnectionString = value;
            }

            get
            {
                return _dbConnectionString;
            }
        }

        public static string DbProviderName
        {
            get
            {
                return _dbProviderName;
            }
        }

        #endregion
    }
}