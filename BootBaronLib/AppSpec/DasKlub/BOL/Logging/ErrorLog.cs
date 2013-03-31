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
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.Logging
{
    public class ErrorLog : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private string _message = string.Empty;

        private string _url = string.Empty;
        public int ErrorLogID { get; set; }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #endregion

        public int? ResponseCode { get; set; }

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddErrorLog";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("message", Message);
            comm.AddParameter("url", Url);
            comm.AddParameter("responseCode", ResponseCode);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            ErrorLogID = Convert.ToInt32(result);

            return ErrorLogID;
        }
    }
}