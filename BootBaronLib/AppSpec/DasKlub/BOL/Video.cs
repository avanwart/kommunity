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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BLL;
using BootBaronLib.AppSpec.DasKlub.BOL.ArtistContent;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Enums;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Resources;

namespace BootBaronLib.AppSpec.DasKlub.BOL
{
    public class Video : BaseIUserLogCRUD, BootBaronLib.Interfaces.ICacheName
    {

        #region properties

        private bool _enableTrim = false;

        public bool EnableTrim
        {
            get { return _enableTrim; }
            set { _enableTrim = value; }
        }

        private int _volumeLevel = 0;

        public int VolumeLevel
        {
            get { return _volumeLevel; }
            set { _volumeLevel = value; }
        }


        private string _providerUserKey = string.Empty;

        public string ProviderUserKey
        {
            get { return _providerUserKey; }
            set { _providerUserKey = value; }
        }

        private int _videoID = 0;

        public int VideoID
        {
            get { return _videoID; }
            set { _videoID = value; }
        }
        private string _videoKey = string.Empty;

        public string VideoKey
        {
            get { return _videoKey; }
            set { _videoKey = value; }
        }
        private string _providerKey = string.Empty;

        public string ProviderKey
        {
            get { return _providerKey; }
            set { _providerKey = value; }
        }
        private string _providerCode = string.Empty;

        public string ProviderCode
        {
            get { return _providerCode; }
            set { _providerCode = value; }
        }

        private bool _isHidden = false;

        public bool IsHidden
        {
            get { return _isHidden; }
            set { _isHidden = value; }
        }
        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }
        private int _statusID = 0;

        public int StatusID
        {
            get { return _statusID; }
            set { _statusID = value; }
        }


        private float _duration = 0;

        public float Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
        private float _intro = 0;

        public float Intro
        {
            get { return _intro; }
            set { _intro = value; }
        }
        private float _lengthFromStart = 0;

        public float LengthFromStart
        {
            // BUG: THE PLAYER DOES NOT READ CORRECTLY SO FORGET IT
            get { return _lengthFromStart; }
            //get { return this.Duration; }
            set { _lengthFromStart = value; }
        }

