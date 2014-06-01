using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Web;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class MultiProperty : BaseIUserLogCrud, ICacheName, IDisplayName
    {
        #region properties

        private string _name = string.Empty;
        private string _propertyContent = string.Empty;
        public int MultiPropertyID { get; set; }

        public int PropertyTypeID { get; set; }

        public string Name
        {
            get
            {
                if (_name == null)
                    return string.Empty;
                else
                    return _name.Trim();
            }
            set { _name = value; }
        }

        public string PropertyContent
        {
            get
            {
                if (_propertyContent == null) return string.Empty;

                return _propertyContent.Trim();
            }
            set { _propertyContent = value; }
        }


        /****** extra *******/

        public int VideoID { get; set; }

        public int ProductID { get; set; }

        #endregion

        public string DisplayName
        {
            get
            {
                string reslt = Utilities.ResourceValue(Name);

                if (string.IsNullOrWhiteSpace(reslt)) return Name;

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


        public MultiProperty(DataRow dr)
        {
            Get(dr);
        }

        public MultiProperty()
        {
        }


        public MultiProperty(int multiPropertyProductID)
        {
            Get(multiPropertyProductID);
        }

        public void GetMultiPropertyVideo(int propertyTypeID, int videoID)
        {
            PropertyTypeID = propertyTypeID;
            VideoID = videoID;


            if (HttpRuntime.Cache[CacheName] == null)
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
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
                else if (dt.Rows.Count > 1)
                {
                    // THEY SHOULD NOT HAVE MORE THAN 1, this might matter later
                    var mpt = new MultiProperty();

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
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }


        public void GetMultiPropertyProduct(int propertyTypeID, int productID)
        {
            PropertyTypeID = propertyTypeID;
            ProductID = productID;

            if (HttpContext.Current == null ||
                HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetMultiPropertyProduct";

                comm.AddParameter("propertyTypeID", PropertyTypeID);
                comm.AddParameter("productID", ProductID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    Get(dt.Rows[0]);
                    if (HttpContext.Current != null)
                    {
                        HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    }
                }
           
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        #endregion

        public override void Get(int uniqueID)
        {
            MultiPropertyID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMultiPropertyByID";

            comm.AddParameter("multiPropertyID", MultiPropertyID);

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

            comm.AddParameter("multiPropertyID", MultiPropertyID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            if (result > 0) RemoveCache();

            return (result != -1);
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);


            MultiPropertyID = FromObj.IntFromObj(dr["multiPropertyID"]);
            PropertyTypeID = FromObj.IntFromObj(dr["propertyTypeID"]);
            PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
            Name = FromObj.StringFromObj(dr["name"]);
            PropertyContent = FromObj.StringFromObj(dr["propertyContent"]);
        }


        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddMultiProperty";

            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("propertyTypeID", PropertyTypeID);
            comm.AddParameter("name", Name);
            comm.AddParameter("propertyContent", PropertyContent);

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
                MultiPropertyID = Convert.ToInt32(result);

                return MultiPropertyID;
            }
        }


        public override bool Update()
        {
            if (MultiPropertyID == 0) return false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateMultiProperty";

            // create a new parameter
            comm.AddParameter("propertyTypeID", PropertyTypeID);
            comm.AddParameter("name", Name);
            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("propertyContent", PropertyContent);
            comm.AddParameter("multiPropertyID", MultiPropertyID);

            // result will represent the number of changed rows
            bool result = false;
            // execute the stored procedure
            result = Convert.ToBoolean(DbAct.ExecuteNonQuery(comm));

            RemoveCache();

            return result;
        }


        public int Set()
        {
            if (MultiPropertyID == 0) return Create();
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
                return string.Format("{0}-{1}-{2}-{3}-{4}", GetType().FullName,
                                     MultiPropertyID.ToString(),
                                     PropertyTypeID.ToString(),
                                     VideoID.ToString(),
                                     ProductID.ToString());
            }
        }

        public void RemoveCache()
        {
            // delete the main one
            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            int multiPropertyID = MultiPropertyID;
            int productID = ProductID;

            // delete the other
            MultiPropertyID = 0;
            ProductID = 0;
            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            MultiPropertyID = multiPropertyID;
            ProductID = productID;
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

            comm.AddParameter("propertyTypeID", propertyTypeID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            MultiProperty mp = null;

            foreach (DataRow dr in dt.Rows)
            {
                mp = new MultiProperty(dr);
                Add(mp);
            }
        }


        public void FilteredOptions(int propertyTypeID, int? guitarType, int? humanType, int? footageType,
                                    int? genreType, int? videoType, int? difficultyLevel, int? languge)
        {
            var allFilters = new ArrayList();

            if (guitarType != null && guitarType != 0) allFilters.Add(Convert.ToInt32(guitarType));
            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (genreType != null && genreType != 0) allFilters.Add(Convert.ToInt32(genreType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
            if (difficultyLevel != null && difficultyLevel != 0) allFilters.Add(Convert.ToInt32(difficultyLevel));
            if (languge != null && languge != 0) allFilters.Add(Convert.ToInt32(languge));

            var sb = new StringBuilder(100);

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
                sb.AppendFormat(
                    @"  AND vid.videoID in (SELECT mpv2.videoID FROM MultiPropertyVideo mpv2 WHERE mpv2.multiPropertyID = {0}) ",
                    allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(
                        @"  AND vid.videoID in (SELECT mpv3.videoID FROM MultiPropertyVideo mpv3 WHERE mpv3.multiPropertyID = {0}) ",
                        allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(
                            @"  AND vid.videoID in (SELECT mpv4.videoID FROM MultiPropertyVideo mpv4 WHERE mpv4.multiPropertyID = {0}) ",
                            allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(
                                @"  AND vid.videoID in (SELECT mpv5.videoID FROM MultiPropertyVideo mpv5 WHERE mpv5.multiPropertyID = {0}) ",
                                allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(
                                    @"  AND vid.videoID in (SELECT mpv6.videoID FROM MultiPropertyVideo mpv6 WHERE mpv6.multiPropertyID = {0}) ",
                                    allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(
                                        @"  AND vid.videoID in (SELECT mpv7.videoID FROM MultiPropertyVideo mpv7 WHERE mpv7.multiPropertyID = {0}) ",
                                        allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(
                                            @"  AND vid.videoID in (SELECT mpv8.videoID FROM MultiPropertyVideo mpv8 WHERE mpv8.multiPropertyID = {0}) ",
                                            allFilters[6]);
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
                Add(mp);
            }
        }
    }
}