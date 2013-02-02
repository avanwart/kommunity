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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Enums;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class MultiProperty : BaseIUserLogCRUD, ICacheName, IDisplayName
    {

        #region properties

        private int _multiPropertyID = 0;

        public int MultiPropertyID
        {
            get { return _multiPropertyID; }
            set { _multiPropertyID = value; }
        }

        private int _propertyTypeID = 0;

        public int PropertyTypeID
        {
            get { return _propertyTypeID; }
            set { _propertyTypeID = value; }
        }

        private string _name = string.Empty;

        public string Name
        {
            get
            {
                if (_name == null)
                    return string.Empty;
                else
                return _name.Trim(); }
            set { _name = value; }
        }

        private string _propertyContent = string.Empty;

        public string PropertyContent
        {
            get {
                if (_propertyContent == null) return string.Empty;
                
                return _propertyContent.Trim(); }
            set { _propertyContent = value; }
        }


        /****** extra *******/

        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }

        private int _productID = 0;

        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        #endregion

        public string DisplayName
        {
            get
            {
                string reslt = Utilities.ResourceValue(this.Name);

                if (string.IsNullOrWhiteSpace(reslt)) return this.Name;

                return reslt;

            }
        }

        #region constructors

        public MultiProperty(string name)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMultiPropertyByName";
            // create a new parameter
            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@name";
            param.Value = name;
            param.DbType = DbType.String;
            comm.Parameters.Add(param);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public MultiProperty(int typeID, int propertyTypeID, SiteEnums.MultiPropertyType mpType)
        {
            switch (mpType)
            {
                case SiteEnums.MultiPropertyType.PRODUCT:
                    GetMultiPropertyProduct(propertyTypeID, typeID);
                    break;
                case SiteEnums.MultiPropertyType.VIDEO:
                    GetMultiPropertyVideo(propertyTypeID, typeID);
                    break;
                default:
                    break;
            }
        }


        public void GetMultiPropertyVideo(int propertyTypeID, int videoID)
        {
            this.PropertyTypeID = propertyTypeID;
            this.VideoID = videoID;
            

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetMultiPropertyVideo";
                // create a new parameter
                DbParameter param = comm.CreateParameter();

                param.ParameterName = "@propertyTypeID";
                param.Value = propertyTypeID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);
                //
                param = comm.CreateParameter();
                param.ParameterName = "@videoID";
                param.Value = videoID;
                param.DbType = DbType.Int32;
                comm.Parameters.Add(param);


                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);
                    
                }
                else if (dt.Rows.Count > 1)
                {
                    // THEY SHOULD NOT HAVE MORE THAN 1, this might matter later
                    MultiProperty mpt = new MultiProperty();

                    foreach (DataRow dr in dt.Rows)
                    {
                        
                       mpt = new MultiProperty(dr);

                       MultiPropertyVideo.DeleteMultiPropertyVideo(mpt.MultiPropertyID, videoID);

                       RemoveCache();
                    }
                }
            }
            else
            {
                Get((DataRow) HttpContext.Current.Cache[this.CacheName]);
            }
        }


        public void GetMultiPropertyProduct(int propertyTypeID, int productID)
        {
            this.PropertyTypeID = propertyTypeID;
            this.ProductID = productID;

            if (HttpContext.Current == null ||
                HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetMultiPropertyProduct";

                ADOExtenstion.AddParameter(comm, "propertyTypeID", PropertyTypeID);
                ADOExtenstion.AddParameter(comm, "productID", ProductID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    Get(dt.Rows[0]);
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    }
                }
                //else if (dt.Rows.Count > 1)
                //{
                //    // THEY SHOULD NOT HAVE MORE THAN 1, this might matter later
                //    MultiProperty mpt = new MultiProperty();

                //    foreach (DataRow dr in dt.Rows)
                //    {

                //        mpt = new MultiProperty(dr);

                //        MultiPropertyProduct.DeleteMultiPropertyProduct(mpt.MultiPropertyID, productID);

                //        RemoveCache();
                //    }
                //}
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }



        public MultiProperty(DataRow dr)
        {
            Get( dr);
        }

        public MultiProperty()
        {
            // TODO: Complete member initialization
        }


        public MultiProperty(int multiPropertyProductID)
        {
            Get(multiPropertyProductID);
        }

        #endregion


        public override void Get(int uniqueID)
        {
            this.MultiPropertyID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMultiPropertyByID";

            ADOExtenstion.AddParameter(comm, "multiPropertyID", MultiPropertyID);

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
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteMultiPropertyByID";

            ADOExtenstion.AddParameter(comm, "multiPropertyID", MultiPropertyID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            if (result > 0) RemoveCache();

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);
 

            this.MultiPropertyID = FromObj.IntFromObj(dr["multiPropertyID"]);
            this.PropertyTypeID = FromObj.IntFromObj(dr["propertyTypeID"]);
            this.PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
            this.Name = FromObj.StringFromObj(dr["name"]);
            this.PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);

        }


        public override int Create()
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddMultiProperty";

            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "propertyTypeID", PropertyTypeID);
            ADOExtenstion.AddParameter(comm, "name", Name);
            ADOExtenstion.AddParameter(comm, "propertyContent", PropertyContent);

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
                this.MultiPropertyID = Convert.ToInt32(result);

                return this.MultiPropertyID;
            }

        }



        public override bool Update()
        {
            if (this.MultiPropertyID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateMultiProperty";

            // create a new parameter
            ADOExtenstion.AddParameter(comm, "propertyTypeID", PropertyTypeID);
            ADOExtenstion.AddParameter(comm, "name", Name);
            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "propertyContent", PropertyContent);
            ADOExtenstion.AddParameter(comm, "multiPropertyID", MultiPropertyID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }



        public int Set()
        {
            if (this.MultiPropertyID == 0) return Create();
            else
            {
                int rslt = 0;

                if (Update()) rslt = 1;

                return rslt;
            }
        }


        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}-{2}-{3}-{4}", this.GetType().FullName,
                    this.MultiPropertyID.ToString() ,
                    this.PropertyTypeID.ToString() ,
                    this.VideoID.ToString() ,
                    this.ProductID.ToString());
            }
        }

        public void RemoveCache()
        {
            // delete the main one
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            int multiPropertyID = this.MultiPropertyID;
            int productID = this.ProductID;

            // delete the other
            this.MultiPropertyID = 0;
            this.ProductID = 0;
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);
            
            this.MultiPropertyID = multiPropertyID;
            this.ProductID = productID;
        }

        #endregion
    }

    public class MultiProperties : List<MultiProperty>
    {
        public MultiProperties(int propertyTypeID)
        {
            GetMultiPropertyByPropertyTypeID(propertyTypeID);
        }

        public MultiProperties()
        {
            // TODO: Complete member initialization
        }

        
        public void GetMultiPropertyByPropertyTypeID(int propertyTypeID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMultiPropertyByPropertyTypeID";

            ADOExtenstion.AddParameter(comm, "propertyTypeID", propertyTypeID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            MultiProperty mp = null;

            foreach (DataRow dr in dt.Rows)
            {
                mp = new MultiProperty(dr);
                this.Add(mp);
            }


        }



        public void FilteredOptions(int propertyTypeID, int? guitarType, int? humanType, int? footageType, int? genreType, int? videoType, int? difficultyLevel, int? languge)
        {
            ArrayList allFilters = new ArrayList();

            if (guitarType != null && guitarType != 0) allFilters.Add(Convert.ToInt32(guitarType));
            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (genreType != null && genreType != 0) allFilters.Add(Convert.ToInt32(genreType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
            if (difficultyLevel != null && difficultyLevel != 0) allFilters.Add(Convert.ToInt32(difficultyLevel));
            if (languge != null && languge != 0) allFilters.Add(Convert.ToInt32(languge));

            StringBuilder sb = new StringBuilder(100);

            sb.AppendFormat(@"
 
 SELECT distinct
 		 MP0.[multiPropertyID]
      ,MP0.[propertyTypeID]
      ,MP0.[name]
      ,MP0.[createDate]
      ,MP0.[updateDate]
      ,MP0.[createdByUserID]
      ,MP0.[updatedByUserID]
      ,MP0.[propertyContent] 
  FROM  [MultiProperty] MP0
  WHERE  MP0.[multiPropertyID] in
  
 (
SELECT 
		 MP.[multiPropertyID]
     
  FROM  [MultiProperty] mp INNER JOIN MultiPropertyVideo mpv on mp.multiPropertyID = mpv.multiPropertyID INNER JOIN
  Video vid ON vid.videoID = mpv.videoID
 WHERE    mp.propertyTypeID = {0} AND isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT'

  ", propertyTypeID);

            if (allFilters.Count > 0)
            {
                sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv2.videoID FROM MultiPropertyVideo mpv2 WHERE mpv2.multiPropertyID = {0}) ", allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv3.videoID FROM MultiPropertyVideo mpv3 WHERE mpv3.multiPropertyID = {0}) ", allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv4.videoID FROM MultiPropertyVideo mpv4 WHERE mpv4.multiPropertyID = {0}) ", allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv5.videoID FROM MultiPropertyVideo mpv5 WHERE mpv5.multiPropertyID = {0}) ", allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv6.videoID FROM MultiPropertyVideo mpv6 WHERE mpv6.multiPropertyID = {0}) ", allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv7.videoID FROM MultiPropertyVideo mpv7 WHERE mpv7.multiPropertyID = {0}) ", allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(@"  AND vid.videoID in (SELECT mpv8.videoID FROM MultiPropertyVideo mpv8 WHERE mpv8.multiPropertyID = {0}) ", allFilters[6]);
                                    }
                                }
                            }
                        }


                    }


                }
            }

            sb.Append(" ) ");

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand(true);

            comm.CommandText = sb.ToString();

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            MultiProperty mp = null;

            foreach (DataRow dr in dt.Rows)
            {
                mp = new MultiProperty(dr);
                this.Add(mp);
            }
        }



    }

    
}
