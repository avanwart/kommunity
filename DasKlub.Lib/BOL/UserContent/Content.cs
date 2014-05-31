using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BLL;
using DasKlub.Lib.DAL;
using DasKlub.Lib.Interfaces;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Lib.BOL.UserContent
{
    public class Content : BaseIUserLogCRUD, IUnorderdListItem
    {
        #region properties

        private string _contentKey = string.Empty;
        private string _contentPhotoThumbURL = string.Empty;
        private string _contentPhotoURL = string.Empty;
        private int _contentTypeID;
        private string _contentVideoURL = string.Empty;
        private string _contentVideoURL2 = string.Empty;
        private char _currentStatus = char.MinValue;
        private string _detail = string.Empty;
        private string _language = string.Empty;
        private string _metaDescription = string.Empty;
        private string _metaKeywords = string.Empty;
        private string _outboundURL = string.Empty;
        private DateTime _releaseDate = DateTime.MinValue;
        private string _title = string.Empty;
        public int ContentID { get; set; }

        public string Language
        {
            get { return _language; }
            set { _language = value; }
        }

        public int SiteDomainID { get; set; }

        public bool IsEnabled { get; set; }

        public char CurrentStatus
        {
            get { return _currentStatus; }
            set { _currentStatus = value; }
        }

        public string OutboundURL
        {
            get { return _outboundURL; }
            set { _outboundURL = value; }
        }


        [Display(ResourceType = typeof (Messages), Name = "Video")]
        public string ContentVideoURL
        {
            get { return _contentVideoURL; }
            set { _contentVideoURL = value; }
        }

        [Display(ResourceType = typeof (Messages), Name = "Video")]
        public string ContentVideoURL2
        {
            get
            {
                if (_contentVideoURL2 == null) return null;

                return _contentVideoURL2.Trim();
            }

            set { _contentVideoURL2 = value; }
        }


        [Display(ResourceType = typeof (Messages), Name = "Photo")]
        public string ContentPhotoURL
        {
            get { return _contentPhotoURL; }
            set { _contentPhotoURL = value; }
        }


        [Display(ResourceType = typeof (Messages), Name = "Photo")]
        public string ContentPhotoThumbURL
        {
            get { return _contentPhotoThumbURL; }
            set { _contentPhotoThumbURL = value; }
        }


        public string ContentKey
        {
            get { return _contentKey; }
            set { _contentKey = value; }
        }


        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Title")]
        public string Title
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_title)) return _title.Trim();

                return _title;
            }
            set { _title = value; }
        }

        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Detail")]
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }


        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "MetaDescription")]
        public string MetaDescription
        {
            get { return _metaDescription; }
            set { _metaDescription = value; }
        }

        [Required(ErrorMessageResourceName = "Required",
            ErrorMessageResourceType = typeof (Messages))]
        [Display(ResourceType = typeof (Messages), Name = "Keywords")]
        public string MetaKeywords
        {
            get { return _metaKeywords; }
            set { _metaKeywords = value; }
        }


        public double Rating { get; set; }

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


        [Display(ResourceType = typeof (Messages), Name = "ReleaseDate")]
        [DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:u}")]
        public DateTime ReleaseDate
        {
            get { return _releaseDate; }
            set { _releaseDate = value; }
        }

        #endregion

        #region constants

        public const string CONTENT_IMAGE_PATH = "~/content/contentinfo/";

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
            
        }

        public Content(int p)
        {
           
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

            comm.AddParameter("createDateCurrent", createDateCurrent);
            comm.AddParameter("language", language);

            // execute the stored procedure
            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt != null && dt.Rows.Count > 0)
            {
                Get(dt.Rows[0]);
            }
        }


        public void GetPreviousNews(DateTime createDateCurrent)
        {
            if (createDateCurrent == DateTime.MinValue) return;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetPreviousNews";

            comm.AddParameter("createDateCurrent", createDateCurrent);


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

            comm.AddParameter("createDateCurrent", createDateCurrent);
            comm.AddParameter("language", language);

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

            comm.AddParameter("createDateCurrent", createDateCurrent);


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
            ContentID = uniqueID;

            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentByID";

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentID), ContentID);

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
            HttpRuntime.Cache.DeleteCacheObj(
                string.Concat(
                    "news-",
                    ContentKey));

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

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Title), Title);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Detail), Detail);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MetaDescription), MetaDescription);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => MetaKeywords), MetaKeywords);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentTypeID), ContentTypeID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CreatedByUserID), CreatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => UpdatedByUserID), UpdatedByUserID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ReleaseDate), ReleaseDate);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentID), ContentID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentKey), ContentKey);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Rating), Rating);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentPhotoURL), ContentPhotoURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentPhotoThumbURL), ContentPhotoThumbURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => OutboundURL), OutboundURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => SiteDomainID), SiteDomainID);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => IsEnabled), IsEnabled);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => CurrentStatus), CurrentStatus);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentVideoURL), ContentVideoURL);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => Language), Language);
            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentVideoURL2), ContentVideoURL2);

            // execute the stored procedure
            string gg = DbAct.ExecuteScalar(comm);

            if (!string.IsNullOrEmpty(gg))
                ContentID = Convert.ToInt32(gg);
            return ContentID;
        }

        public void GetContentByKey(string contentKey)
        {
            ContentKey = contentKey;

            if (!string.IsNullOrEmpty(contentKey))
            {
                // get a configured DbCommand object
                DbCommand comm = DbAct.CreateCommand();
                // set the stored procedure name
                comm.CommandText = "up_GetContentByKey";

                comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentKey), ContentKey);

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

            comm.AddParameter(StaticReflection.GetMemberName<string>(x => ContentID), ContentID);

            return DbAct.ExecuteNonQuery(comm) > 0;
        }

        public override void Get(DataRow dr)
        {
            base.Get(dr);

            ContentID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentID)]);
            Title = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Title)]);
            Detail = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Detail)]);
            MetaDescription = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => MetaDescription)]);
            MetaKeywords = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => MetaKeywords)]);
            ContentTypeID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentTypeID)]);
            ReleaseDate = FromObj.DateFromObj(dr[StaticReflection.GetMemberName<string>(x => ReleaseDate)]);
            ContentKey = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentKey)]);
            Rating = FromObj.DoubleFromObj(dr[StaticReflection.GetMemberName<string>(x => Rating)]);
            ContentPhotoThumbURL =
                FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentPhotoThumbURL)]);
            ContentPhotoURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentPhotoURL)]);
            OutboundURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => OutboundURL)]);
            SiteDomainID = FromObj.IntFromObj(dr[StaticReflection.GetMemberName<string>(x => SiteDomainID)]);
            IsEnabled = FromObj.BoolFromObj(dr[StaticReflection.GetMemberName<string>(x => IsEnabled)]);
            CurrentStatus = FromObj.CharFromObj(dr[StaticReflection.GetMemberName<string>(x => CurrentStatus)]);
            ContentVideoURL = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentVideoURL)]);
            Language = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => Language)]);
            ContentVideoURL2 = FromObj.StringFromObj(dr[StaticReflection.GetMemberName<string>(x => ContentVideoURL2)]);
        }

        #endregion

        #region non-db properties

        private Uri _urlTo;
        public ContentComment Reply { get; set; }


        public IList<ContentComment> Comments
        {
            get
            {
                var ccoms = new ContentComments();

                ccoms.GetCommentsForContent(ContentID, SiteEnums.CommentStatus.C);

                return ccoms;
            }
        }

        public string ContentDetail
        {
            get
            {
                if (Detail == null) return string.Empty;

                return Detail.Replace(Environment.NewLine, "<br />");
            }
        }

        public Uri UrlTo
        {
            get
            {
                string theURL = string.Concat("http://", 
                                              HttpContext.Current.Request.Url.Authority);

                theURL += string.Concat("/news/", ContentKey);

                _urlTo = new Uri(theURL);
                return _urlTo;
            }
            set { _urlTo = value; }
        }


        public string FullContentImageURL
        {
            get
            {
                if (string.IsNullOrEmpty(ContentPhotoURL))
                {
                    return VirtualPathUtility.ToAbsolute("~/content/contentinfo/default.png");
                }
                else
                {
                    return VirtualPathUtility.ToAbsolute(ContentPhotoURL);
                }
            }
        }

        public string RatingAmount
        {
            get { return (Rating*15).ToString(); }
        }


        public string TagLinks
        {
            get
            {
                if (string.IsNullOrEmpty(MetaKeywords)) return string.Empty;

                var sb = new StringBuilder(100);

                string[] keywords = MetaKeywords.Split(',');

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
                var sb = new StringBuilder(100);

                sb.Append(@"<li>");


                sb.Append(@"<div class=""row"">");

                sb.Append(@"<div class=""span3"">");

                sb.AppendFormat(@"<a class=""m_over"" href=""{0}""><img src=""{1}""  title=""{2}"" alt=""{2}""></a>",
                                UrlTo,
                                Utilities.S3ContentPath(ContentPhotoThumbURL), Title);
                sb.Append(@"</div>");


                sb.Append(@"<div class=""span4"">");
                sb.AppendFormat(@"<h4 class=""title""><a href=""{0}"">{1}</a></h4>", UrlTo, Title);

                sb.AppendFormat(@"<b>{0}</b>", Messages.Published);
                sb.Append(@": ");
                sb.Append(Utilities.TimeElapsedMessage(ReleaseDate));

                sb.Append(@"<br />");

                sb.AppendFormat(@"<b>{0}</b>", Messages.Language);
                sb.Append(@": ");

                sb.AppendFormat(@"<span class=""badge  badge-inverse"" title=""{1}"">{0}</span> ",
                                Language.ToUpper(), Utilities.GetLanguageNameForCode(Language));

                sb.Append(@"<br />");

                if (CreatedByUserID > 0)
                {
                    var ua = new UserAccount(CreatedByUserID);
                    var uad = new UserAccountDetail();
                    uad.GetUserAccountDeailForUser(ua.UserAccountID);

                    sb.AppendFormat(@"<b>{0}</b>", "Author"); //TODO: LOCALIZE
                    sb.Append(@": ");

                    sb.AppendFormat(@"<a href=""{0}"">{1}</a> &nbsp;", ua.UrlTo, ua.UserName);


                    sb.AppendFormat(@"<div title=""{0}"" class=""sprites sprite-{1}_small""></div>", uad.CountryName,
                                    uad.Country);
                    sb.Append(" ");
                    sb.Append(uad.SiteBagesLine);

                    sb.Append(@"<br />");
                }

                sb.AppendFormat(@"<b>{0}</b>", Messages.Tagged);
                sb.Append(": ");
                sb.Append(TagLinks);

                sb.Append(@"<br />");

                sb.Append(@"<p>");
                sb.Append(FromString.Truncate(MetaDescription, 300));
                sb.Append(@"</p>");

                sb.AppendFormat(@"<a href=""{0}"" class=""readmore"">{1} »</a>", UrlTo, Messages.ReadMore);

                sb.Append(@"<br />");

                if (Comments != null && Comments.Count > 0)
                {
                    sb.AppendFormat(@"<a href=""{0}#content_comments"">{1}: {2}</a>",
                                    UrlTo,
                                    Messages.Comments,
                                    Comments.Count.ToString());
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
        private bool _includeStartAndEndTags = true;

        public bool IncludeStartAndEndTags
        {
            private get { return _includeStartAndEndTags; }
            set { _includeStartAndEndTags = value; }
        }

        public string ToUnorderdList
        {
            get
            {
                var sb = new StringBuilder(100);

                if (IncludeStartAndEndTags) sb.Append(@"<ul>");

                int i = 0;

                foreach (var con in this)
                {
                    sb.Append(con.ToUnorderdListItem);

                    i++;
                }

                if (IncludeStartAndEndTags) sb.Append(@"</ul>");

                return sb.ToString();
            }
        }

        public static Dictionary<string, string> GetDistinctNewsLanguages()
        {
            var comm = DbAct.CreateCommand();
            comm.CommandText = "up_GetDistinctNewsLanguages";
            var dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            var dict = new Dictionary<string, string>();

            foreach (DataRow dr in dt.Rows)
            {
                string lang = FromObj.StringFromObj(dr["language"]);
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

            comm.AddParameter("language", language);

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            var ds = new DataSet();

            string[] keywords = null;

            var keywordsDict = new Dictionary<string, int>();

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

            keywordsDict =
                (from entry in keywordsDict orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key,
                                                                                                   pair => pair.Value);

            var contentTagList = new DataTable();

            contentTagList.Columns.Add("keyword_id", typeof (int));
            contentTagList.Columns.Add("keyword_value", typeof (string));
            contentTagList.Columns.Add("keyword_count", typeof (int));
            contentTagList.Columns.Add("keyword_url", typeof (string));

            int keywordID = 1;

            foreach (var tag in keywordsDict)
            {
                keywordID++;

                contentTagList.Rows.Add(keywordID, tag.Key, tag.Value, VirtualPathUtility.ToAbsolute("~/news/tag/" +
                                                                                                      FromString.URLKey(
                                                                                                          tag.Key)
                                                                            ));
            }

            ds.Tables.Add(contentTagList);

            return ds;
        }


        public static DataSet GetContentTagsAll()
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();

            // set the stored procedure name
            comm.CommandText = "up_GetContentTagsAll";

            DataTable dt = DbAct.ExecuteSelectCommand(comm);

            if (dt == null || dt.Rows.Count == 0) return null;

            var ds = new DataSet();

            string[] keywords = null;

            var keywordsDict = new Dictionary<string, int>();

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

            const int minimumNumberOfTagsToDisplay = 3;

            var keywordsDict2 = keywordsDict.Where(tag => tag.Value >= minimumNumberOfTagsToDisplay)
                                            .ToDictionary(tag => tag.Key, tag => tag.Value);

            keywordsDict2 =
                (from entry in keywordsDict2 orderby entry.Key ascending select entry).ToDictionary(pair => pair.Key,
                                                                                                    pair => pair.Value);

            var contentTagList = new DataTable();

            contentTagList.Columns.Add("keyword_id", typeof (int));
            contentTagList.Columns.Add("keyword_value", typeof (string));
            contentTagList.Columns.Add("keyword_count", typeof (int));
            contentTagList.Columns.Add("keyword_url", typeof (string));

            var keywordID = 1;

            foreach (var tag in keywordsDict2)
            {
                keywordID++;

                contentTagList.Rows.Add(keywordID, tag.Key, tag.Value, VirtualPathUtility.ToAbsolute("~/news/tag/" +
                                                                                                      FromString.URLKey(
                                                                                                          tag.Key)
                                                                            ));
            }

            ds.Tables.Add(contentTagList);

            return ds;
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

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
            comm.AddParameter("language", language);

            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
                }
            }

            return recordCount;
        }

        public int GetContentPageWiseRelease(int pageIndex, int pageSize, string language)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentPageWiseRelease";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
            comm.AddParameter("language", language);

            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
                }
            }

            return recordCount;
        }


        public int GetContentPageWiseReleaseAll(int pageIndex, int pageSize )
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentPageWiseReleaseAll";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
 

            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
                }
            }

            return recordCount;
        }


        public int  GetContentPageWiseKeyRelease(int pageIndex, int pageSize,   string key)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentPageWiseKeyRelease";

            DbParameter param = comm.CreateParameter();
            param.ParameterName = "@RecordCount";
            //http://stackoverflow.com/questions/3759285/ado-net-the-size-property-has-an-invalid-size-of-0
            param.Size = 1000;
            param.Direction = ParameterDirection.Output;
            comm.Parameters.Add(param);

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
            comm.AddParameter("key", key);

            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
                }
            }

            return recordCount;
        }


        public int GetContentPageWiseAll(int pageIndex, int pageSize)
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

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);


            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
                }
            }

            return recordCount;
        }

        public int GetContentAllPageWiseKey(int pageIndex, int pageSize, string key)
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

            comm.AddParameter("PageIndex", pageIndex);
            comm.AddParameter("PageSize", pageSize);
            comm.AddParameter("key", key);


            var ds = DbAct.ExecuteMultipleTableSelectCommand(comm);

            var recordCount = Convert.ToInt32(comm.Parameters["@RecordCount"].Value);

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (var content in from DataRow dr in ds.Tables[0].Rows select new Content(dr))
                {
                    Add(content);
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

            comm.AddParameter("createdByUserID", createdByUserID);

            var dt = DbAct.ExecuteSelectCommand(comm);

            // was something returned?
            if (dt == null || dt.Rows.Count <= 0) return;
            foreach (Content cnt in from DataRow dr in dt.Rows select new Content(dr))
            {
                Add(cnt);
            }
        }

        #region IGetAll Members

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

                    Add(cnt);
                }
            }
        }

        public void GetContentForContentTypeID(int contentTypeID, int siteDomainID)
        {
            // get a configured DbCommand object
            DbCommand comm = DbAct.CreateCommand();
            // set the stored procedure name
            comm.CommandText = "up_GetContentForContentTypeID";

            comm.AddParameter("contentTypeID", contentTypeID);
            comm.AddParameter("siteDomainID", siteDomainID);

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
                        Add(cnt);
                    }
                }
            }
        }

        #endregion
    }
}