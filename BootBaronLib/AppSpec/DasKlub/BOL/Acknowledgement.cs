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

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Acknowledgement : BaseIUserLogCRUD
    {
        #region properties

        private int _acknowledgementID = 0;

        public int AcknowledgementID
        {
            get { return _acknowledgementID; }
            set { _acknowledgementID = value; }
        }

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

        private int _statusUpdateID = 0;

        public int StatusUpdateID
        {
            get { return _statusUpdateID; }
            set { _statusUpdateID = value; }
        }


        private int? _statusCommentID = null;
       
        public Acknowledgement(DataRow dr)
        {
            Get(dr);
        }

        public Acknowledgement()
        {
            // TODO: Complete member initialization
        }

        public int? StatusCommentID
        {
            get { return _statusCommentID; }
            set { _statusCommentID = value; }
        }

        #endregion

        public override bool Delete()
        {
            if (this.AcknowledgementID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_DeleteAcknowledgement";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.AcknowledgementID), AcknowledgementID);

           // RemoveCache();

            // execute the stored procedure

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddAcknowledgement";

            ADOExtenstion.AddParameter(comm, "statusUpdateID",  StatusUpdateID);
            ADOExtenstion.AddParameter(comm, "createdByUserID",  CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  UserAccountID);
            ADOExtenstion.AddParameter(comm, "acknowledgementType", AcknowledgementType);

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
                this.AcknowledgementID = Convert.ToInt32(result);

                return this.AcknowledgementID;
            }
        }

        public static bool IsUserAcknowledgement(int statusUpdateID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsUserAcknowledgement";

            ADOExtenstion.AddParameter(comm, "statusUpdateID",   statusUpdateID);
            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // execute the stored procedure
            return  DbAct.ExecuteScalar(comm) == "1";
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.AcknowledgementID = FromObj.IntFromObj(dr["acknowledgementID"]);
            this.AcknowledgementType = FromObj.CharFromObj(dr["acknowledgementType"]);
            this.StatusUpdateID = FromObj.IntFromObj(dr["statusUpdateID"]);
            
        }


        public void  GetAcknowledgement(int statusUpdateID, int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgement";

            ADOExtenstion.AddParameter(comm, "userAccountID", userAccountID);
            ADOExtenstion.AddParameter(comm, "statusUpdateID", statusUpdateID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt.Rows.Count == 1)
            {
                Get(dt.Rows[0]);
            }
        }
    }

    public class Acknowledgements : List<Acknowledgement>
    {
        public static bool DeleteAllAcknowledgements(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteAllAcknowledgements";

            ADOExtenstion.AddParameter(comm, "userAccountID",  userAccountID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public static int GetAcknowledgementCount(int statusUpdateID, char acknowledgementType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgementCount";
 
            ADOExtenstion.AddParameter(comm, "statusUpdateID",   statusUpdateID);
            ADOExtenstion.AddParameter(comm, "acknowledgementType", acknowledgementType);

            // execute the stored procedure
            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            else return Convert.ToInt32(str);
        }


        public static bool DeleteStatusAcknowledgements(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteStatusAcknowledgements";

            ADOExtenstion.AddParameter(comm, "statusUpdateID",  statusUpdateID);

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }



        public void GetAcknowledgementsForStatus(int statusUpdateID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAcknowledgementsForStatus";

            ADOExtenstion.AddParameter(comm, "statusUpdateID", statusUpdateID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Acknowledgement art = null;
                foreach (DataRow dr in dt.Rows)
                {
                    art = new Acknowledgement(dr);
                    this.Add(art);
                }
            }
           
        }
    }
}
