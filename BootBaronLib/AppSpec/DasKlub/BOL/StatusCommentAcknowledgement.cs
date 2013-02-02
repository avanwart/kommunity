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
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class StatusCommentAcknowledgement : BaseIUserLogCRUD
    {
        #region properties

         

        private char _acknowledgementType = char.MinValue;

        /// <summary>
        /// A = applaud, B = beat
        /// </summary>
        public char AcknowledgementType
        {
            get { return _acknowledgementType; }
            set { _acknowledgementType = value; }
        }

        private int _userAccountID = 0;

        public int UserAccountID
        {
            get { return _userAccountID; }
            set { _userAccountID = value; }
        }

        private int _statusCommentAcknowledgementID = 0;

        public int StatusCommentAcknowledgementID
        {
            get { return _statusCommentAcknowledgementID; }
            set { _statusCommentAcknowledgementID = value; }
        }

       

        private int _statusCommentID = 0;

        public int StatusCommentID
        {
            get { return _statusCommentID; }
            set { _statusCommentID = value; }
        }

        #endregion

        #region methods
        
        public override bool Delete()
        {
            if (this.StatusCommentAcknowledgementID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusCommentAcknowledgement";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusCommentAcknowledgementID), StatusCommentAcknowledgementID);

           // RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_AddStatusCommentAcknowledgement";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusCommentID), StatusCommentID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AcknowledgementType), AcknowledgementType);

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
                this.StatusCommentAcknowledgementID = Convert.ToInt32(result);

                return this.StatusCommentAcknowledgementID;
            }
        }

        public static bool IsUserCommentAcknowledgement(int statusCommentID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserCommentAcknowledgement";

            ADOExtenstion.AddParameter(comm, "statusCommentID", statusCommentID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // execute the stored procedure
            return  DbAct.ExecuteScalar(comm) == "1";
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.StatusCommentAcknowledgementID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.StatusCommentAcknowledgementID)]);
            this.AcknowledgementType = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.AcknowledgementType)]);
            this.StatusCommentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.StatusCommentID)]);
            
        }


        public void  GetCommentAcknowledgement(int statusCommentID, int userAccountID)
        {
            this.StatusCommentID = statusCommentID;
            this.UserAccountID = userAccountID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCommentAcknowledgement";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.StatusCommentID), StatusCommentID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UserAccountID), UserAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }

        #endregion
    }

    public class StatusCommentAcknowledgements : List<StatusCommentAcknowledgement>
    {
        public static bool DeleteAllCommentAcknowledgements(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllCommentAcknowledgements";

            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static int GetCommentAcknowledgementCount(int statusCommentID, char acknowledgementType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetCommentAcknowledgementCount";

            ADOExtenstion.AddParameter(comm, "statusCommentID", statusCommentID);
            ADOExtenstion.AddParameter(comm, "acknowledgementType", acknowledgementType);

            // execute the stored procedure
            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else return Convert.ToInt32(str);
        }


        public static bool DeleteStatusCommentAcknowledgements(int statusCommentID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusCommentAcknowledgements";

            ADOExtenstion.AddParameter(comm, "statusCommentID", statusCommentID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }


    }
}
