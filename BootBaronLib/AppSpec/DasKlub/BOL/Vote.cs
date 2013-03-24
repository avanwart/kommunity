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
using System.Linq;
using System.Text;
using BootBaronLib.Interfaces;
using System.Data.Common;
using BootBaronLib.DAL;
using System.Data;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Vote
    {

        public static bool IsUserVideoVote(int videoID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_IsUserVideoVote";

            ADOExtenstion.AddParameter(comm, "videoID",  videoID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            return result == "1";
        }


        private int _voteID = 0;

        public int VoteID
        {
            get { return _voteID; }
            set { _voteID = value; }
        }
        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }
        private DateTime _createDate = DateTime.MinValue;

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }
        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }
        private int _score = 0;

        public int Score
        {
            get { return _score; }
            set { _score = value; }
        }


        public Vote(DataRow dr)
        {
            this.VoteID = FromObj.IntFromObj(dr["voteID"]);
            this.UserAccountID = FromObj.IntFromObj(dr["userAccountID"]);
            this.CreateDate = FromObj.DateFromObj(dr["createDate"]);
            this.VideoID = FromObj.IntFromObj(dr["videoID"]);
            this.Score = FromObj.IntFromObj(dr["score"]);
        }

        public Vote() { }

        public int Create()
        {
            
    
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVote";

            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);
            ADOExtenstion.AddParameter(comm, "videoID",  VideoID);
            ADOExtenstion.AddParameter(comm, "score", Score);


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
                this.VoteID = Convert.ToInt32(result);

                return this.VoteID;
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
                    this.Add(art);
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

            ADOExtenstion.AddParameter(comm, "userAccountID",   userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;

        }
    }
}
