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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using BootBaronLib.BaseTypes;
using BootBaronLib.DAL;
using BootBaronLib.Interfaces;
using BootBaronLib.Operational;
using BootBaronLib.Operational.Converters;
using BootBaronLib.Resources;


namespace BootBaronLib.AppSpec.DasKlub.BOL.UserContent
{
    public class Content : BaseIUserLogCRUD, IUnorderdListItem
    {
        #region properties

        private int _contentID = 0;

        public int ContentID
        {
            get { return _contentID; }
            set { _contentID = value; }
        }

        private string _language = string.Empty;

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        private int _siteDomainID = 0;

        public int SiteDomainID
        {
            get { return _siteDomainID; }
            set { _siteDomainID = value; }
        }

        private bool _isEnabled = false;

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; }
        }

        private char _currentStatus = char.MinValue;

        public char CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }

        private string _outboundURL = string.Empty;

        public string OutboundURL
        {
            get { return _outboundURL; }
            set { _outboundURL = value; }
        }


        private string _contentVideoURL = string.Empty;

        [Display(ResourceType = typeof(Resources.Messages), Name = "Video")]
        public string ContentVideoURL
        {
            get { return _contentVideoURL; }
            set { _contentVideoURL = value; }
        }

        private string _contentVideoURL2 = string.Empty;

        [Display(ResourceType = typeof(Resources.Messages), Name = "Video")]
        public string ContentVideoURL2
        {
            get
            {
                if (_contentVideoURL2 == null) return null;

                return _contentVideoURL2.Trim();
            }

            set
            {
                _contentVideoURL2 = value;
            }
        }



        private string _contentPhotoURL = string.Empty;

        [Display(ResourceType = typeof(Resources.Messages), Name = "Photo")]
        public string ContentPhotoURL
        {
            get { return _contentPhotoURL; }
            set { _contentPhotoURL = value; }
        }

        private string _contentPhotoThumbURL = string.Empty;


        [Display(ResourceType = typeof(Resources.Messages), Name = "Photo")]
        public string ContentPhotoThumbURL
        {
            get { return _contentPhotoThumbURL; }
            set { _contentPhotoThumbURL = value; }
        }


        private string _contentKey = string.Empty;

        public string ContentKey
        {
            get { return _contentKey; }
            set { _contentKey = value; }
        }


        private string _title = string.Empty;

        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "Title")]
        public string Title
        {
            get {
                if (!string.IsNullOrWhiteSpace(_title)) return _title.Trim();
                
                return _title; }
            set { _title = value; }
        }

        private string _detail = string.Empty;

        [Required(ErrorMessageResourceName = "Required",
    ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "Detail")]
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private string _metaDescription = string.Empty;




        [Required(ErrorMessageResourceName = "Required",
    ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "MetaDescription")]
        public string MetaDescription
        {
            get { return _metaDescription; }
            set { _metaDescription = value; }
        }
        private string _metaKeywords = string.Empty;

        [Required(ErrorMessageResourceName = "Required",
ErrorMessageResourceType = typeof(Resources.Messages))]
        [Display(ResourceType = typeof(Resources.Messages), Name = "Keywords")]
        public string MetaKeywords
        {
            get { return _metaKeywords; }
            set { _metaKeywords = value; }
        }


        private double _rating = 0;

        public double Rating
        {
            get { return _rating; }
            set { _rating = value; }
        }

        private int _contentTypeID = 0;

        public int ContentTypeID
        {
            get
            {
                // HACK
                if (_contentTypeID == 0) _contentTypeID = 2;
                return _contentTypeID;
            }
            set { _contentTypeID = value; }
        }


        private DateTime _releaseDate = DateTime.MinValue;


        [Display(ResourceType = typeof(Resources.Messages), Name = "ReleaseDate")]
        [DataType(DataType.Time)]
        [DisplayFormatAttribute(ApplyFormatInEditMode = true, DataFormatString = "{0:s}")]
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; }
        }

        #endregion

        #region constants

        public const string CONTENT_IMAGE_PATH = "~/Content/contentinfo/";

        #endregion

        #region constructors

        public Content(string key)
        {
            GetContentByKey(key);
        }

        public Content(DataRow dr)
        {
            Get(dr);
        }

        public Content()
        {
            // TODO: Complete member initialization
        }

        public Content(int p)
        {
            // TODO: Complete member initialization
            Get(p);
        }

        #endregion

        #region methods



        public void GetPreviousNewsLang(DateTime createDateCurrent, string language)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousNewsLang";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);
            ADOExtenstion.AddParameter(comm, "language", language);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }

        }


        public void GetPreviousNews(DateTime createDateCurrent )
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousNews";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);
 

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }

        }


        public void GetNextNewsLang(DateTime createDateCurrent, string language)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextNews";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);
            ADOExtenstion.AddParameter(comm, "language", language);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public void GetNextNews(DateTime createDateCurrent)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetNextNews";

            ADOExtenstion.AddParameter(comm, "createDateCurrent", createDateCurrent);
 

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override void Get(int uniqueID)
        {
            this.ContentID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentByID";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentID), ContentID);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }

        public override bool Update()
        {
            return Set() > 0;
        }

        public override int Create()
        {
            return Set();
        }

        public int Set()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_SetContent";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Title), Title);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Detail), Detail);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MetaDescription), MetaDescription);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.MetaKeywords), MetaKeywords);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentTypeID), ContentTypeID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CreatedByUserID), CreatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.UpdatedByUserID), UpdatedByUserID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ReleaseDate), ReleaseDate);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentID), ContentID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentKey), ContentKey);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Rating), Rating);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentPhotoURL), ContentPhotoURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentPhotoThumbURL), ContentPhotoThumbURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.OutboundURL), OutboundURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.SiteDomainID), SiteDomainID);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.IsEnabled), IsEnabled);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.CurrentStatus), CurrentStatus);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentVideoURL), ContentVideoURL);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.Language), Language);
            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentVideoURL2), ContentVideoURL2);

            // execute the stored procedure
            string gg = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(gg))
                this.ContentID = Convert.ToInt32(gg);
            return this.ContentID;

        }

        public void GetContentByKey(string contentKey)
        {
            this.ContentKey = contentKey;

            if (!string.IsNullOrEmpty(contentKey))
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetContentByKey";

                ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentKey), ContentKey);

                // execute the stored procedure
                DataTable dt = DbAct.ExecuteSelectCommand(comm);

                // was something returned?
                if (dt != null && dt.Rows.Count > 0)
                {
                    Get(dt.Rows[0]);
                }
            }
        }


        public override bool Delete()
        {
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_DeleteFromContentByID";

            ADOExtenstion.AddParameter(comm, StaticReflection.GetMemberName<string>(x => this.ContentID), ContentID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            this.ContentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentID)]);
            this.Title = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Title)]);
            this.Detail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Detail)]);
            this.MetaDescription = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.MetaDescription)]);
            this.MetaKeywords = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.MetaKeywords)]);
            this.ContentTypeID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentTypeID)]);
            this.ReleaseDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ReleaseDate)]);
            this.ContentKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentKey)]);
            this.Rating = FromObj.DoubleFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Rating)]);
            this.ContentPhotoThumbURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentPhotoThumbURL)]);
            this.ContentPhotoURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentPhotoURL)]);
            this.OutboundURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.OutboundURL)]);
            this.SiteDomainID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => this.SiteDomainID)]);
            this.IsEnabled = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => this.IsEnabled)]);
            this.CurrentStatus = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => this.CurrentStatus)]);
            this.ContentVideoURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentVideoURL)]);
            this.Language = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.Language)]);
            this.ContentVideoURL2 = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => this.ContentVideoURL2)]);
        }

        #endregion

        #region non-db properties

        private ContentComment _reply = null;

        public ContentComment Reply
        {
            get { return _reply; }
            set { _reply = value; }
        }



        public IList<ContentComment> Comments
        {
            get
            {
                ContentComments ccoms = new ContentComments();

                ccoms.GetCommentsForContent(this.ContentID, Enums.SiteEnums.CommentStatus.C);

                return ccoms;
            }
        }

        public string ContentDetail
        {
            get
            {
                if (this.Detail == null) return string.Empty;

                return this.Detail.Replace("\r\n", "<br />");
            }
        }

        private Uri _urlTo = null;

        public Uri UrlTo
        {
            get
            {
                string theURL = "http://" +

                    HttpContext.Current.Request.Url.Authority;

                theURL += "/news/" + this.ContentKey;

                _urlTo = new Uri(theURL);
                return _urlTo;
            }
            set { _urlTo = value; }
        }




        public string FullContentImageURL
        {
            get
            {
                if (string.IsNullOrEmpty(this.ContentPhotoURL))
                {
                    return System.Web.VirtualPathUtility.ToAbsolute("~/content/contentinfo/default.png");
                }
                else
                {
                    return System.Web.VirtualPathUtility.ToAbsolute(this.ContentPhotoURL);
                }
            }
        }

        public string RatingAmount
        {
            get
            {
                return (this.Rating * 15).ToString();
            }
        }


        public string TagLinks
        {
            get
            {
                if (string.IsNullOrEmpty(this.MetaKeywords)) return string.Empty;

                StringBuilder sb = new StringBuilder(100);

                string[] keywords = this.MetaKeywords.Split(',');

                int keywordCount = 0;

                foreach (string keyword in keywords)
                {
                    keywordCount++;

                    sb.Append(string.Format(@"<a href=""{0}"">{1}</a>",

                        VirtualPathUtility.ToAbsolute("~/news/tag/" +
                        FromString.URLKey(keyword)
                        )
                         , keyword));

                    if (keywordCount < keywords.Length)
                    {
                        sb.Append(", ");
                    }

                }

                return sb.ToString();
            }

        }

        public string ToUnorderdListItem
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                sb.Append(@"<li>");


                sb.Append(@"<div class=""row"">");
                
                sb.Append(@"<div class=""span2"">");

                sb.AppendFormat(@"<a class=""m_over"" href=""{0}""><img src=""{1}""  title=""{2}"" alt=""{2}""></a>", this.UrlTo,
                   Utilities.S3ContentPath(this.ContentPhotoThumbURL), this.Title);
                sb.Append(@"</div>");


                sb.Append(@"<div class=""span3"">");
                sb.AppendFormat(@"<h4 class=""title""><a href=""{0}"">{1}</a></h4>", this.UrlTo, this.Title);

                sb.Append(@"<i>");
                sb.Append(Messages.Published);
                sb.Append(@": ");
                sb.Append(BootBaronLib.Operational.Utilities.TimeElapsedMessage(ReleaseDate));
                sb.Append(@"</i>");

                sb.Append(@"<br />");

                sb.Append(Messages.Language);
                sb.Append(@": ");

                sb.AppendFormat(@"<span class=""badge  badge-inverse"" title=""{1}"">{0}</span> ",
                    this.Language.ToUpper(), Utilities.GetLanguageNameForCode(this.Language));

                sb.Append(@"<br />");

                if (this.CreatedByUserID > 0)
                {
                    UserAccount ua = new UserAccount(this.CreatedByUserID);
                    UserAccountDetail uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(ua.UserAccountID);

                    sb.Append(Messages.From);
                    sb.Append(@": ");

                    sb.AppendFormat(@"<a href=""{0}"">{1}</a> &nbsp;", ua.UrlTo.ToString(), ua.UserName);
                   

                    sb.AppendFormat(@"<div title=""{0}"" class=""sprites sprite-{1}_small""></div>", uad.CountryName, uad.Country);
              
 
                    sb.Append(@"<br />");
                }

                sb.Append(Messages.Tagged);
                sb.Append(": ");
                sb.Append(TagLinks);

                sb.Append(@"<br />");

                sb.Append(@"<p>");
                sb.Append(FromString.Truncate(this.MetaDescription, 300));
                sb.Append(@"</p>");
                
                sb.AppendFormat (@"<a href=""{0}"" class=""readmore"">{1} »</a>", this.UrlTo, Messages.ReadMore );

                sb.Append(@"<br />");

                if (this.Comments != null && this.Comments.Count > 0)
                {
                    sb.AppendFormat( @"<a href=""{0}#content_comments"">{1} : {2}</a>", 
                        this.UrlTo,
                        Messages.Comments,
                        this.Comments.Count.ToString() );
                }
                else
                {
                    sb.Append(@"<i>");
                    sb.Append(Messages.NoComments);
                    sb.Append(@"</i>");
                }

                
                sb.Append(@"</div>");
                sb.Append(@"</div>");

                sb.Append(@"<hr />");

                sb.Append(@"</li>");

                return sb.ToString();
            }
        }

        #endregion
    }

    public class Contents : List<Content>, IGetAll, IUnorderdList
    {


        public static Dictionary<string, string> GetDistinctNewsLanguages()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetDistinctNewsLanguages";
 

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            Dictionary<string, string> dict = new Dictionary<string, string>();

            string lang = string.Empty;

            foreach (DataRow dr in dt.Rows)
            {
                lang = FromObj.StringFromObj(dr["language"]);
                if (!string.IsNullOrWhiteSpace(lang))
                {
                    dict.Add(lang, Utilities.GetLanguageNameForCode(lang));
                }
            }

            return dict;
        }


        public static DataSet GetContentTags(string language)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetContentTags";

            ADOExtenstion.AddParameter(comm, "language", language);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            DataSet ds = new DataSet();

            string[] keywords = null;

            Dictionary<string, int> keywordsDict = new Dictionary<string, int>();

            foreach (DataRow dr in dt.Rows)
            {
                keywords = FromObj.StringFromObj(dr["metaKeywords"]).Split(',');

                foreach (string keyword in keywords)
                {
                    string word = keyword.Trim().ToLower();

                    if (keywordsDict.ContainsKey(word))
                    {
                        int timeFound = keywordsDict[word];
                        keywordsDict.Remove(word);
                        keywordsDict.Add(word, timeFound + 1);
                    }
                    else
                    {
                        keywordsDict.Add(word, 1);
                    }
                }
            }

            keywordsDict = (from entry in keywordsDict orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            DataTable contentTagList = new DataTable();

            contentTagList.Columns.Add("keyword_id", typeof(int)); ;
            contentTagList.Columns.Add("keyword_value", typeof(string));
            contentTagList.Columns.Add("keyword_count", typeof(int));
            contentTagList.Columns.Add("keyword_url", typeof(string));

            int keyword_id = 1;

            foreach (KeyValuePair<string, int> tag in keywordsDict)
            {
                keyword_id++;

                contentTagList.Rows.Add(keyword_id, tag.Key, tag.Value, VirtualPathUtility.ToAbsolute("~/news/tag/" +
                        FromString.URLKey(tag.Key)
                        ));
            }

            ds.Tables.Add(contentTagList);

            return ds;
        }



        public static DataSet GetContentTagsAll( )
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetContentTagsAll";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            DataSet ds = new DataSet();

            string[] keywords = null;

            Dictionary<string, int> keywordsDict = new Dictionary<string, int>();

            foreach (DataRow dr in dt.Rows)
            {
                keywords = FromObj.StringFromObj(dr["metaKeywords"]).Split(',');

                foreach (string keyword in keywords)
                {
                    string word = keyword.Trim().ToLower();

                    if (keywordsDict.ContainsKey(word))
                    {
                        int timeFound = keywordsDict[word];
                        keywordsDict.Remove(word);
                        keywordsDict.Add(word, timeFound + 1);
                    }
                    else
                    {
                        keywordsDict.Add(word, 1);
                    }
                }
            }


            Dictionary<string, int> keywordsDict2 = new Dictionary<string, int>();

            foreach (KeyValuePair<string, int> tag in keywordsDict)
            {
                if (tag.Value > 1) keywordsDict2.Add(tag.Key, tag.Value);
            }

            keywordsDict2 = (from entry in keywordsDict2 orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key, pair => pair.Value);

            DataTable contentTagList = new DataTable();

            contentTagList.Columns.Add("keyword_id", typeof(int)); ;
            contentTagList.Columns.Add("keyword_value", typeof(string));
            contentTagList.Columns.Add("keyword_count", typeof(int));
            contentTagList.Columns.Add("keyword_url", typeof(string));

            int keyword_id = 1;

            foreach (KeyValuePair<string, int> tag in keywordsDict2)
            {
                keyword_id++;

                contentTagList.Rows.Add(keyword_id, tag.Key, tag.Value, VirtualPathUtility.ToAbsolute("~/news/tag/" +
                        FromString.URLKey(tag.Key)
                        ));
            }

            ds.Tables.Add(contentTagList);

            return ds;
        }


        #region IGetAll Members


        public void GetAllActiveContent()
        {
            Contents cntss = new Contents();
            cntss.GetAll();

            this.Clear();

            DateTime currentTime = DateTime.UtcNow;

            foreach (Content cnt in cntss)
            {
                if (cnt.ReleaseDate < currentTime) // forget about the future ones
                {
                    this.Add(cnt);
                }
            }

        }


        public void GetAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetAllContent";

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Content cnt = null;

                foreach (DataRow dr in dt.Rows)
                {
                    cnt = new Content(dr);

                    this.Add(cnt);
                }
            }
        }


        public void GetContentForContentTypeID(int contentTypeID, int siteDomainID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentForContentTypeID";

            ADOExtenstion.AddParameter(comm, "contentTypeID", contentTypeID);
            ADOExtenstion.AddParameter(comm, "siteDomainID", siteDomainID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Content cnt = null;
                DateTime currentTime = DateTime.UtcNow;

                foreach (DataRow dr in dt.Rows)
                {
                    cnt = new Content(dr);

                    if (cnt.ReleaseDate < currentTime) // forget about the future ones
                    {
                        this.Add(cnt);
                    }
                }
            }

        }

        #endregion

        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }

        public string ToUnorderdList
        {
            get
            {
                StringBuilder sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul>");

                int i = 0;

                foreach (Content con in this)
                {
                    //if (i % 2 == 1)
                    //{
                    //    sb.Append(con.ToUnorderdListItem.Replace(@"<li class=""review_item"">",
                    //        @"<li class=""review_item alt_review"">"));
                    //}
                    //else
                    //{
                    //    sb.Append(con.ToUnorderdListItem);
                    //}

                    sb.Append(con.ToUnorderdListItem);

                    i++;
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }



        public int GetContentPageWise(int pageIndex, int pageSize, string language)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentPageWise";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
            ADOExtenstion.AddParameter(comm, "language", language);

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Content content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new Content(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }


        public int GetContentPageWiseAll(int pageIndex, int pageSize )
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentPageWiseAll";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
 

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Content content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new Content(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }

        public int  GetContentAllPageWiseKey(int pageIndex, int pageSize, string key)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentAllPageWiseKey";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            ADOExtenstion.AddParameter(comm, "PageIndex", pageIndex);
            ADOExtenstion.AddParameter(comm, "PageSize", pageSize);
            ADOExtenstion.AddParameter(comm, "key", key);
 

            DataSet ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            int recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Content content = null;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    content = new Content(dr);
                    this.Add(content);
                }
            }

            return recordCount;
        }





        public void GetContentForUser(int createdByUserID)
        {

             // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentForUser";

            ADOExtenstion.AddParameter(comm, "createdByUserID", createdByUserID);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Content cnt = null;

                foreach (DataRow dr in dt.Rows)
                {
                    cnt = new Content(dr);

                    this.Add(cnt);
                }
            }
        }

       
    }
}
