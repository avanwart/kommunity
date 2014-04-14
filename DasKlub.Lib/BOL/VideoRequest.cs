using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class VideoRequest : BaseIUserLogCRUD, ICacheName
    {
        #region methods 

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideoRequest";
            // create a new parameter
            DbParameter param = null;

            param = comm.CreateParameter();
            param.ParameterName = "@createdByUserID";
            param.Value = CreatedByUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@requestURL";
            param.Value = RequestURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@statusType";
            param.Value = StatusType;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@videoKey";
            param.Value = VideoKey;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            else
            {
                VideoRequestID = Convert.ToInt32(result);

                return VideoRequestID;
            }
        }

        public void GetVideoRequest()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoRequest";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@videoKey";
            param.Value = VideoKey;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override bool Update()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateVideoRequest";


            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("requestURL", RequestURL);
            comm.AddParameter("statusType", StatusType);
            comm.AddParameter("videoKey", VideoKey);
            comm.AddParameter("videoRequestID", VideoRequestID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);
                RequestURL = FromObj.StringFromObj(dr["requestURL"]);
                StatusType = FromObj.CharFromObj(dr["statusType"]);
                VideoKey = FromObj.StringFromObj(dr["videoKey"]);
                VideoRequestID = FromObj.IntFromObj(dr["videoRequestID"]);
            }
            catch
            {
            }
        }

        public override void Get(int videoRequestID)
        {
            VideoRequestID = videoRequestID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoRequestByID";

            comm.AddParameter("videoRequestID", VideoRequestID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion

        #region properties

        private string _requestURL = string.Empty;


        private char _statusType = char.MinValue;

        private string _videoKey = string.Empty;
        public int VideoRequestID { get; set; }

        public string RequestURL
        {
            get { return _requestURL; }
            set { _requestURL = value; }
        }

        /// <summary>
        ///     W = waiting for review
        ///     R = rejected
        ///     I = invalid submission
        ///     A = approved
        /// </summary>
        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        public string VideoKey
        {
            get { return _videoKey; }
            set { _videoKey = value; }
        }

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

        public VideoRequest()
        {
        }

        public VideoRequest(DataRow dr)
        {
            Get(dr);
        }

        public VideoRequest(int videoRequestID)
        {
            Get(videoRequestID);
        }
    }

    public class VideoRequests : List<VideoRequest>
    {
        public void GetUnprocessedVideos()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetUnprocessedVideoRequests";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                VideoRequest vreq = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vreq = new VideoRequest(dr);
                    Add(vreq);
                }
            }
        }
    }
}