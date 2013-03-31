using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.VideoContest
{
    public class ContestResult
    {
        public int TotalCount { get; set; }
        public string UserName { get; set; }
        public string UrlTo { get; set; }

        public void Get(DataRow dr)
        {
            try
            {
                TotalCount = FromObj.IntFromObj(dr["cnts"]);
                UserName = FromObj.StringFromObj(dr["youtubeusername"]);
                UrlTo = FromObj.StringFromObj(dr["videoURL"]);
            }
            catch
            {
            }
        }
    }

    public class ContestResults : List<ContestResult>
    {
        public int TotalVotes { get; set; }

        public void GetContestVideosForContest(int contestID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestResults";

            comm.AddParameter("contestID", contestID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                ContestResult rslt = null;

                foreach (DataRow dr in dt.Rows)
                {
                    rslt = new ContestResult();
                    rslt.Get(dr);
                    Add(rslt);
                    TotalVotes += rslt.TotalCount;
                }
            }
        }
    }
}