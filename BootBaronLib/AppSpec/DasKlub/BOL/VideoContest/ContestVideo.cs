﻿//  Copyright 2013 
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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using BootBaronLib.Values;

namespace BootBaronLib.AppSpec.DasKlub.BOL.VideoContest
{
    public class ContestVideo : BaseIUserLogCRUD
    {
        #region constructors

        public ContestVideo()
        {
        }

        #endregion

        #region properties

        private char _subContest = Convert.ToChar(SiteEnums.SubContest.U.ToString());
        public int ContestVideoID { get; set; }

        public int VideoID { get; set; }


        public int ContestRank { get; set; }


        public char SubContest
        {
            get { return _subContest; }
            set { _subContest = value; }
        }

        public int ContestID { get; set; }

        #endregion

        public ContestVideo(DataRow dr)
        {
            Get(dr);
        }

        #region methods

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddContestVideo";


            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => VideoID), VideoID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContestID), ContestID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => SubContest), SubContest);


            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            ContestVideoID = Convert.ToInt32(result);

            return ContestVideoID;
        }

        public static bool IsUserContestVoted(int userAccountID, int contestID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserContestVoted";

            comm.AddParameter("userAccountID", userAccountID);
            comm.AddParameter("contestID", contestID);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }

        public override bool Delete()
        {
            return base.Delete();
        }

        public void GetContestVideoForContestAndVideo(int videoID, int contestID)
        {
            VideoID = videoID;
            ContestID = contestID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestVideoForContestAndVideo";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => VideoID), VideoID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContestID), ContestID);


            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public void GetContestVideo(int videoID)
        {
            VideoID = videoID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestVideo";

            // create a new parameter
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => VideoID), VideoID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ContestID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContestID)]);
                ContestVideoID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContestVideoID)]);
                VideoID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => VideoID)]);
                SubContest = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => SubContest)]);
            }
            catch
            {
            }
        }


        public static void DeleteVideoFromAllContests(int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteContestVideo";

            comm.AddParameter("videoID", videoID);

            //RemoveCache();

            // execute the stored procedure

            DbAct.ExecuteNonQuery(comm);
        }

        #endregion
    }

    public class ContestVideos : List<ContestVideo>
    {
        public void GetContestVideosForContest(int contestID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestVideosForContest";

            comm.AddParameter("contestID", contestID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContestVideo cvid = null;
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    cvid = new ContestVideo(dr);
                    vid = new Video(cvid.VideoID);

                    if (vid.IsEnabled) Add(cvid);
                }
            }
        }
    }
}