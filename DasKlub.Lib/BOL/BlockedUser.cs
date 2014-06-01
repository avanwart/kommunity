using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BLL;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class BlockedUser : BaseIUserLogCrud
    {
        public BlockedUser()
        {
            UserAccountIDBlocked = 0;
            UserAccountIDBlocking = 0;
            BlockedUserID = 0;
        }

        public BlockedUser(DataRow dr)
        {
            UserAccountIDBlocked = 0;
            UserAccountIDBlocking = 0;
            BlockedUserID = 0;
            Get(dr);
        }

        #region properties

        public int BlockedUserID { get; set; }

        public int UserAccountIDBlocking { get; set; }

        public int UserAccountIDBlocked { get; set; }

        #endregion

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddBlockedUser";

            // create a new parameter

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountIDBlocking), UserAccountIDBlocking);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountIDBlocked), UserAccountIDBlocked);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            BlockedUserID = Convert.ToInt32(result);

            return BlockedUserID;
        }

        public override sealed void Get(DataRow dr)
        {
            base.Get(dr);

            BlockedUserID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => BlockedUserID)]);
            UserAccountIDBlocked =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountIDBlocked)]);
            UserAccountIDBlocking =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => UserAccountIDBlocking)]);
        }


        public override bool Delete()
        {
            return Delete(UserAccountIDBlocking, UserAccountIDBlocked);
        }


        public static bool Delete(int userAccountIDBlocking, int userAccountIDBlocked)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteBlockedUser";

            comm.AddParameter("userAccountIDBlocking", userAccountIDBlocking);
            comm.AddParameter("userAccountIDBlocked", userAccountIDBlocked);
            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public static bool IsBlockedUser(int userAccountIDBlocking, int userAccountIDBlocked)
        {
            string cacheName = string.Format("IsBlockedUser-{0}-{1}", userAccountIDBlocking, userAccountIDBlocked);

            bool rslt;


            if (HttpRuntime.Cache[cacheName] == null)
            {
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_IsBlockedUser";

                comm.AddParameter("userAccountIDBlocking", userAccountIDBlocking);
                comm.AddParameter("userAccountIDBlocked", userAccountIDBlocked);

                // execute the stored procedure
                rslt = DbAct.ExecuteScalar(comm) == "1";

                HttpRuntime.Cache.AddObjToCache(rslt, cacheName);
            }
            else
            {
                rslt = (bool) HttpRuntime.Cache[cacheName];
            }
            return rslt;
        }

        public static bool IsBlockingUser(int userAccountIDBlocking, int userAccountIDBlocked)
        {
            string cacheName = string.Format("IsBlockingUser-{0}-{1}", userAccountIDBlocking, userAccountIDBlocked);

            bool rslt;

            if (HttpRuntime.Cache[cacheName] == null)
            {
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_IsBlockingUser";

                comm.AddParameter("userAccountIDBlocking", userAccountIDBlocking);
                comm.AddParameter("userAccountIDBlocked", userAccountIDBlocked);

                // execute the stored procedure
                rslt = DbAct.ExecuteScalar(comm) == "1";

                HttpRuntime.Cache.AddObjToCache(rslt, cacheName);
            }
            else
            {
                rslt = (bool) HttpRuntime.Cache[cacheName];
            }
            return rslt;
        }
    }

    public class BlockedUsers : List<BlockedUser>
    {
        public static bool HasBlockedUsers(int userAccountIDBlocking)
        {
            var bus = new BlockedUsers();

            bus.GetBlockedUsers(userAccountIDBlocking);

            return (bus.Count > 0);
        }

        public void GetBlockedUsers(int userAccountIDBlocking)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetBlockedUsers";

            comm.AddParameter("userAccountIDBlocking", userAccountIDBlocking);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            foreach (BlockedUser ccomm in from DataRow dr in dt.Rows select new BlockedUser(dr))
            {
                Add(ccomm);
            }
        }
    }
}