using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class ProfileLog : BaseExistance, ICacheName
    {
        public static ArrayList GetRecentProfileViews(int lookedAtUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentProfileViews";

            comm.AddParameter("lookedAtUserAccountID", lookedAtUserAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null)
            {
                var theIDs = new ArrayList();

                foreach (DataRow dr in dt.Rows)
                {
                    theIDs.Add(FromObj.IntFromObj(dr["lookingUserAccountID"]));
                }

                return theIDs;
            }


            return null;
        }

        public static int GetUniqueProfileVisitorCount(int lookedAtUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetUniqueProfileVisitorCount";

            comm.AddParameter("lookedAtUserAccountID", lookedAtUserAccountID);

            // execute the stored procedure
            return Convert.ToInt32(DbAct.ExecuteScalar(comm));
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddProfileLog";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("lookingUserAccountID", LookingUserAccountID);
            comm.AddParameter("lookedAtUserAccountID", LookedAtUserAccountID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            ProfileLogID = Convert.ToInt32(result);

            return ProfileLogID;
        }

        #region properties

        public int ProfileLogID { get; set; }

        public int LookingUserAccountID { get; set; }

        public int LookedAtUserAccountID { get; set; }

        #endregion

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

    public class ProfileLogs : List<ProfileLog>
    {
        public static bool DeleteProfileLog(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteProfileLog";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}