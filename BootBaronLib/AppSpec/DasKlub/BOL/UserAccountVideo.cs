//  Copyright 2012 
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
using System.Linq;
using System.Text;
using System.Data.Common;
using BootBaronLib.DAL;
using System.Data;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Operational;
using BootBaronLib.BaseTypes;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class UserAccountVideo  
    {
        #region properties

        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }

        private char _videoType = char.MinValue;

        /// <summary>
        /// F = favorite video
        /// U = uploaded video
        /// </summary>
        public char VideoType
        {
            get { return _videoType; }
            set { _videoType = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }




        private DateTime _createDate = DateTime.MinValue;

        public UserAccountVideo(DataRow dr)
        {
            Get(dr);
        }

        public UserAccountVideo()
        {
            // TODO: Complete member initialization
        }

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
                this.CreateDate = FromObj.DateFromObj(dr["createDate"]);
                this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
                this.VideoID = FromObj.IntFromObj(dr["videoID"]);
                this.VideoType = FromObj.CharFromObj(dr["videoType"]);
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

            ADOExtenstion.AddParameter(comm, "videoID", VideoID);
            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm, "videoType", VideoType);

            return DbAct.ExecuteNonQuery(comm);
        }


        public static bool DeleteVideoForUser(int userAccountID, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteVideoForUser";

            ADOExtenstion.AddParameter(comm, "videoID", videoID);
            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

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

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);
            ADOExtenstion.AddParameter(comm, "videoType", videoType);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccountVideo uav = null;

                foreach (DataRow dr in dt.Rows)
                {
                    uav = new UserAccountVideo(dr);
                    this.Add(uav);
                }
            }
        }

        public static bool DeleteUserAccountVideo(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserAccountVideo";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;

        }

        public void GetRecentUserAccountVideos(int userAccountID, char videoType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentUserAccountVideos";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);
            ADOExtenstion.AddParameter(comm, "videoType", videoType);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                UserAccountVideo uav = null;

                foreach (DataRow dr in dt.Rows)
                {
                    uav = new UserAccountVideo(dr);
                    this.Add(uav);
                }
            }
        }


    }
}
