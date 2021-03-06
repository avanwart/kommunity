﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class StatusCommentAcknowledgement : BaseIUserLogCrud
    {
        #region properties

        private char _acknowledgementType = char.MinValue;

        /// <summary>
        ///     A = applaud, B = beat
        /// </summary>
        public char AcknowledgementType
        {
            get { return _acknowledgementType; }
            set { _acknowledgementType = value; }
        }

        public int UserAccountID { get; set; }

        public int StatusCommentAcknowledgementID { get; set; }


        public int StatusCommentID { get; set; }

        #endregion

        #region methods

        public override bool Delete()
        {
            if (StatusCommentAcknowledgementID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusCommentAcknowledgement";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusCommentAcknowledgementID),
                StatusCommentAcknowledgementID);

            // RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_AddStatusCommentAcknowledgement";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusCommentID), StatusCommentID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AcknowledgementType), AcknowledgementType);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            StatusCommentAcknowledgementID = Convert.ToInt32(result);

            return StatusCommentAcknowledgementID;
        }

        public static bool IsUserCommentAcknowledgement(int statusCommentID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserCommentAcknowledgement";

            comm.AddParameter("statusCommentID", statusCommentID);
            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            StatusCommentAcknowledgementID =
                FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusCommentAcknowledgementID)]);
            AcknowledgementType =
                FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => AcknowledgementType)]);
            StatusCommentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => StatusCommentID)]);
        }


        public void GetCommentAcknowledgement(int statusCommentID, int userAccountID)
        {
            StatusCommentID = statusCommentID;
            UserAccountID = userAccountID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCommentAcknowledgement";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => StatusCommentID), StatusCommentID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UserAccountID), UserAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion
    }

    public class StatusCommentAcknowledgements : List<StatusCommentAcknowledgement>
    {
        public static bool DeleteAllCommentAcknowledgements(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllCommentAcknowledgements";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static int GetCommentAcknowledgementCount(int statusCommentID, char acknowledgementType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCommentAcknowledgementCount";

            comm.AddParameter("statusCommentID", statusCommentID);
            comm.AddParameter("acknowledgementType", acknowledgementType);

            // execute the stored procedure
            var str = DbAct.ExecuteScalar(comm);

            return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
        }


        public static bool DeleteStatusCommentAcknowledgements(int statusCommentID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusCommentAcknowledgements";

            comm.AddParameter("statusCommentID", statusCommentID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}