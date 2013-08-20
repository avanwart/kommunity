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
using System.Data;
using System.Data.Common;
using DasKlub.Lib.Configs;

namespace DasKlub.Lib.Operational
{
    public static class ADOExtenstion
    {
        /// <summary>
        ///     Add parameter with main provider and infer DbType from value type
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void AddParameter(this DbCommand comm, string parameterName, object value)
        {
            var factory = DbProviderFactories.GetFactory(DataBaseConfigs.DbProviderName);

            var type = ((value != null) ? value.GetType() : null) ?? typeof (object);

            DbType dbType;

            switch (type.FullName)
            {
                case "System.Guid":
                    dbType = DbType.Guid;
                    break;
                case "System.DBNull":
                    dbType = DbType.Object;
                    break;
                case "System.String":
                case "System.Char":
                    dbType = DbType.String;
                    break;
                case "System.Int32":
                    dbType = DbType.Int32;
                    break;
                case "System.DateTime":
                    dbType = DbType.DateTime;
                    break;
                case "System.Int64":
                    dbType = DbType.Int64;
                    break;
                case "System.Int16":
                    dbType = DbType.Int16;
                    break;
                case "System.Decimal":
                    dbType = DbType.Decimal;
                    break;
                case "System.Single":
                    dbType = DbType.Single;
                    break;
                case "System.Double":
                case "System.Float":
                    dbType = DbType.Double;
                    break;
                case "System.Boolean":
                    dbType = DbType.Boolean;
                    break;
                default:
                    dbType = DbType.String; // this deals with the fact that it could be null
                    break;
            }


            try
            {
                AddParameter(comm, factory, parameterName, dbType, value);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }
        }


        private static void AddParameter(this DbCommand comm, DbProviderFactory factory, string parameterName, DbType type, object value)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }
            var param = factory.CreateParameter();
            if (param == null) return;
            param.DbType = type;
            param.ParameterName = parameterName;

            param.Value = value ?? DBNull.Value;

            comm.Parameters.Add(param);
        }
    }
}