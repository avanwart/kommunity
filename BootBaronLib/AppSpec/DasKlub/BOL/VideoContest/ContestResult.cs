using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL.VideoContest
{
    public class ContestResult  
    {
        public int TotalCount { get; set; }
        public string UserName { get; set; }
        public string UrlTo { get; set; }

        public  void Get(DataRow dr)
        {
            try
            {
                this.TotalCount = FromObj.IntFromObj(dr["cnts"]);
                this.UserName = FromObj.StringFromObj(dr["youtubeusername"]);  
                this.UrlTo =   FromObj.StringFromObj(dr[ "videoURL"]) ;
            }
            catch
            {

            }
        }
    }

    public class ContestResults : List<ContestResult>
    {
        public void GetContestVideosForContest(int contestID)
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContestResults";

            ADOExtenstion.AddParameter(comm, "contestID", contestID);

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
                    this.Add(rslt);
                    TotalVotes += rslt.TotalCount;
                }

              
            }
        }

        public int TotalVotes { get; set; }
       
    }
}
