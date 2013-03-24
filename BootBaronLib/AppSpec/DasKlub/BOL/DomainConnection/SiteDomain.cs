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
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Enums;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL.DomainConnection
{
    public class SiteDomain : BaseIUserLogCRUD, ICacheName, ISet
    {

        public static string GetSiteDomainValue(SiteEnums.SiteBrandType propertyType, string language)
        {
            SiteDomain sd = new SiteDomain();

            sd.Get(propertyType.ToString(), language);

            if (sd.SiteDomainID == 0)
            {
                sd.Get(propertyType.ToString(), string.Empty);
            }

            return sd.Description;
        }


        #region properties 

        private int _siteDomainID = 0;

        public int SiteDomainID
        {
            get { return _siteDomainID; }
            set { _siteDomainID = value; }
        }

        private string _propertyType = string.Empty;

        public string PropertyType
        {
            get { return _propertyType; }
            set { _propertyType = value; }
        }

        private string _language = string.Empty;

        public string Language
        {
            get {
                if (_language == null) return string.Empty;
                _language = _language.Trim();
                
                return _language; }
            set { _language = value; }
        }

        private string _description = string.Empty;
 
        public SiteDomain(System.Data.DataRow dr)
        {
            Get(dr);
        }

        public SiteDomain()
        {
            // TODO: Complete member initialization
        }

        public SiteDomain(int siteDomainID)
        {
            Get(siteDomainID);
        }

      
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        #endregion

        #region methods 

     
        public override void Get(int uniqueID)
        {

            this.SiteDomainID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetSiteDomain";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SiteDomainID), SiteDomainID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }


        }
 

        public override bool Delete()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteSiteDomain";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SiteDomainID), SiteDomainID);

            RemoveCache();

            // execute the stored procedure
            return DbAct.ExecuteNonQuery(comm) > 0;

        }

        public override bool Update()
        {
             
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateSiteDomain";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PropertyType), PropertyType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Language), Language);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Description), Description);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SiteDomainID), SiteDomainID);
            

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            RemoveCache();

            return (result != -1);
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddSiteDomain";
             
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.PropertyType), PropertyType);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Language), Language);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Description), Description);

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
                this.SiteDomainID = Convert.ToInt32(result);

                return this.SiteDomainID;
            }
        }

        public void Get(string propertyType, string language)
        {
            this.PropertyType = propertyType;
            this.Language = language;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetSiteDomainPropertyLanguage";

                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => PropertyType), PropertyType);
                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => Language), Language);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    Get(dt.Rows[0]);

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    }
                }
                else
                {
                    // make an empty one to improve 
                    
                    DataTable table = new DataTable();
 
                    table.Columns.Add("createDate", typeof(DateTime));
                    table.Columns.Add("updateDate", typeof(DateTime));
                    table.Columns.Add("createdByUserID", typeof(int));
                    table.Columns.Add("updatedByUserID", typeof(int));
                    table.Columns.Add("siteDomainID", typeof(int));
                    table.Columns.Add("propertyType", typeof(string));
                    table.Columns.Add("description", typeof(string));
                    table.Columns.Add("language", typeof(string));
                    
                    table.Rows.Add(DateTime.MinValue, DateTime.MinValue, 0, 0,0,string.Empty,string.Empty,string.Empty);
              
                    HttpContext.Current.Cache.AddObjToCache(table.Rows[0], this.CacheName);
                }


            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }

        public void Get(string propertyType)
        {
            this.PropertyType = propertyType;

            if (HttpContext.Current == null || HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetSiteDomainProperty";

                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => PropertyType), PropertyType);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    if ( HttpContext.Current != null) HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);

                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }


        }

        public override void Get(DataRow dr)
        {
            try
            {

                base.Get(dr);

                this.SiteDomainID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.SiteDomainID)]);
                this.PropertyType = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.PropertyType)]);
                this.Description = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Description)]);
                this.Language = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Language)]);
            }
            catch
            {

            }
        }

        #endregion

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}-{2}",
                    this.GetType().FullName, this.PropertyType, this.Language);
            }
        }

        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
        }

        public bool Set()
        {
            if (this.SiteDomainID == 0) return this.Create() > 0;
            else return this.Update();
        }
    }

    public class SiteDomains : List<SiteDomain>, IGetAll
    {
        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllSiteDomain";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                SiteDomain sdomain = null;
                foreach (DataRow dr in dt.Rows)
                {
                    sdomain = new SiteDomain(dr);
                    this.Add(sdomain);
                }
            }
        }
    }
}
