using System;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.Logging
{
    public class ClickLog : BaseExistance
    {
        #region properties

        private char _clickType = char.MinValue;
        private string _currentURL = string.Empty;

        private string _ipAddress = string.Empty;
        private string _referringURL = string.Empty;
        public int ClickLogID { get; set; }

        /// <summary>
        ///     V = product is being viewed
        ///     T = clicking through to add to cart or affilaite outbound link
        /// </summary>
        public char ClickType
        {
            get { return _clickType; }
            set { _clickType = value; }
        }

        public string IpAddress
        {
            get { return _ipAddress; }
            set { _ipAddress = value; }
        }

        public string CurrentURL
        {
            get { return _currentURL; }
            set { _currentURL = value; }
        }

        public string ReferringURL
        {
            get { return _referringURL; }
            set { _referringURL = value; }
        }

        public int ProductID { get; set; }

        #endregion

        #region methods

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddClickLog";

            comm.AddParameter("clickType", ClickType);
            comm.AddParameter("ipAddress", IpAddress);
            comm.AddParameter("currentURL", CurrentURL);
            comm.AddParameter("referringURL", ReferringURL);
            comm.AddParameter("productID", ProductID);
            comm.AddParameter("createdByUserID", CreatedByUserID);

            // the result is their ID
            // execute the stored procedure
            string result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }

            ClickLogID = Convert.ToInt32(result);

            return ClickLogID;
        }

        #endregion
    }
}