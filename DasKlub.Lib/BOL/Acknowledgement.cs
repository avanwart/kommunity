using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class Acknowledgement : BaseIUserLogCRUD
    {
        #region properties

        private char _acknowledgementType = char.MinValue;

        public Acknowledgement(DataRow dr)
        {
            Get(dr);
        }

        public Acknowledgement()
        {
        }

        public int AcknowledgementID { get; private set; }

        /// <summary>
        ///     A = applaud, B = beat
        /// </summary>
        public char AcknowledgementType
        {
            get { return _acknowledgementType; }
            set { _acknowledgementType = value; }
        }

        public int UserAccountID { get; set; }

        public int StatusUpdateID { get; set; }

        #endregion

        public override bool Delete()
        {
            if (AcknowledgementID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeleteAcknowledgement";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => AcknowledgementID), AcknowledgementID);

            // RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddAcknowledgement";

            comm.AddParameter("statusUpdateID", StatusUpdateID);
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("acknowledgementType", AcknowledgementType);

            // the result is their ID
            // execute the stored procedure
            var result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            AcknowledgementID = Convert.ToInt32(result);

            return AcknowledgementID;
        }

        public static bool IsUserAcknowledgement(int statusUpdateID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserAcknowledgement";

            comm.AddParameter("statusUpdateID", statusUpdateID);
            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        public override sealed void Get(DataRow dr)
        {
            base.Get(dr);

            AcknowledgementID = FromObj.IntFromObj(dr["acknowledgementID"]);
            AcknowledgementType = FromObj.CharFromObj(dr["acknowledgementType"]);
            StatusUpdateID = FromObj.IntFromObj(dr["statusUpdateID"]);
        }


        public void GetAcknowledgement(int statusUpdateID, int userAccountID)
        {
            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgement";

            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("statusUpdateID", statusUpdateID);

            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }
    }

    public class Acknowledgements : List<Acknowledgement>
    {
        public static bool DeleteAllAcknowledgements(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllAcknowledgements";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static int GetAcknowledgementCount(int statusUpdateID, char acknowledgementType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgementCount";

            comm.AddParameter("statusUpdateID", statusUpdateID);
            comm.AddParameter("acknowledgementType", acknowledgementType);

            // execute the stored procedure
            string str = DbAct.ExecuteScalar(comm);

            return string.IsNullOrEmpty(str) ? 0 : Convert.ToInt32(str);
        }


        public static bool DeleteStatusAcknowledgements(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusAcknowledgements";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


        public void GetAcknowledgementsForStatus(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgementsForStatus";

            comm.AddParameter("statusUpdateID", statusUpdateID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (var art in from DataRow dr in dt.Rows select new Acknowledgement(dr))
            {
                Add(art);
            }
        }
    }
}