        private DateTime _publishDate = DateTime.MinValue;

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
                PropertyType pt = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
                MultiProperty mp = new MultiProperty(this.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

                return mp.Name;
            }
        }


        public string VideoType
        {
            get
            {
                PropertyType pt = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
                MultiProperty mp = new MultiProperty(this.VideoID, pt.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

                return mp.Name;
            }
        }

        public string VideoURL
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                //http://DasKlub.com/video/YT#!CmrAHl8FKb8
                sb.Append(Utilities.URLAuthority());
                sb.Append(@"/video/");
                sb.Append(this.ProviderCode);
                sb.Append("#!");
                sb.Append(this.ProviderKey);

                return sb.ToString();
            }
        }

        #endregion

        #region constructors

        public Video() { }

        public Video(string providerCode, string providerKey)
        {
            this.ProviderCode = providerCode;
            this.ProviderKey = providerKey;

            if (HttpContext.Current.Cache[this.CacheName] == null)
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

                if (dt.Rows.Count == 1)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
                    Get(dt.Rows[0]);
                }
            }
            else
            {
                Get((DataRow)HttpContext.Current.Cache[this.CacheName]);
            }
        }

        public Video(DataRow dr)
        {
            Get(dr);
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










        public Video(int videoID)
        {
            Get(videoID);
        }

        #endregion


        public static int GetVideoViews(int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideoViews";

            ADOExtenstion.AddParameter(comm, "videoID", videoID);

            string str = DbAct.ExecuteScalar(comm);

            if (string.IsNullOrEmpty(str)) return 0;
            else return Convert.ToInt32(str);
        }

        public override void Get(int videoID)
        {
            this.VideoID = videoID;

            if (HttpContext.Current.Cache[this.CacheName] == null)
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetVideoByID";

                ADOExtenstion.AddParameter(comm, "videoID", videoID);

                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                if (dt.Rows.Count == 1)
                {
                    HttpContext.Current.Cache.AddObjToCache(dt.Rows[0], this.CacheName);
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

                this.VideoID = FromObj.IntFromObj(dr["videoID"]);
                this.VideoKey = FromObj.StringFromObj(dr["videoKey"]);
                this.ProviderKey = FromObj.StringFromObj(dr["providerKey"]);
                this.ProviderUserKey = FromObj.StringFromObj(dr["providerUserKey"]);
                this.ProviderCode = FromObj.StringFromObj(dr["providerCode"]);
                this.IsHidden = FromObj.BoolFromObj(dr["isHidden"]);
                this.IsEnabled = FromObj.BoolFromObj(dr["isEnabled"]);
                this.StatusID = FromObj.IntFromObj(dr["statusID"]);
                this.Duration = FromObj.FloatFromObj(dr["duration"]);
                this.Intro = FromObj.FloatFromObj(dr["intro"]);
                this.LengthFromStart = FromObj.FloatFromObj(dr["lengthFromStart"]);
                this.VolumeLevel = FromObj.IntFromObj(dr["volumeLevel"]);
                //  this.EnableTrim = FromObj.BoolFromObj(dr["enableTrim"]);
                this.PublishDate = FromObj.DateFromObj(dr["publishDate"]);

            }
            catch 
            {
                //Utilities.LogError(ex);
            }
        }

        #region methods

        public override int Create()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_AddVideo";

            ADOExtenstion.AddParameter(comm, "enableTrim", EnableTrim);
            ADOExtenstion.AddParameter(comm, "videoKey", VideoKey);
            ADOExtenstion.AddParameter(comm, "providerKey", ProviderKey);
            ADOExtenstion.AddParameter(comm, "providerUserKey", ProviderUserKey);
            ADOExtenstion.AddParameter(comm, "providerCode", ProviderCode);
            ADOExtenstion.AddParameter(comm, "createdByUserID", CreatedByUserID);
            ADOExtenstion.AddParameter(comm, "isHidden", IsHidden);
            ADOExtenstion.AddParameter(comm, "isEnabled", IsEnabled);
            ADOExtenstion.AddParameter(comm, "statusID", StatusID);
            ADOExtenstion.AddParameter(comm, "intro", Intro);
            ADOExtenstion.AddParameter(comm, "duration", Duration);
            ADOExtenstion.AddParameter(comm, "lengthFromStart", LengthFromStart);
            ADOExtenstion.AddParameter(comm, "volumeLevel", VolumeLevel);
            ADOExtenstion.AddParameter(comm, "publishDate", PublishDate);

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
                this.VideoID = Convert.ToInt32(result);

                return this.VideoID;
            }

        }

        public override bool Update()
        {

            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_UpdateVideo";

            ADOExtenstion.AddParameter(comm, "enableTrim", EnableTrim);
            ADOExtenstion.AddParameter(comm, "videoKey", VideoKey);
            ADOExtenstion.AddParameter(comm, "providerKey", ProviderKey);
            ADOExtenstion.AddParameter(comm, "providerUserKey", ProviderUserKey);
            ADOExtenstion.AddParameter(comm, "providerCode", ProviderCode);
            ADOExtenstion.AddParameter(comm, "updatedByUserID", UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, "isHidden", IsHidden);
            ADOExtenstion.AddParameter(comm, "isEnabled", IsEnabled);
            ADOExtenstion.AddParameter(comm, "statusID", StatusID);
            ADOExtenstion.AddParameter(comm, "intro", Intro);
            ADOExtenstion.AddParameter(comm, "duration", Duration);
            ADOExtenstion.AddParameter(comm, "lengthFromStart", LengthFromStart);
            ADOExtenstion.AddParameter(comm, "volumeLevel", VolumeLevel);
            ADOExtenstion.AddParameter(comm, "publishDate", PublishDate);
            ADOExtenstion.AddParameter(comm, "videoID", VideoID);

            int result = -1;

            result = DbAct.ExecuteNonQuery(comm);

            if (result > 0) RemoveCache();

            return (result != -1);
        }

        #endregion


        public void RemoveCache()
        {
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            int vidid = this.VideoID;
            // other way
            this.VideoID = 0;
            HttpContext.Current.Cache.DeleteCacheObj(this.CacheName);

            this.VideoID = vidid;
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
            Video vid = new Video("YT", key);

            SongRecord sngr = new SongRecord(vid);

            //            string jsongic =

            //            @"{""EndTime"": """ + sngr.EndTime.ToString() + @""", 
            //            ""Human"": """ + sngr.Human + @""", 
            //            ""ProviderCode"": """ + sngr.ProviderCode + @""", 
            //            ""ProviderKey"": """ + sngr.ProviderKey + @""", 
            //            ""SongDisplay"": """ + sngr.SongDisplay + @""", 
            //            ""StartTime"": """ + sngr.StartTime.ToString() + @""", 
            //            ""TotalSeconds"": """ + sngr.TotalSeconds.ToString() + @""", 
            //            ""UserAccount"": """ + sngr.UserAccount + @""", 
            //            ""RelatedVids"": """ + Video.RelatedVidList(vid.VideoID) + @""", 
            //            ""VolumeLevel"": """ + sngr.VolumeLevel.ToString() + @"""}";

            //            return jsongic;

            return sngr.JSONResponse;
        }


        public static string GetRelatedVideosList(int videoId)
        {

            Video vid = new Video(videoId);
            SongRecord sngrcd = new SongRecord(vid);

            PropertyType propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            MultiProperty mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            string humanName = mp.Name;

            int humanID = mp.MultiPropertyID;

            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            int vidTypeID = mp.MultiPropertyID;

            Videos vids = new Videos();

            vids.GetRelatedVideos(humanID, vidTypeID, vid.VideoID);

            //Videos newVids = Videos.GetRandoms(vids);

            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd2 = null;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video v1 in vids)
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
            Video vid = new Video(videoId);
            SongRecord sngrcd = new SongRecord(vid);
            MultiProperty mp = new MultiProperty(sngrcd.Human);
            Videos vids = new Videos();

            int typ1 = mp.MultiPropertyID;

            PropertyType propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            MultiProperty mp2 = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            mp = new MultiProperty(mp2.Name);
            int typ2 = mp.MultiPropertyID;

            vids.GetVideosForPropertyType2(typ1, typ2);

            Videos newVids = Videos.GetRandoms(vids);

            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd2 = null;
            int max = 1;

            foreach (BootBaronLib.AppSpec.DasKlub.BOL.Video v1 in newVids)
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

            Video vid = new Video();
            vid.GetRandomVideo();

            SongRecord sngr = new SongRecord(vid);


            //            string jsongic =

            //            @"{""EndTime"": """ + sngr.EndTime.ToString() + @""", 
            //            ""Human"": """ + sngr.Human + @""", 
            //            ""ProviderCode"": """ + sngr.ProviderCode + @""", 
            //            ""ProviderKey"": """ + sngr.ProviderKey + @""", 
            //            ""SongDisplay"": """ + sngr.SongDisplay + @""", 
            //            ""StartTime"": """ + sngr.StartTime.ToString() + @""", 
            //            ""TotalSeconds"": """ + sngr.TotalSeconds.ToString() + @""", 
            //            ""UserAccount"": """ + sngr.UserAccount + @""", 
            //            ""RelatedVids"": """ + Video.RelatedVidList(vid.VideoID) + @""", 
            //            ""VolumeLevel"": """ + sngr.VolumeLevel.ToString() + @"""}";

            //            return jsongic;

            return sngr.JSONResponse;

        }


        #region ICacheName Members

        public string CacheName
        {
            get
            {
                return string.Format("{0}-{1}-{2}-{3}", this.GetType().FullName, this.ProviderCode, this.ProviderKey
                    , this.VideoID.ToString());
            }
        }

        #endregion

        public int ClipLength
        {
            get
            {
                int difbeginend = Convert.ToInt32(this.Duration - this.LengthFromStart);

                return Convert.ToInt32(this.Duration - (this.Intro + difbeginend));
            }
        }


        public static string IFrameVideoOnly(string txt)
        {
            Regex regx = new Regex(
                "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);
            NameValueCollection nvcKey = null;
            string vidKey = string.Empty;
            string theLink = string.Empty;
            int height = 500;
            int width = 400;

            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                height = 200;
            }

            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http://www.youtube.com/watch?"))
                {
                    nvcKey = HttpUtility.ParseQueryString(match.Value.Replace("http://www.youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey, width, height));
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    vidKey = match.Value.Replace("http://youtu.be/", string.Empty);
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey, width, height));

                }

            }


            return txt;
        }




        public static string IFrameVideo(string txt)
        {
            Regex regx = new Regex(
                "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?",
                RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);
            NameValueCollection nvcKey = null;
            string vidKey = string.Empty;
            string theLink = string.Empty;
            int height = 500;
            int width = 400;

            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                height = 200;
            }

            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http://www.youtube.com/watch?"))
                {
                    nvcKey = HttpUtility.ParseQueryString(match.Value.Replace("http://www.youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey, width, height));
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    vidKey = match.Value.Replace("http://youtu.be/", string.Empty);
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey, width, height));

                }
                else
                {
                    // txt = Utilities.MakeLink(txt, "LINK");
                    txt = txt.Replace(match.Value, string.Format(@"<a target=""_blank"" href='{0}'>{1}</a>", match.Value, Messages.Link));
                }
            }


            return txt;
        }


        public static string IFrameVideo(string txt, int height  )
        {
            Regex regx = new Regex(
                "(http|https)://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", 
                RegexOptions.IgnoreCase);

            MatchCollection mactches = regx.Matches(txt);
            NameValueCollection nvcKey = null;
            string vidKey = string.Empty;
            string theLink = string.Empty;
          

            if (HttpContext.Current != null && HttpContext.Current.Request.Browser.IsMobileDevice)
            {
                height = height / 2;
            }

            foreach (Match match in mactches)
            {
                if (match.Value.Contains("http://www.youtube.com/watch?"))
                {
                    nvcKey = HttpUtility.ParseQueryString(match.Value.Replace("http://www.youtube.com/watch?", string.Empty));

                    vidKey = nvcKey["v"];
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey,  0,  height));
                }
                else if (match.Value.Contains("http://youtu.be/"))
                {
                    vidKey = match.Value.Replace("http://youtu.be/", string.Empty);
                    theLink = match.Value;

                    txt = txt.Replace(theLink,
                        string.Format(@"<div class=""you_tube_iframe"">
                        <iframe width=""100%""  height=""{2}"" src=""http://www.youtube.com/embed/{0}?rel=0"" frameborder=""0"" allowfullscreen></iframe></div>", vidKey, 0, height));

                }
                else
                {
                    // txt = Utilities.MakeLink(txt, "LINK");
                    txt = txt.Replace(match.Value, string.Format(@"<a target=""_blank"" href='{0}'>{1}</a>", match.Value, Messages.Link));
                }
            }


            return txt;
        }




        public static string GetRandomJSON(string vidID)
        {
            //Video vid = new Video(vidID, "YT");
            //vid.GetRandomVideo();

            //SongRecord sngr = new SongRecord(vid);

            ////sngr.GetRelatedVideos = false;

            //return sngr.JSONResponse;


            Video vid = new Video("YT", vidID);
            SongRecord sngrcd = new SongRecord(vid);

            PropertyType propTyp = new PropertyType(SiteEnums.PropertyTypeCode.HUMAN);
            MultiProperty mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            string genreName = mp.Name;

            int genreID = mp.MultiPropertyID;

            propTyp = new PropertyType(SiteEnums.PropertyTypeCode.VIDTP);
            mp = new MultiProperty(vid.VideoID, propTyp.PropertyTypeID, SiteEnums.MultiPropertyType.VIDEO);

            int vidTypeID = mp.MultiPropertyID;

            Video vid1 = new Video();

            vid1.GetRelatedVideo(genreID, vidTypeID, vid.VideoID);

            SongRecords sngrcs = new SongRecords();
            SongRecord sngrcd2 = null;

            sngrcd2 = new SongRecord(vid1);
            sngrcs.Add(sngrcd2);

            return sngrcd2.JSONResponse;
        }


        public void GetRelatedVideo(int multiPropertyIDGenre, int multiPropertyIDVidType, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRelatedVideo";

            ADOExtenstion.AddParameter(comm, "multiPropertyIDGenre", multiPropertyIDGenre);
            ADOExtenstion.AddParameter(comm, "multiPropertyIDVidType", multiPropertyIDVidType);
            ADOExtenstion.AddParameter(comm, "currentVideoID", videoID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {

                Get(dt.Rows[0]);
            }
        }
    }

    public class Videos : List<Video>, IGetAll
    {

        public static ArrayList GetDistinctUsers()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetDistinctUsers";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            ArrayList alist = new ArrayList();

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    alist.Add(
                        FromObj.StringFromObj(
                        dr["providerUserKey"]));
                }
            }

            return alist;
        }


        public void GetMostWatchedVideos(int daysBack)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostWatchedVideos";

            ADOExtenstion.AddParameter(comm, "daysBack", daysBack);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid =
                        new Video(
                            FromObj.IntFromObj(dr["videoID"])
                        );

                    this.Add(vid);
                }
            }
        }




        public void GetMostRecentVideos()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetMostRecentVideos";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);

                    this.Add(vid);
                }
            }
        }



        public static DataSet GetAccountCloudByLetter(string letter)
        {

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAccountCloudByLetter";

            ADOExtenstion.AddParameter(comm, "firstLetter", letter);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }


        public void GetVideosForSong(int songID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForSong";

            ADOExtenstion.AddParameter(comm, "songID", songID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);

                    this.Add(vid);
                }
            }
        }


        public void GetAllVideosByUser(string providerUserKey)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllVideosByUser";

            ADOExtenstion.AddParameter(comm, "providerUserKey", providerUserKey);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);

                    this.Add(vid);
                }
            }
        }


        public static Videos GetRandoms(Videos list)
        {
            Random rng = new Random();
            for (int i = list.Count - 1; i > 0; i--)
            {
                int swapIndex = rng.Next(i + 1);
                if (swapIndex != i)
                {
                    //Changed this from "object" to "T" in order to support generics.
                    object tmp = list[swapIndex];
                    list[swapIndex] = list[i];
                    list[i] = (Video)tmp;
                }
            }

            return list;
        }

        //public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source)
        //{
        //    Random rnd = new Random();
        //    return source.OrderBy<T, int>((item) => rnd.Next());
        //}


        public void GetVideosForPropertyType2(int multiPropertyID1, int multiPropertyID2)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForPropertyType2";

            ADOExtenstion.AddParameter(comm, "multiPropertyID1", multiPropertyID1);
            ADOExtenstion.AddParameter(comm, "multiPropertyID2", multiPropertyID2);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(FromObj.IntFromObj(dr["videoID"]));

                    this.Add(vid);
                }
            }
        }



        public void GetVideosForPropertyType(int multiPropertyID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetVideosForPropertyType";

            ADOExtenstion.AddParameter(comm, "multiPropertyID", multiPropertyID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(FromObj.IntFromObj(dr["videoID"]));

                    this.Add(vid);
                }
            }
        }


        #region IGetAll Members

        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllVideos";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;
                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);
                    this.Add(vid);
                }
            }
        }

        #endregion

        public void GetRelatedVideos(int multiPropertyIDGenre, int multiPropertyIDVidType, int videoID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetRelatedVideos";

            ADOExtenstion.AddParameter(comm, "multiPropertyIDGenre", multiPropertyIDGenre);
            ADOExtenstion.AddParameter(comm, "multiPropertyIDVidType", multiPropertyIDVidType);
            ADOExtenstion.AddParameter(comm, "currentVideoID", videoID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Video vid = null;

                foreach (DataRow dr in dt.Rows)
                {
                    vid = new Video(dr);

                    this.Add(vid);
                }
            }
        }


