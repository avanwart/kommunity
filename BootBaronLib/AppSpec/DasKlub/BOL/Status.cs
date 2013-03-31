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

using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Status
    {
        #region properties

        private string _statusCode = string.Empty;
        private string _statusDescription = string.Empty;
        public int StatusID { get; set; }

        public string StatusDescription
        {
            get { return _statusDescription; }
            set { _statusDescription = value; }
        }

        public string StatusCode
        {
            get { return _statusCode; }
            set { _statusCode = value; }
        }

        #endregion

        #region constructors 

        public Status(DataRow dr)
        {
            Get(dr);
        }

        #endregion

        #region methods

        public void Get(DataRow dr)
        {
            try
            {
                StatusCode = FromObj.StringFromObj(dr["statusCode"]);
                StatusDescription = FromObj.StringFromObj(dr["statusDescription"]);
                StatusID = FromObj.IntFromObj(dr["statusID"]);
            }
            catch
            {
                //Utilities.LogError(ex);
            }
        }

        #endregion
    }

    public class Statuses : List<Status>, IGetAll
    {
        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllStatus";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Status str = null;
                foreach (DataRow dr in dt.Rows)
                {
                    str = new Status(dr);
                    Add(str);
                }
            }
        }

        #endregion
    }
}