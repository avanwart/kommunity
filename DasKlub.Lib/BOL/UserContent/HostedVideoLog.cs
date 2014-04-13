using System;
using System.Data.Common;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.UserContent
{
    public class HostedVideoLog
    {
        #region properties

        private DateTime _createDate = DateTime.MinValue;
        private string _ipAddress = string.Empty;
        private string _videoType = string.Empty;
        private string _viewURL = string.Empty;
        public int VideoLogID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public int SecondsElapsed { get; set; }

        public string ViewURL
        {
            get { return _viewURL; }
            set { _viewURL = value; }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public string VideoType
        {
            get { return _videoType; }
            set { _videoType = value; }
        }

        #endregion

        public static void AddHostedVideoLog(string viewURL, string ipAddress, int secondsElapsed, string videoType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddHostedVideoLog";

            comm.AddParameter("viewURL", viewURL);
            comm.AddParameter("ipAddress", ipAddress);
            comm.AddParameter("secondsElapsed", secondsElapsed);
            comm.AddParameter("videoType", videoType);

            DbAct.ExecuteNonQuery(comm);
        }
    }
}