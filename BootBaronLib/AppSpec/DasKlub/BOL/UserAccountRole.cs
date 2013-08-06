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

using System.Collections.Generic;
using System.Data;
using System.Linq;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAccountRole
    {
        /// <summary>
        /// Deletes all user roles
        /// </summary>
        /// <param name="userAccountID"></param>
        /// <returns></returns>
        public static bool DeleteUserRoles(int userAccountID)
        {
            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_DeleteUserRoles";
            comm.AddParameter("userAccountID", userAccountID);
            
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        /// <summary>
        /// Gets all the users in a role
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static IList<UserAccount> GetUsersInRole(int roleID)
        {
            UserAccounts uars = null;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetUsersInRole";
            comm.AddParameter("roleID", roleID);
            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                uars = new UserAccounts();

                uars.AddRange(from DataRow dr in dt.Rows select new UserAccount(dr));
            }
            return uars;
        }

        /// <summary>
        /// Adds a user to an existing role
        /// </summary>
        /// <param name="userAccountID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static bool AddUserToRole(int userAccountID, int roleID)
        {
            if (userAccountID == 0 || roleID == 0) return false;

            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_AddUserAccountRole";
            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("roleID", roleID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}