//        public static bool HasResults(int p, int? footageType, int? videoType)
//        {
//             ArrayList allFilters = new ArrayList();

//            if (guitarType != null && guitarType != 0) allFilters.Add(Convert.ToInt32(guitarType));
//            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
//            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
//            if (genreType != null && genreType != 0) allFilters.Add(Convert.ToInt32(genreType));
//            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
//            if (difficultyLevel != null && difficultyLevel != 0) allFilters.Add(Convert.ToInt32(difficultyLevel));
//            if (languge != null && languge != 0) allFilters.Add(Convert.ToInt32(languge));

//            StringBuilder sb = new StringBuilder(100);

//            sb.AppendFormat(@"
// 
//SET NOCOUNT ON; 
//IF EXISTS (
//
//SELECT
//vid.[videoID] 
//FROM Video vid  
//
//  ");
//            if (allFilters.Count > 0)
//            {
//                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

//                if (allFilters.Count > 1)
//                {
//                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

//                    if (allFilters.Count > 2)
//                    {
//                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

//                        if (allFilters.Count > 3)
//                        {
//                            sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

//                            if (allFilters.Count > 4)
//                            {
//                                sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

//                                if (allFilters.Count > 5)
//                                {
//                                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

//                                    if (allFilters.Count > 6)
//                                    {
//                                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
//                                    }
//                                }
//                            }
//                        }


