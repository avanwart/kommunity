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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.VideoContest
{
    public class ContestVideoVote : BaseIUserLogCRUD
    {
        #region properties

        private int _contestVideoVoteID = 0;

        public int ContestVideoVoteID
        {
            get { return _contestVideoVoteID; }
            set { _contestVideoVoteID = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        private int _contestVideoID = 0;

        public int ContestVideoID
        {
            get { return _contestVideoID; }
            set { _contestVideoID = value; }
        }

        #endregion

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddContestVideoVote";

            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "contestVideoID", ContestVideoID);
            ADOExtenstion.AddParameter(comm, "userAccountID", UserAccountID);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.ContestVideoVoteID = Convert.ToInt32(result);

            return this.ContestVideoVoteID;
        }

    }

    public class ContestVideoVotes : List<ContestVideoVote>
    {
        
        public static bool DeleteAllUserContestVotes(int userAccountID)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllUserContestVotes";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }
    }
}
