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
using System.Data;
using System.Data.Common;
using BootBaronLib.DAL;
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class VideoLog
    {
        #region properties 

        private int _videoLogID = 0;

        public int VideoLogID
        {
            get { return _videoLogID; }
            set { _videoLogID = value; }
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

        private string _ipAddress = string.Empty;

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        #endregion

        public static void AddVideoLog(int videoID, string ipAddress)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideoLog";

            ADOExtenstion.AddParameter(comm, "videoID",   videoID);
            ADOExtenstion.AddParameter(comm, "ipAddress",  ipAddress);

            DbAct.ExecuteNonQuery(comm);

        }

    }
}
