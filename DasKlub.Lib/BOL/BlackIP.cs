using System.Collections.Generic;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
{
    public class BlackIP : BaseIUserLogCrud
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
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_IsIPBlocked";

            comm.AddParameter("ipAddress", ipAddress);

            // execute the stored procedure
            return DbAct.ExecuteScalar(comm) == "1";
        }
    }
}