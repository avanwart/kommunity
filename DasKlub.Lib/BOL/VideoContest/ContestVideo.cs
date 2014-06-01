using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL.VideoContest
{
    public class ContestVideo : BaseIUserLogCrud
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
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

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
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (ContestVideo cvid in from DataRow dr in dt.Rows
                select new ContestVideo(dr)
                into cvid
                let vid = new Video(cvid.VideoID)
                where vid.IsEnabled
                select cvid)
            {
                Add(cvid);
            }
        }
    }
}