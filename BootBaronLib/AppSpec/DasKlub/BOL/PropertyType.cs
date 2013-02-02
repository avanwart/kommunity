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
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Enums;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.Interfaces;
using System.Web;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class PropertyType : BaseIUserLogCRUD, ICacheName
    {
         
            #region properties

            private int _propertyTypeID = 0;

            public int PropertyTypeID
            {
                get { return _propertyTypeID; }
                set { _propertyTypeID = value; }
            }


            private string _propertyTypeName = string.Empty;

            public string PropertyTypeName
            {
                get { return _propertyTypeName; }
                set { _propertyTypeName = value; }
            }

            private SiteEnums.PropertyTypeCode _propertyTypeCode = SiteEnums.PropertyTypeCode.UNKNO;

            public SiteEnums.PropertyTypeCode PropertyTypeCode
            {
                get { return _propertyTypeCode; }
                set { _propertyTypeCode = value; }
            }

            #endregion

            #region constructors

            public PropertyType() { }

            public PropertyType(SiteEnums.PropertyTypeCode propertyTypeCode)
            {
                if (propertyTypeCode == SiteEnums.PropertyTypeCode.UNKNO) return;

                this.PropertyTypeCode = propertyTypeCode;

                if (HttpContext.Current == null ||
                    HttpContext.Current.Cache[this.CacheName] == null)
                {

                    // get a configured DbCommand object
                    DbCommand comm = DbAct.CreateCommand();
                    // set the stored procedure name
                    comm.CommandText = "up_GetPropertyTypeByCode";

                    ADOExtenstion.AddParameter(comm, "propertyTypeCode",  propertyTypeCode.ToString());

                    // execute the stored procedure
                    DataTable dt = DbAct.ExecuteSelectCommand(comm);

                    // was something returned?
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                        }
                        Get(dt.Rows[0]);
                        
                    }
                }
                else
                {
                    Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
                }

            }

            public PropertyType(int propertyTypeID)
            {


                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetPropertyTypeByID";

                ADOExtenstion.AddParameter(comm, "propertyTypeID",  propertyTypeID );

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



                    this.PropertyTypeID = FromObj.IntFromObj(dr["propertyTypeID"]);

                    string itemPropertyType = FromObj.StringFromObj(dr["propertyTypeCode"]);

                    if (string.IsNullOrEmpty(itemPropertyType))
                        this.PropertyTypeCode = SiteEnums.PropertyTypeCode.UNKNO;
                    else

                        this.PropertyTypeCode = (SiteEnums.PropertyTypeCode)Enum.Parse(typeof(SiteEnums.PropertyTypeCode), itemPropertyType);



                    this.PropertyTypeName = FromObj.StringFromObj(dr["propertyTypeName"]);
                    this.CreateDate = FromObj.DateFromObj(dr["createDate"]);
                    this.UpdateDate = FromObj.DateFromObj(dr["updateDate"]);
                    this.UpdatedByUserID = FromObj.IntFromObj(dr["updatedByUserID"]);
                    this.CreatedByUserID = FromObj.IntFromObj(dr["createdByUserID"]);

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
                get { return this.GetType().FullName + "-" + this.PropertyTypeID.ToString() + "-" + this.PropertyTypeCode.ToString(); }
            }

            public void RemoveCache()
            {
                throw new NotImplementedException();
            }

            #endregion
    }
}
