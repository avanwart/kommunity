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
using System.Data;
using System.Data.Common;
using BootBaronLib.Configs;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.ComponentModel;


namespace BootBaronLib.Operational
{

 

    public static class ADOExtenstion
    {

      
 
        /// <summary>
        /// Add parameter with main provider and infer DbType from value type
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="parameterName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DbParameter AddParameter(this DbCommand comm, string parameterName, object value)
        {
            //if (value == null)
            //{
            //    Utilities.LogError("PARAM: " + parameterName + " IS NULL ");
            //    return null;
            //}

            DbProviderFactory factory = DbProviderFactories.GetFactory(DataBaseConfigs.DbProviderName);

            Type type = (value != null) ? value.GetType() : null ;

            if (type == null)
            {
                type = typeof(object);
            }

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
                    //Utilities.LogError("UNKNOWN TYPE: " + type.FullName);
                    //throw new System.Data.DataException("Type not supported for implicit DbType mapping.");
                    dbType = DbType.String; // this deals with the fact that it could be null
                    break;
            }


            try
            {
                return AddParameter(comm, factory, parameterName, dbType, value);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
                return null;
            }
        }




        public static DbParameter AddParameter(this DbCommand comm, DbProviderFactory factory, string parameterName, DbType type, object value)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException("parameterName");
            }
            //if (parameterName.StartsWith("@") || parameterName.StartsWith(":") || parameterName.StartsWith("?"))
            //{
            //    throw new ArgumentException("Do not include prefix in parameter name.");
            //}
            DbParameter param = factory.CreateParameter();
            param.DbType = type;
            //param.ParameterName = SiteConfiguration.Current.DataAccess.ParameterPrefix + parameterName;
            param.ParameterName = parameterName;

            if (value == null)
            {
                param.Value = DBNull.Value;
            }
            else
            {
                param.Value = value;
            }

            comm.Parameters.Add(param);
            return param;
        }

        public static DbParameter AddParameter<T>(this DbCommand comm, DbProviderFactory factory, string parameterName, T value)
        {
            Type type = typeof(T);
            DbType dbType;
            object parameterValue = value;
            switch (type.FullName)
            {
                case "System.String":
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
                case "System.Double":
                    dbType = DbType.Double;
                    break;
                case "System.Boolean":
                    dbType = DbType.Boolean;
                    break;
                case "System.Guid":
                    dbType = DbType.String;
                    parameterValue = ((Guid)(object)value).ToString("N");
                    break;
                default:
                    throw new System.Data.DataException("Type not supported for implicit DbType mapping.");
            }
            return AddParameter(comm, factory, parameterName, dbType, parameterValue);
        }

    }
}
