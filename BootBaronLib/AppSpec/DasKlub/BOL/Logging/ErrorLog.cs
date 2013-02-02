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
using System.Linq;
using System.Text;
using BootBaronLib.Interfaces;
using BootBaronLib.BaseTypes;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Operational;
using System.Data;

namespace BootBaronLib.AppSpec.DasKlub.BOL.Logging
{
    public class ErrorLog : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private int _errorLogID = 0;

        public int ErrorLogID
        {
            get { return _errorLogID; }
            set { _errorLogID = value; }
        }
        private string _message = string.Empty;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        private string _url = string.Empty;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        #endregion

        public override int Create()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddErrorLog";

            ADOExtenstion.AddParameter(comm, "createdByUserID",  CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "message",  Message);
            ADOExtenstion.AddParameter(comm, "url", Url);
            ADOExtenstion.AddParameter(comm, "responseCode", ResponseCode);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result)) return 0;

            this.ErrorLogID = Convert.ToInt32(result);

            return this.ErrorLogID;


        }

        public string CacheName
        {
            get { throw new NotImplementedException(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        public int? ResponseCode { get; set; }
    }
}