//                    }


//                }
//            }
//            else
//            {
//                sb.Append(@" 
//WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
//            }

//            sb.Append(@" 
//)
//  BEGIN 
//  SELECT 1
//  END
//  ELSE SELECT 0");

//            bool hasResults = false;

//            // get a configured DbCommand object
//            DbCommand comm = DbAct.CreateCommand(true);
            
//            // set the stored procedure name
//            comm.CommandText = sb.ToString();

//            // execute the stored procedure
//            hasResults = DbAct.ExecuteScalar(comm) == "1";

//            return hasResults;
//        }


//        public int GetListFilter(int pageNumber, int resultSize, int? guitarType, int? humanType, int? footageType, int? genreType, int? videoType, int? difficultyLevel, int? languge)
//        {
//            ArrayList allFilters = new ArrayList();

//            if (guitarType != null && guitarType != 0) allFilters.Add(Convert.ToInt32(guitarType));
//            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
//            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
//            if (genreType != null && genreType != 0) allFilters.Add(Convert.ToInt32(genreType));
//            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
//            if (difficultyLevel != null && difficultyLevel != 0) allFilters.Add(Convert.ToInt32(difficultyLevel));
//            if (languge != null && languge != 0) allFilters.Add(Convert.ToInt32(languge));
            
            


