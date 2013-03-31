//  Copyright 2013 
//  Name: Ryan Williams
//  URL: http://ryanmichaelwilliams.com | http://dasklub.com

//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using System;
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using System.Collections.Generic;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
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
            param.Value = this.CreatedByUserID;
            param.DbType = DbType.Int32;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@requestURL";
            param.Value = this.RequestURL;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@statusType";
            param.Value = this.StatusType;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);
            //
            param = comm.CreateParameter();
            param.ParameterName = "@videoKey";
            param.Value = this.VideoKey;
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
                this.VideoRequestID = Convert.ToInt32(result);

                return VideoRequestID;
            }



        }

        public  void GetVideoRequest()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoRequest";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@videoKey";
            param.Value = this.VideoKey;
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


            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "requestURL", RequestURL);
            ADOExtenstion.AddParameter(comm, "statusType", StatusType);
            ADOExtenstion.AddParameter(comm, "videoKey", VideoKey);
            ADOExtenstion.AddParameter(comm, "videoRequestID", VideoRequestID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);
                this.RequestURL = FromObj.StringFromObj(dr["requestURL"]);
                this.StatusType = FromObj.CharFromObj(dr["statusType"]);
                this.VideoKey = FromObj.StringFromObj(dr["videoKey"]);
                this.VideoRequestID = FromObj.IntFromObj(dr["videoRequestID"]);
            }
            catch { }
        }

        public override void Get(int videoRequestID)
        {
            this.VideoRequestID = videoRequestID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoRequestByID";
            
            ADOExtenstion.AddParameter(comm, "videoRequestID",   VideoRequestID);

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

        private int _videoRequestID = 0;

        public int VideoRequestID
        {
            get { return _videoRequestID; }
            set { _videoRequestID = value; }
        }



        private string _requestURL = string.Empty;

        public string RequestURL
        {
            get { return _requestURL; }
            set { _requestURL = value; }
        }


        private char _statusType = char.MinValue;

        /// <summary>
        /// W = waiting for review
        /// R = rejected
        /// I = invalid submission
        /// A = approved
        /// </summary>
        public char StatusType
        {
            get { return _statusType; }
            set { _statusType = value; }
        }

        private string _videoKey = string.Empty;

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

        public VideoRequest() { }

        public VideoRequest(DataRow dr) { Get(dr); }

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
                    this.Add(vreq);
                }
            }
        }

    }
}
