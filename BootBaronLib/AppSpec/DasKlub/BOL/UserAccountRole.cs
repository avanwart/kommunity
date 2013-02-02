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
using System.Data;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using System.Web;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAccountRole
    {
        public static bool DeleteUserRoles(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserRoles";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);


            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static UserAccounts GetUsersInRole(int roleID)
        {
            UserAccounts uars = null;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUsersInRole";

            ADOExtenstion.AddParameter(comm, "roleID", roleID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                uars = new UserAccounts();
                UserAccount art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new UserAccount(dr);
                    uars.Add(art);
                }
            }
            return uars;
        }

        public static bool AddUserToRole(int userAccountID, int roleID)
        {
            if (userAccountID == 0 || roleID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccountRole";
            
            // create a new parameter
            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);
            ADOExtenstion.AddParameter(comm, "roleID", roleID);

            return DbAct.ExecuteNonQuery(comm) > 0;
          
        }
    }
}