//            StringBuilder sb = new StringBuilder(100);

//            sb.AppendFormat(@"
//
//DECLARE  @PageIndex INT = {0}
//DECLARE  @PageSize INT = {1}
//
//SET NOCOUNT ON; 
//SELECT ROW_NUMBER() OVER 
//( ORDER BY publishdate DESC  ) AS RowNumber 
//,vid.[videoID], [videoKey], [providerKey], [providerUserKey],
//[providerCode], vid.[updatedByUserID], vid.[createDate], 
//vid.[updateDate], 
//vid.[createdByUserID], [isHidden], [isEnabled], [statusID], [duration], [intro], 
//[lengthFromStart], [volumeLevel], enableTrim, publishdate
//INTO #Results
//FROM Video vid  ", pageNumber, resultSize);

//            if (allFilters.Count > 0)
//            {
//                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

//                if (allFilters.Count > 1)
//                {
//                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

//                    if (allFilters.Count > 2)
//                    {
//                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

//                        if (allFilters.Count > 3)
//                        {
//                            sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

//                            if (allFilters.Count > 4)
//                            {
//                                sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

//                                if (allFilters.Count > 5)
//                                {
//                                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

//                                    if (allFilters.Count > 6)
//                                    {
//                                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on  mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
//                                    }
//                                }
//                            }
//                        }


