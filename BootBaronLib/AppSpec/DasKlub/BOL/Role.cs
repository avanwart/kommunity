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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{


    public class Role : BaseIUserLogCRUD, ICacheName
    {

        #region properties 

        private int _roleID = 0;

        public int RoleID
        {
            get { return _roleID; }
            set { _roleID = value; }
        }

        private string _roleName = string.Empty;

        public string RoleName
        {
            get { return _roleName; }
            set { _roleName = value; }
        }

        private string _description = string.Empty;

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        public override void Get(DataRow dr)
        {

            base.Get(dr);
            this.RoleID = FromObj.IntFromObj(dr["roleID"]);
            this.RoleName = FromObj.StringFromObj(dr["roleName"]);
            this.Description = FromObj.StringFromObj(dr["description"]);
        }

        public Role(string roleName)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRoleByName";

            ADOExtenstion.AddParameter(comm, "roleName", roleName);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        /// <summary>
        /// Make a new role if it doesn't exist
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool Create(string roleName)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            ADOExtenstion.AddParameter(comm, "roleName", roleName);

            // execute the stored procedure
            int result = Convert.ToInt32(DbAct.ExecuteScalar(comm));

            // was something returned?
            if (result != 0) { return true; }
            else return false;
        }

        /// <summary>
        /// Get all the roles in the site in the database
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllRoles()
        {

            ArrayList allRoles = new ArrayList();
            DataTable dt;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
             comm.CommandText = "up_GetAllRoles";
            // exec
            dt = DbAct.ExecuteSelectCommand(comm);
            foreach (DataRow r in dt.Rows)
            {
                allRoles.Add(FromObj.StringFromObj(r["RoleName"]));
            }
            string[] stringArray = (string[])allRoles.ToArray(typeof(string));
            return stringArray;
        }

        /// <summary>
        /// Does this role name exist?
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool IsRole(string roleName)
        {
            return true;
            // if (roleName == string.Empty) return false; // name not given

            // // get a configured DbCommand object
            // DbCommand comm = DbAct.CreateCommand();
            // // set the stored procedure name
            ////  comm.CommandText = StoredProcedures.Name.up_IsRole.ToString();
            // // create a new parameter
            // DbParameter param = comm.CreateParameter();
            // // 
            // param.ParameterName = "@roleName";
            // param.Value = roleName;
            // param.DbType = DbType.String;
            // comm.Parameters.Add(param);

            // /// Exec
            // bool result = bool.TryParse(DbAct.ExecuteScalar(comm), out result);
            // return result;
        }


        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

 

}
