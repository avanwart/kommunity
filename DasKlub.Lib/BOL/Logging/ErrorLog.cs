using System;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.Logging
{
    public class ErrorLog : BaseIUserLogCrud, ICacheName
    {
        #region properties

        private string _message = string.Empty;

        private string _url = string.Empty;
        public int ErrorLogID { get; set; }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #endregion

        public int? ResponseCode { get; set; }

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddErrorLog";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("message", Message);
            comm.AddParameter("url", Url);
            comm.AddParameter("responseCode", ResponseCode);

            // the result is their ID
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            ErrorLogID = Convert.ToInt32(result);

            return ErrorLogID;
        }
    }
}