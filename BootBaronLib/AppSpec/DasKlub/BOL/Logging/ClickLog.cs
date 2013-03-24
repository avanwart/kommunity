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
using BootBaronLib.Operational;

namespace BootBaronLib.AppSpec.DasKlub.BOL.Logging
{
    public class ClickLog : BaseExistance
    {
        #region properties

        private int _clickLogID = 0;

        public int ClickLogID
        {
            get { return _clickLogID; }
            set { _clickLogID = value; }
        }

        private char _clickType = char.MinValue;

        /// <summary>
        /// V = product is being viewed
        /// T = clicking through to add to cart or affilaite outbound link
        /// </summary>
        public char ClickType
        {
            get { return _clickType; }
            set { _clickType = value; }
        }

        private string _ipAddress = string.Empty;

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        private string _currentURL = string.Empty;

        public string CurrentURL
        {
            get { return _currentURL; }
            set { _currentURL = value; }
        }

        private string _referringURL = string.Empty;

        public string ReferringURL
        {
            get { return _referringURL; }
            set { _referringURL = value; }
        }

        private int _productID = 0;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        #endregion

        #region methods

        public override int Create()
        { 
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddClickLog";

            ADOExtenstion.AddParameter(comm, "clickType", this.ClickType);
            ADOExtenstion.AddParameter(comm, "ipAddress", IpAddress);
            ADOExtenstion.AddParameter(comm, "currentURL", CurrentURL);
            ADOExtenstion.AddParameter(comm, "referringURL", ReferringURL);
            ADOExtenstion.AddParameter(comm, "productID", this.ProductID);
            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);

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
                this.ClickLogID = Convert.ToInt32(result);

                return this.ClickLogID;
            }
        }

        #endregion
    }
}
