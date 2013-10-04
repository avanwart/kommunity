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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class UserAccountVideo
    {
        #region properties

        private DateTime _createDate = DateTime.MinValue;
        private char _videoType = char.MinValue;

        public UserAccountVideo(DataRow dr)
        {
            Get(dr);
        }

        public UserAccountVideo()
        {
         
        }

        public int VideoID { get; set; }

        /// <summary>
        ///     F = favorite video
        ///     U = uploaded video
        /// </summary>
        public char VideoType
        {
            get { return _videoType; }
            set { _videoType = value; }
        }

        public int UserAccountID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        #endregion

        public void Get(DataRow dr)
        {
            try
            {
                CreateDate = FromObj.DateFromObj(dr["createDate"]);
                UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                VideoID = FromObj.IntFromObj(dr["videoID"]);
                VideoType = FromObj.CharFromObj(dr["videoType"]);
            }
            catch
            {
                //Utilities.LogError(ex);
            }
        }

        public int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddUserAccountVideo";

            comm.AddParameter("videoID", VideoID);
            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("videoType", VideoType);

            return DbAct.ExecuteNonQuery(comm);
        }


        public static bool DeleteVideoForUser(int userAccountID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteVideoForUser";

            comm.AddParameter("videoID", videoID);
            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }

    public class UserAccountVideos : List<UserAccountVideo>
    {
        public void GetVideosForUserAccount(int userAccountID, char videoType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForUserAccount";

            // create a new parameter

            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("videoType", videoType);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccountVideo uav = null;

                foreach (DataRow dr in dt.Rows)
                {
                    uav = new UserAccountVideo(dr);
                    Add(uav);
                }
            }
        }

        public static bool DeleteUserAccountVideo(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAccountVideo";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public void GetRecentUserAccountVideos(int userAccountID, char videoType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentUserAccountVideos";

            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("videoType", videoType);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccountVideo uav = null;

                foreach (DataRow dr in dt.Rows)
                {
                    uav = new UserAccountVideo(dr);
                    Add(uav);
                }
            }
        }
    }
}