﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BLL;
using DasKlub.Lib.BOL.ArtistContent;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL
{
    public class Video : BaseIUserLogCrud, ICacheName
    {
        #region properties

        private string _providerCode = string.Empty;
        private string _providerKey = string.Empty;
        private string _providerUserKey = string.Empty;
        private DateTime _publishDate = DateTime.MinValue;
        private string _videoKey = string.Empty;

        public bool EnableTrim { get; set; }

        public int VolumeLevel { get; set; }

        public string ProviderUserKey
        {
            get { return _providerUserKey; }
            set { _providerUserKey = value; }
        }

        public int VideoID { get; set; }

        public string VideoKey
        {
            get { return _videoKey; }
            set { _videoKey = value; }
        }

        public string ProviderKey
        {
            get { return _providerKey; }
            set { _providerKey = value; }
        }

        public string ProviderCode
        {
            get { return _providerCode; }
            set { _providerCode = value; }
        }

        public bool IsHidden { get; set; }

        public bool IsEnabled { get; set; }

        public int StatusID { get; set; }


        public float Duration { get; set; }

        public float Intro { get; set; }

        public float LengthFromStart { // BUG: THE PLAYER DOES NOT READ CORRECTLY SO FORGET IT
            get; set; }

        public DateTime PublishDate
        {
            get
            {
                if (_publishDate == DateTime.MinValue)
                    return new DateTime(1900, 1, 1);

                return _publishDate;
            }
            set { _publishDate = value; }
        }

        /**********NON DB*************/

        public string HumanType
        {
            get
            {
                var pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
                var mp = new MultiProperty(VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

                return mp.Name;
            }
        }


        public string VideoType
        {
            get
            {
                var pt = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
                var mp = new MultiProperty(VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

                return mp.Name;
            }
        }

        public string VideoURL
        {
            get
            {
                var sb = new StringBuilder(100);

                // video/YT#!CmrAHl8FKb8
                sb.Append(Utilities.URLAuthority());
                sb.Append(@"/video/");
                sb.Append(ProviderCode);
                sb.Append("#!");
                sb.Append(ProviderKey);

                return sb.ToString();
            }
        }

        #endregion

        #region constructors

        public Video()
        {
        }

        public Video(string providerCode, string providerKey)
        {
            ProviderCode = providerCode;
            ProviderKey = providerKey;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetVideoByProviderKeyCode";
                // create a new parameter
                DbParameter param = comm.CreateParameter();

                param.ParameterName = "@providerKey";
                param.Value = providerKey;
                param.DbType = DbType.String;
                comm.Parameters.Add(param);
                //
                param = comm.CreateParameter();
                param.ParameterName = "@providerCode";
                param.Value = providerCode;
                param.DbType = DbType.String;
                comm.Parameters.Add(param);


                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count != 1) return;

                HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                Get(dt.Rows[0]);
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public Video(DataRow dr)
        {
            Get(dr);
        }


        public Video(int videoID)
        {
            Get(videoID);
        }

        public static int RandomVideoIDVideo()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRandomVideoID";

            string rslt = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(rslt)) return 0;

            return Convert.ToInt32(rslt);
        }

        #endregion

        public int ClipLength
        {
            get
            {
                int difbeginend = Convert.ToInt32(Duration - LengthFromStart);

                return Convert.ToInt32(Duration - (Intro + difbeginend));
            }
        }

        public void RemoveCache()
        {
            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            int vidid = VideoID;
            // other way
            VideoID = 0;
            HttpRuntime.Cache.DeleteCacheObj(CacheName);

            VideoID = vidid;
        }

        public static int GetVideoViews(int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoViews";

            comm.AddParameter("videoID", videoID);

            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;
            return Convert.ToInt32(str);
        }

        public override void Get(int videoID)
        {
            VideoID = videoID;

            if (HttpRuntime.Cache[CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetVideoByID";

                comm.AddParameter("videoID", videoID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpRuntime.Cache.AddObjToCache(dt.Rows[0], CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow) HttpRuntime.Cache[CacheName]);
            }
        }

        public override void Get(DataRow dr)
        {
            try
            {
                base.Get(dr);

                VideoID = FromObj.IntFromObj(dr["videoID"]);
                VideoKey = FromObj.StringFromObj(dr["videoKey"]);
                ProviderKey = FromObj.StringFromObj(dr["providerKey"]);
                ProviderUserKey = FromObj.StringFromObj(dr["providerUserKey"]);
                ProviderCode = FromObj.StringFromObj(dr["providerCode"]);
                IsHidden = FromObj.BoolFromObj(dr["isHidden"]);
                IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                StatusID = FromObj.IntFromObj(dr["statusID"]);
                Duration = FromObj.FloatFromObj(dr["duration"]);
                Intro = FromObj.FloatFromObj(dr["intro"]);
                LengthFromStart = FromObj.FloatFromObj(dr["lengthFromStart"]);
                VolumeLevel = FromObj.IntFromObj(dr["volumeLevel"]);
                PublishDate = FromObj.DateFromObj(dr["publishDate"]);
            }
            catch
            {
            }
        }

        public void GetRandomVideo()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRandomVideo";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0) Get(dt.Rows[0]);
        }

        public void GetMostRecentFavoriteVideo()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostRecentFavoriteVideo";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0) Get(FromObj.IntFromObj(dt.Rows[0]["videoID"]));
        }

        public static string GetVideoJSON(string key)
        {
            var vid = new Video("YT", key);

            var sngr = new SongRecord(vid);

            return sngr.JSONResponse;
        }


        public static string GetRelatedVideosList(int videoId)
        {
            var vid = new Video(videoId);
            var sngrcd = new SongRecord(vid);

            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            var mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            string humanName = mp.Name;

            int humanID = mp.MultiPropertyID;

            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            int vidTypeID = mp.MultiPropertyID;

            var vids = new Videos();

            vids.GetRelatedVideos(humanID, vidTypeID, vid.VideoID);

            var sngrcs = new SongRecords();
            SongRecord sngrcd2 = null;

            foreach (Video v1 in vids)
            {
                if (sngrcd.ProviderKey != v1.ProviderKey)
                {
                    sngrcd2 = new SongRecord(v1);
                    sngrcs.Add(sngrcd2);
                }
            }

            return HttpUtility.HtmlEncode(sngrcs.ListOfVideos(humanName, mp.Name));
        }


        public static string RelatedVidList(int videoId)
        {
            var vid = new Video(videoId);
            var sngrcd = new SongRecord(vid);
            var mp = new MultiProperty(sngrcd.Human);
            var vids = new Videos();

            int typ1 = mp.MultiPropertyID;

            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            var mp2 = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            mp = new MultiProperty(mp2.Name);
            int typ2 = mp.MultiPropertyID;

            vids.GetVideosForPropertyType2(typ1, typ2);

            Videos newVids = Videos.GetRandoms(vids);

            var sngrcs = new SongRecords();
            SongRecord sngrcd2 = null;
            int max = 1;

            foreach (Video v1 in newVids)
            {
                if (max <= 8 && sngrcd.ProviderKey != v1.ProviderKey)
                {
                    sngrcd2 = new SongRecord(v1);
                    sngrcs.Add(sngrcd2);
                    max++;
                }
            }


            return HttpUtility.HtmlEncode(sngrcs.ListOfVideos(string.Empty, string.Empty));
        }

        public static string GetRandomJSON()
        {
            var vid = new Video();
            vid.GetRandomVideo();

            var sngr = new SongRecord(vid);

            return sngr.JSONResponse;
        }


        public static string IFrameVideoOnly(string txt)
        {
            var regx = new Regex(
                "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);
            int height = 500;

            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                height = 200;
            }

            foreach (Match match in mactches)
            {
                string theLink;
                string vidKey;
                if (match.Value.Contains("http://www.youtube.com/watch?"))
                {
                    NameValueCollection nvcKey =
                        HttpUtility.ParseQueryString(match.Value.Replace("http://www.youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
                            vidKey, height));
                }
                else if (match.Value.Contains("http://youtube.com/watch?"))
                {
                    NameValueCollection nvcKey =
                        HttpUtility.ParseQueryString(match.Value.Replace("http://youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
                            vidKey, height));
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    vidKey = match.Value.Replace("http://youtu.be/", string.Empty);
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
                            vidKey, height));
                }
            }


            return txt;
        }


        public static string IFrameVideo(string txt)
        {
            return IFrameVideo(txt, 500, 0);
        }

        public static string IFrameVideo(string txt, int height)
        {
            return IFrameVideo(txt, height, 0);
        }

        /// <summary>
        ///     Converts text to text with HTML links and YouTube iframes
        /// </summary>
        /// <param name="txt"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static string IFrameVideo(string txt, int height, int width)
        {
            txt = txt.Replace("https://", "http://");

            var regx = new Regex(
                "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);

            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                height = height/2;
            }

            foreach (Match match in mactches)
            {
                string vidKey;
                string theLink;
                if (match.Value.Contains("http://www.youtube.com/watch?") ||
                    match.Value.Contains("http://youtube.com/watch?"))
                {
                    theLink = match.Value;

                    if (match.Value.Contains("http://youtube.com/watch?"))
                    {
                        theLink = match.Value.Replace("http://youtube.com/watch?",
                            "http://www.youtube.com/watch?");
                    }

                    NameValueCollection nvcKey =
                        HttpUtility.ParseQueryString(theLink.Replace("http://www.youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];

                    txt = txt.Replace(match.Value,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""{2}""  height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
                            vidKey, height, ((width == 0) ? (object) "100%" : width)));
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    vidKey = match.Value.Replace("http://youtu.be/", string.Empty);
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""{2}""  height=""{1}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>",
                            vidKey, height, ((width == 0) ? (object) "100%" : width)));
                }
                else
                {
                    const int maxChars = 30;
                    string displayLink = match.Value;
                    if (displayLink.Length > maxChars)
                    {
                        displayLink = string.Format("{0}...", displayLink.Substring(0, maxChars));
                    }
                    txt = txt.Replace(match.Value,
                        string.Format(@"<a target=""_blank"" href='{0}'>{1}</a>", match.Value, displayLink));
                }
            }

            return txt;
        }

        public static string GetRandomJSON(string vidID)
        {
            var vid = new Video("YT", vidID);
            var propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            var mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            int genreID = mp.MultiPropertyID;
            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);
            int vidTypeID = mp.MultiPropertyID;
            var vid1 = new Video();
            vid1.GetRelatedVideo(genreID, vidTypeID, vid.VideoID);
            var sngrcs = new SongRecords();
            var sngrcd2 = new SongRecord(vid1);
            sngrcs.Add(sngrcd2);

            return sngrcd2.JSONResponse;
        }


        public void GetRelatedVideo(int multiPropertyIDGenre, int multiPropertyIDVidType, int videoID)
        {
            DbCommand comm = DbAct.CreateCommand();

            comm.CommandText = "up_GetRelatedVideo";
            comm.AddParameter("multiPropertyIDGenre", multiPropertyIDGenre);
            comm.AddParameter("multiPropertyIDVidType", multiPropertyIDVidType);
            comm.AddParameter("currentVideoID", videoID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}-{2}-{3}", GetType().FullName, ProviderCode, ProviderKey
                    , VideoID);
            }
        }

        #endregion

        #region methods

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideo";

            comm.AddParameter("enableTrim", EnableTrim);
            comm.AddParameter("videoKey", VideoKey);
            comm.AddParameter("providerKey", ProviderKey);
            comm.AddParameter("providerUserKey", ProviderUserKey);
            comm.AddParameter("providerCode", ProviderCode);
            comm.AddParameter("createdByUserID", CreatedByUserID);
            comm.AddParameter("isHidden", IsHidden);
            comm.AddParameter("isEnabled", IsEnabled);
            comm.AddParameter("statusID", StatusID);
            comm.AddParameter("intro", Intro);
            comm.AddParameter("duration", Duration);
            comm.AddParameter("lengthFromStart", LengthFromStart);
            comm.AddParameter("volumeLevel", VolumeLevel);
            comm.AddParameter("publishDate", PublishDate);

            // the result is their ID
            string result = string.Empty;
            // execute the stored procedure
            result = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(result))
            {
                return 0;
            }
            VideoID = Convert.ToInt32(result);

            return VideoID;
        }

        public override bool Update()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateVideo";

            comm.AddParameter("enableTrim", EnableTrim);
            comm.AddParameter("videoKey", VideoKey);
            comm.AddParameter("providerKey", ProviderKey);
            comm.AddParameter("providerUserKey", ProviderUserKey);
            comm.AddParameter("providerCode", ProviderCode);
            comm.AddParameter("updatedByUserID", UpdatedByUserID);
            comm.AddParameter("isHidden", IsHidden);
            comm.AddParameter("isEnabled", IsEnabled);
            comm.AddParameter("statusID", StatusID);
            comm.AddParameter("intro", Intro);
            comm.AddParameter("duration", Duration);
            comm.AddParameter("lengthFromStart", LengthFromStart);
            comm.AddParameter("volumeLevel", VolumeLevel);
            comm.AddParameter("publishDate", PublishDate);
            comm.AddParameter("videoID", VideoID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            if (result > 0) RemoveCache();

            return (result != -1);
        }

        #endregion
    }

    public class Videos : List<Video>, IGetAll
    {
        public static ArrayList GetDistinctUsers()
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetDistinctUsers";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            var alist = new ArrayList();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    alist.Add(FromObj.StringFromObj(dr["providerUserKey"]));
                }
            }

            return alist;
        }


        public void GetMostWatchedVideos(int daysBack)
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetMostWatchedVideos";
            comm.AddParameter("daysBack", daysBack);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (Video vid in from DataRow dr in dt.Rows select new Video(FromObj.IntFromObj(dr["videoID"])))
            {
                Add(vid);
            }
        }


        public void GetMostRecentVideos()
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetMostRecentVideos";
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (Video vid in from DataRow dr in dt.Rows select new Video(dr))
            {
                Add(vid);
            }
        }


        public static DataSet GetAccountCloudByLetter(string letter)
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetAccountCloudByLetter";
            comm.AddParameter("firstLetter", letter);
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            var ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }


        public void GetVideosForSong(int songID)
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetVideosForSong";
            comm.AddParameter("songID", songID);
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (Video vid in from DataRow dr in dt.Rows select new Video(dr))
            {
                Add(vid);
            }
        }


        public void GetAllVideosByUser(string providerUserKey)
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetAllVideosByUser";
            comm.AddParameter("providerUserKey", providerUserKey);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (Video vid in from DataRow dr in dt.Rows select new Video(dr))
            {
                Add(vid);
            }
        }


        public static Videos GetRandoms(Videos list)
        {
            var rng = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                if (swapIndex == i) continue;
                object tmp = list[swapIndex];
                list[swapIndex] = list[i];
                list[i] = (Video) tmp;
            }

            return list;
        }

        public void GetVideosForPropertyType2(int multiPropertyID1, int multiPropertyID2)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForPropertyType2";

            comm.AddParameter("multiPropertyID1", multiPropertyID1);
            comm.AddParameter("multiPropertyID2", multiPropertyID2);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(FromObj.IntFromObj(dr["videoID"]));

                    Add(vid);
                }
            }
        }


        public void GetVideosForPropertyType(int multiPropertyID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForPropertyType";

            comm.AddParameter("multiPropertyID", multiPropertyID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(FromObj.IntFromObj(dr["videoID"]));

                    Add(vid);
                }
            }
        }

        public void GetRelatedVideos(int multiPropertyIDGenre, int multiPropertyIDVidType, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRelatedVideos";

            comm.AddParameter("multiPropertyIDGenre", multiPropertyIDGenre);
            comm.AddParameter("multiPropertyIDVidType", multiPropertyIDVidType);
            comm.AddParameter("currentVideoID", videoID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);

                    Add(vid);
                }
            }
        }

        public int GetListFilter(int pageNumber, int resultSize, int? humanType, int? footageType, int? videoType)
        {
            var allFilters = new ArrayList();

            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));


            var sb = new StringBuilder(100);

            sb.AppendFormat(@"

DECLARE  @PageIndex INT = {0}
DECLARE  @PageSize INT = {1}

SET NOCOUNT ON; 
SELECT ROW_NUMBER() OVER 
( ORDER BY publishdate DESC  ) AS RowNumber 
,vid.[videoID], [videoKey], [providerKey], [providerUserKey],
[providerCode], vid.[updatedByUserID], vid.[createDate], 
vid.[updateDate], 
vid.[createdByUserID], [isHidden], [isEnabled], [statusID], [duration], [intro], 
[lengthFromStart], [volumeLevel], enableTrim, publishdate
INTO #Results
FROM Video vid  ", pageNumber, resultSize);

            if (allFilters.Count > 0)
            {
                sb.AppendFormat(
                    @" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}",
                    allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )",
                        allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )",
                            allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )",
                                allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )",
                                    allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )",
                                        allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on  mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )",
                                            allFilters[6]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                sb.Append(@" 
WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
            }

            sb.Append(@" 
SELECT  COUNT(*) as 'totalResults'
FROM #Results

SELECT * FROM #Results
WHERE RowNumber BETWEEN(@PageIndex -1)
* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1

DROP TABLE #Results");

            int totalResults = 0;
            DbCommand comm = DbAct.CreateCommand(true);
            comm.CommandText = sb.ToString();
            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

                foreach (Video v1 in from DataRow dr in ds.Tables[1].Rows select new Video(dr))
                {
                    Add(v1);
                }
            }

            return totalResults;
        }


        public static bool HasResults(int? footageType, int? videoType, int? personType)
        {
            var allFilters = new ArrayList();

            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
            if (personType != null && personType != 0) allFilters.Add(Convert.ToInt32(personType));

            var sb = new StringBuilder(100);

            sb.AppendFormat(@"
 
SET NOCOUNT ON; 
IF EXISTS (

SELECT
vid.[videoID] 
FROM Video vid  

  ");
            if (allFilters.Count > 0)
            {
                sb.AppendFormat(
                    @" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}",
                    allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )",
                        allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )",
                            allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )",
                                allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )",
                                    allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )",
                                        allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )",
                                            allFilters[6]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                sb.Append(@" 
WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
            }

            sb.Append(@" 
)
  BEGIN 
  SELECT 1
  END
  ELSE SELECT 0");

            DbCommand comm = DbAct.CreateCommand(true);
            comm.CommandText = sb.ToString();

            bool hasResults = DbAct.ExecuteScalar(comm) == "1";

            return hasResults;
        }

        #region IGetAll Members

        public void GetAll()
        {
            DbCommand comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetAllVideos";
            DataTable dt = DbAct.ExecuteSelectCommand(comm);
            if (dt == null || dt.Rows.Count <= 0) return;

            foreach (Video vid in from DataRow dr in dt.Rows select new Video(dr))
            {
                Add(vid);
            }
        }

        #endregion
    }
}