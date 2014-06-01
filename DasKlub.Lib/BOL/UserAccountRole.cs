using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class UserAccountRole
    {
        /// <summary>
        ///     Deletes all user roles
        /// </summary>
        /// <param name="userAccountID"></param>
        /// <returns></returns>
        public static bool DeleteUserRoles(int userAccountID)
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_DeleteUserRoles";
            comm.AddParameter("userAccountID", userAccountID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        /// <summary>
        ///     Gets all the users in a role
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static IList<UserAccount> GetUsersInRole(int roleID)
        {
            UserAccounts uars = null;

            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetUsersInRole";
            comm.AddParameter("roleID", roleID);
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                uars = new UserAccounts();

                uars.AddRange(from DataRow dr in dt.Rows select new UserAccount(dr));
            }
            return uars;
        }

        /// <summary>
        ///     Adds a user to an existing role
        /// </summary>
        /// <param name="userAccountID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static bool AddUserToRole(int userAccountID, int roleID)
        {
            if (userAccountID == 0 || roleID == 0) return false;

            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_AddUserAccountRole";
            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("roleID", roleID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}