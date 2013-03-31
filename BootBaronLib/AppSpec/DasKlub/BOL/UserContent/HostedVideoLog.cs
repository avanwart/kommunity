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
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.UserContent
{
    public class HostedVideoLog
    {
        #region properties

        private DateTime _createDate = DateTime.MinValue;
        private string _ipAddress = string.Empty;
        private string _videoType = string.Empty;
        private string _viewURL = string.Empty;
        public int VideoLogID { get; set; }

        public DateTime CreateDate
        {
            get { return _createDate; }
            set { _createDate = value; }
        }

        public int SecondsElapsed { get; set; }

        public string ViewURL
        {
            get { return _viewURL; }
            set { _viewURL = value; }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public string VideoType
        {
            get { return _videoType; }
            set { _videoType = value; }
        }

        #endregion

        public static void AddHostedVideoLog(string viewURL, string ipAddress, int secondsElapsed, string videoType)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddHostedVideoLog";

            comm.AddParameter("viewURL", viewURL);
            comm.AddParameter("ipAddress", ipAddress);
            comm.AddParameter("secondsElapsed", secondsElapsed);
            comm.AddParameter("videoType", videoType);

            DbAct.ExecuteNonQuery(comm);
        }
    }
}