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
using System.Data.Common;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class BlackIP : BaseIUserLogCRUD
    {
        #region properties 

        private string _ipAddress = string.Empty;

        public BlackIP()
        {
            BlackIPID = 0;
        }

        public int BlackIPID { get; set; }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        #endregion
    }

    public class BlackIPs : List<BlackIP>
    {
        public static bool IsIPBlocked(string ipAddress)
        {
            if (string.IsNullOrEmpty(ipAddress)) return true;

            // get a configured DbCommand object
            var comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsIPBlocked";

            comm.AddParameter("ipAddress", ipAddress);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }
    }
}