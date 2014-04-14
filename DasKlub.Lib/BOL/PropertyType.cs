using System;
using System.Data;
using System.Data.Common;
using System.Web;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class PropertyType : BaseIUserLogCRUD, ICacheName
    {
        #region properties

        private SiteEnums.PropertyTypeCode _propertyTypeCode = SiteEnums.PropertyTypeCode.UNKNO;
        private string _propertyTypeName = string.Empty;
        public int PropertyTypeID { get; set; }

        public string PropertyTypeName
        {
            get { return _propertyTypeName; }
            set { _propertyTypeName = value; }
        }

        public SiteEnums.PropertyTypeCode PropertyTypeCode
        {
            get { return _propertyTypeCode; }
            set { _propertyTypeCode = value; }
        }

        #endregion

        #region constructors

        public PropertyType()
        {
        }

        public PropertyType(SiteEnums.PropertyTypeCode propertyTypeCode)
        {
            if (propertyTypeCode == SiteEnums.PropertyTypeCode.UNKNO) return;

            PropertyTypeCode = propertyTypeCode;

            if (HttpContext.Current == null ||
                HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetPropertyTypeByCode";

                comm.AddParameter("propertyTypeCode", propertyTypeCode.ToString());

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (HttpContext.Current != null)
                    {
                        HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    }
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public PropertyType(int propertyTypeID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPropertyTypeByID";

            comm.AddParameter("propertyTypeID", propertyTypeID);

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


                PropertyTypeID = FromObj.IntFromObj(dr["propertyTypeID"]);

                string itemPropertyType = FromObj.StringFromObj(dr["propertyTypeCode"]);

                if (string.IsNullOrEmpty(itemPropertyType))
                    PropertyTypeCode = SiteEnums.PropertyTypeCode.UNKNO;
                else

                    PropertyTypeCode =
                        (SiteEnums.PropertyTypeCode) Enum.Parse(typeof (SiteEnums.PropertyTypeCode), itemPropertyType);


                PropertyTypeName = FromObj.StringFromObj(dr["propertyTypeName"]);
                CreateDate = FromObj.DateFromObj(dr["createDate"]);
                UpdateDate = FromObj.DateFromObj(dr["updateDate"]);
                UpdatedByUserID = FromObj.IntFromObj(dr["updatedByUserID"]);
                CreatedByUserID = FromObj.IntFromObj(dr["createdByUserID"]);
            }
            catch (Exception ex)
            {
                Utilities.LogError(ex);
            }
        }

        #endregion

        #region ICacheName Members

        public string CacheName
        {
            get { return GetType().FullName + "-" + PropertyTypeID.ToString() + "-" + PropertyTypeCode.ToString(); }
        }

        public void RemoveCache()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}