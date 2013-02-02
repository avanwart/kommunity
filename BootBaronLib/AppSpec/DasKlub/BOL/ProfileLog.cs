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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class ProfileLog : BaseExistance, ICacheName
    {
        public static ArrayList GetRecentProfileViews(int lookedAtUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRecentProfileViews";

            ADOExtenstion.AddParameter(comm, "lookedAtUserAccountID",  lookedAtUserAccountID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null)
            {
                ArrayList theIDs = new ArrayList();

                foreach (DataRow dr in dt.Rows)
                {
                    theIDs.Add(FromObj.IntFromObj(dr["lookingUserAccountID"]));
                }

                return theIDs;
            }

            //if (string.IsNullOrEmpty(result))
            //{
            //    return 0;
            //}
            //else
            //{
            //    this.ProfileLogID = Convert.ToInt32(result);

            //    return this.ProfileLogID;
            //}

            return null;

        }

        public static int GetUniqueProfileVisitorCount(int lookedAtUserAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            
            // set the stored procedure name
            comm.CommandText = "up_GetUniqueProfileVisitorCount";

            ADOExtenstion.AddParameter(comm, "lookedAtUserAccountID", lookedAtUserAccountID);

            // execute the stored procedure
            return Convert.ToInt32(DbAct.ExecuteScalar(comm));

        }

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddProfileLog";

            ADOExtenstion.AddParameter(comm, "createdByUserID",   CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "lookingUserAccountID",  LookingUserAccountID);
            ADOExtenstion.AddParameter(comm, "lookedAtUserAccountID", LookedAtUserAccountID);

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
                this.ProfileLogID = Convert.ToInt32(result);

                return this.ProfileLogID;
            }

        }


        #region properties

        private int _profileLogID = 0;

        public int ProfileLogID
        {
            get { return _profileLogID; }
            set { _profileLogID = value; }
        }

        private int _lookingUserAccountID = 0;

        public int LookingUserAccountID
        {
            get { return _lookingUserAccountID; }
            set { _lookingUserAccountID = value; }
        }

        private int _lookedAtUserAccountID = 0;

        public int LookedAtUserAccountID
        {
            get { return _lookedAtUserAccountID; }
            set { _lookedAtUserAccountID = value; }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class ProfileLogs : List<ProfileLog>
    {
        public static bool DeleteProfileLog(int userAccountID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteProfileLog";

            ADOExtenstion.AddParameter(comm, "userAccountID",   userAccountID);
            
            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;
        }

    }
}
