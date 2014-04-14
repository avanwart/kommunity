using System;
using System.Data.Common;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class VideoLog
    {
        #region properties 

        private DateTime _createDate = DateTime.MinValue;
        private string _ipAddress = string.Empty;
        public int VideoLogID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public int VideoID { get; set; }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        #endregion

        public static void AddVideoLog(int videoID, string ipAddress)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideoLog";

            comm.AddParameter("videoID", videoID);
            comm.AddParameter("ipAddress", ipAddress);

            DbAct.ExecuteNonQuery(comm);
        }
    }
}