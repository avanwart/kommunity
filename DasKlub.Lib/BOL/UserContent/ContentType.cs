using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;

namespace DasKlub.Lib.BOL.UserContent
{
    public class ContentType : BaseIUserLogCrud
    {
        #region properties

        private string _contentName = string.Empty;


        public ContentType(DataRow dr)
        {
            Get(dr);
        }

        public ContentType()
        {
        }

        public ContentType(int p)
        {
            Get(p);
        }

        public int ContentTypeID { get; set; }

        public string ContentName
        {
            get { return _contentName; }
            set { _contentName = value; }
        }

        #endregion

        public override void Get(int contentTypeID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentTypeByID";

            comm.AddParameter("contentTypeID", contentTypeID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                ContentTypeID = FromObj.IntFromObj(dr["contentTypeID"]);

                string contentCode = FromObj.StringFromObj(dr["contentCode"]);

                ContentName = FromObj.StringFromObj(dr["contentName"]);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }
        }
    }

    public class ContentTypes : List<ContentType>, IGetAll
    {
        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllContentTypes";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                    Add(new ContentType(dr));
            }
        }

        #endregion
    }
}