//                    }


//                }
//            }
//            else 
//            {
//                sb.Append(@" 
//WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
//            }

//            sb.Append(@" 
//SELECT  COUNT(*) as 'totalResults'
//FROM #Results
//
//SELECT * FROM #Results
//WHERE RowNumber BETWEEN(@PageIndex -1)
//* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
//
//DROP TABLE #Results");

//            int totalResults = 0;

//            // get a configured DbCommand object
//            DbCommand comm = DbAct.CreateCommand(true);

//            // set the stored procedure name
//            comm.CommandText = sb.ToString();
//            //comm.CommandType = CommandType.Text;

//            // execute the stored procedure
//            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

//            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
//            {
//                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

//                Video v1 = null;

//                foreach (DataRow dr in ds.Tables[1].Rows)
//                {
//                    v1 = new Video(dr);
//                    this.Add(v1);
//                }
//            }

//            return totalResults;
//        }

        public int GetListFilter(int pageNumber, int resultSize, int? humanType, int? footageType, int? videoType)
        {
            ArrayList allFilters = new ArrayList();

            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));


            StringBuilder sb = new StringBuilder(100);

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
                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on  mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
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

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand(true);

            // set the stored procedure name
            comm.CommandText = sb.ToString();
            //comm.CommandType = CommandType.Text;

            // execute the stored procedure
            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

                Video v1 = null;

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    v1 = new Video(dr);
                    this.Add(v1);
                }
            }

            return totalResults;
        }
    


        public static bool HasResults(  int? footageType,   int? videoType, int? personType)
        {
            ArrayList allFilters = new ArrayList();

            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
            if (personType != null && personType != 0) allFilters.Add(Convert.ToInt32(personType));

            StringBuilder sb = new StringBuilder(100);

            sb.AppendFormat(@"
 
SET NOCOUNT ON; 
IF EXISTS (

SELECT
vid.[videoID] 
FROM Video vid  

  ");
            if (allFilters.Count > 0)
            {
                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

                if (allFilters.Count > 1)
                {
                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

                    if (allFilters.Count > 2)
                    {
                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

                        if (allFilters.Count > 3)
                        {
                            sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

                            if (allFilters.Count > 4)
                            {
                                sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

                                if (allFilters.Count > 5)
                                {
                                    sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

                                    if (allFilters.Count > 6)
                                    {
                                        sb.AppendFormat(@"
and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
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

            bool hasResults = false;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand(true);
            
            // set the stored procedure name
            comm.CommandText = sb.ToString();

            // execute the stored procedure
            hasResults = DbAct.ExecuteScalar(comm) == "1";

            return hasResults;
        }



//        public int GetListFilter(int pageNumber, int resultSize, int? guitarType, int? humanType, int? footageType, int? genreType, int? videoType, int? difficultyLevel, int? languge)
//        {
//            ArrayList allFilters = new ArrayList();

//            if (guitarType != null && guitarType != 0) allFilters.Add(Convert.ToInt32(guitarType));
//            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
//            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
//            if (genreType != null && genreType != 0) allFilters.Add(Convert.ToInt32(genreType));
//            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));
//            if (difficultyLevel != null && difficultyLevel != 0) allFilters.Add(Convert.ToInt32(difficultyLevel));
//            if (languge != null && languge != 0) allFilters.Add(Convert.ToInt32(languge));
            
            


//            StringBuilder sb = new StringBuilder(100);

//            sb.AppendFormat(@"
//
//DECLARE  @PageIndex INT = {0}
//DECLARE  @PageSize INT = {1}
//
//SET NOCOUNT ON; 
//SELECT ROW_NUMBER() OVER 
//( ORDER BY publishdate DESC  ) AS RowNumber 
//,vid.[videoID], [videoKey], [providerKey], [providerUserKey],
//[providerCode], vid.[updatedByUserID], vid.[createDate], 
//vid.[updateDate], 
//vid.[createdByUserID], [isHidden], [isEnabled], [statusID], [duration], [intro], 
//[lengthFromStart], [volumeLevel], enableTrim, publishdate
//INTO #Results
//FROM Video vid  ", pageNumber, resultSize);

//            if (allFilters.Count > 0)
//            {
//                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

//                if (allFilters.Count > 1)
//                {
//                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

//                    if (allFilters.Count > 2)
//                    {
//                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

//                        if (allFilters.Count > 3)
//                        {
//                            sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

//                            if (allFilters.Count > 4)
//                            {
//                                sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

//                                if (allFilters.Count > 5)
//                                {
//                                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

//                                    if (allFilters.Count > 6)
//                                    {
//                                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on  mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
//                                    }
//                                }
//                            }
//                        }


//                    }


//                }
//            }
//            else 
//            {
//                sb.Append(@" 
//WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
//            }

//            sb.Append(@" 
//SELECT  COUNT(*) as 'totalResults'
//FROM #Results
//
//SELECT * FROM #Results
//WHERE RowNumber BETWEEN(@PageIndex -1)
//* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
//
//DROP TABLE #Results");

//            int totalResults = 0;

//            // get a configured DbCommand object
//            DbCommand comm = DbAct.CreateCommand(true);

//            // set the stored procedure name
//            comm.CommandText = sb.ToString();
//            //comm.CommandType = CommandType.Text;

//            // execute the stored procedure
//            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

//            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
//            {
//                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

//                Video v1 = null;

//                foreach (DataRow dr in ds.Tables[1].Rows)
//                {
//                    v1 = new Video(dr);
//                    this.Add(v1);
//                }
//            }

//            return totalResults;
//        }

//        public int GetListFilter(int pageNumber, int resultSize, int? humanType, int? footageType, int? videoType)
//        {
//            ArrayList allFilters = new ArrayList();

//            if (humanType != null && humanType != 0) allFilters.Add(Convert.ToInt32(humanType));
//            if (footageType != null && footageType != 0) allFilters.Add(Convert.ToInt32(footageType));
//            if (videoType != null && videoType != 0) allFilters.Add(Convert.ToInt32(videoType));


//            StringBuilder sb = new StringBuilder(100);

//            sb.AppendFormat(@"
//
//DECLARE  @PageIndex INT = {0}
//DECLARE  @PageSize INT = {1}
//
//SET NOCOUNT ON; 
//SELECT ROW_NUMBER() OVER 
//( ORDER BY publishdate DESC  ) AS RowNumber 
//,vid.[videoID], [videoKey], [providerKey], [providerUserKey],
//[providerCode], vid.[updatedByUserID], vid.[createDate], 
//vid.[updateDate], 
//vid.[createdByUserID], [isHidden], [isEnabled], [statusID], [duration], [intro], 
//[lengthFromStart], [volumeLevel], enableTrim, publishdate
//INTO #Results
//FROM Video vid  ", pageNumber, resultSize);

//            if (allFilters.Count > 0)
//            {
//                sb.AppendFormat(@" INNER JOIN [MultiPropertyVideo] mpv ON mpv.videoID = vid.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' AND MPV.multiPropertyID = {0}", allFilters[0]);

//                if (allFilters.Count > 1)
//                {
//                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid2.[videoID] FROM Video vid2 INNER JOIN [MultiPropertyVideo] mpv2 on  mpv2.videoID = vid2.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv2.multiPropertyID = {0} )", allFilters[1]);

//                    if (allFilters.Count > 2)
//                    {
//                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid3.[videoID] FROM Video vid3 INNER JOIN [MultiPropertyVideo] mpv3 on  mpv3.videoID = vid3.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv3.multiPropertyID = {0} )", allFilters[2]);

//                        if (allFilters.Count > 3)
//                        {
//                            sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid4.[videoID] FROM Video vid4 INNER JOIN [MultiPropertyVideo] mpv4 on  mpv4.videoID = vid4.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv4.multiPropertyID = {0} )", allFilters[3]);

//                            if (allFilters.Count > 4)
//                            {
//                                sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid5.[videoID] FROM Video vid5 INNER JOIN [MultiPropertyVideo] mpv5 on  mpv5.videoID = vid5.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv5.multiPropertyID = {0} )", allFilters[4]);

//                                if (allFilters.Count > 5)
//                                {
//                                    sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid6.[videoID] FROM Video vid6 INNER JOIN [MultiPropertyVideo] mpv6 on  mpv6.videoID = vid6.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv6.multiPropertyID = {0} )", allFilters[5]);

//                                    if (allFilters.Count > 6)
//                                    {
//                                        sb.AppendFormat(@"
//and  vid.videoID in ( SELECT vid7.[videoID] FROM Video vid7 INNER JOIN [MultiPropertyVideo] mpv7 on  mpv7.videoID = vid7.VideoID WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' and mpv7.multiPropertyID = {0} )", allFilters[6]);
//                                    }
//                                }
//                            }
//                        }


//                    }


//                }
//            }
//            else
//            {
//                sb.Append(@" 
//WHERE isHidden = 0 AND isEnabled = 1 AND providerCode = 'YT' ");
//            }

//            sb.Append(@" 
//SELECT  COUNT(*) as 'totalResults'
//FROM #Results
//
//SELECT * FROM #Results
//WHERE RowNumber BETWEEN(@PageIndex -1)
//* @PageSize + 1 AND(((@PageIndex -1) * @PageSize + 1) + @PageSize) - 1
//
//DROP TABLE #Results");

//            int totalResults = 0;

//            // get a configured DbCommand object
//            DbCommand comm = DbAct.CreateCommand(true);

//            // set the stored procedure name
//            comm.CommandText = sb.ToString();
//            //comm.CommandType = CommandType.Text;

//            // execute the stored procedure
//            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

//            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count > 0)
//            {
//                totalResults = FromObj.IntFromObj(ds.Tables[0].Rows[0]["totalResults"]);

//                Video v1 = null;

//                foreach (DataRow dr in ds.Tables[1].Rows)
//                {
//                    v1 = new Video(dr);
//                    this.Add(v1);
//                }
//            }

//            return totalResults;
//        }

     
    }
}
