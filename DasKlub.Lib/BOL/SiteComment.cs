using System;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class SiteComment : BaseIUserLogCRUD
    {
        #region properties 

        private string _detail = string.Empty;
        public int SiteCommentID { get; set; }

        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }

        #endregion

        #region methods

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_AddSiteComment";

            comm.AddParameter("detail", Detail);
            comm.AddParameter("createdByUserID", CreatedByUserID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            SiteCommentID = Convert.ToInt32(result);

            return SiteCommentID;
        }

        #endregion
    }
}