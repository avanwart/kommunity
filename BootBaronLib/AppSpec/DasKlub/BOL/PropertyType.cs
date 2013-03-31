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
using System.Data;
using System.Data.Common;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Values;

namespace BootBaronLib.AppSpec.DasKlub.BOL
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
                HttpContext.Current.Cache[CacheName] == null)
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
                        HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    }
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpContext.Current.Cache[CacheName]);
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