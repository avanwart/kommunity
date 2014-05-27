using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL
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
            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            
            foreach (var str in from DataRow dr in dt.Rows select new Status(dr))
            {
                Add(str);
            }
        }

        #endregion
    }
}