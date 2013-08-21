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
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.AppSpec.DasKlub.BOL
{
    public class Vote
    {
        private DateTime _createDate = DateTime.MinValue;

        public Vote(DataRow dr)
        {
            VoteID = FromObj.IntFromObj(dr["voteID"]);
            UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
            CreateDate = FromObj.DateFromObj(dr["createDate"]);
            VideoID = FromObj.IntFromObj(dr["videoID"]);
            Score = FromObj.IntFromObj(dr["score"]);
        }

        public Vote()
        {
        }

        public int VoteID { get; set; }

        public int UserAccountID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public int VideoID { get; set; }

        public int Score { get; set; }

        public static bool IsUserVideoVote(int videoID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_IsUserVideoVote";

            comm.AddParameter("videoID", videoID);
            comm.AddParameter("userAccountID", userAccountID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            return result == "1";
        }


        public int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVote";

            comm.AddParameter("userAccountID", UserAccountID);
            comm.AddParameter("videoID", VideoID);
            comm.AddParameter("score", Score);


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
                VoteID = Convert.ToInt32(result);

                return VoteID;
            }
        }
    }

    public class Votes : List<Vote>, IGetAll
    {
        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllVotes";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Vote art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new Vote(dr);
                    Add(art);
                }
            }
        }

        #endregion

        public static bool DeleteUserAccountVideo(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteUserVotes";

            comm.AddParameter("userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}