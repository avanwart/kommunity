using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
using DasKlub.Lib.BaseTypes;
using DasKlub.Lib.BOL.UserContent;
using DasKlub.Lib.Operational;
using DasKlub.Lib.Resources;
using DasKlub.Lib.Values;

namespace DasKlub.Web.Models
{
    public class ContentModel : BaseIUserLogCrud
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

        public ContentComment Reply { get; set; }
        public string ThumbIcon { get; set; }
        public string PreviousNews { get; set; }
        public string NextNews { get; set; }
        public string VideoWidth { get; set; }

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

        public const string ContentImagePath = "~/content/contentinfo/";

        #endregion

        private Uri _urlTo;

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
            get { return Detail == null ? string.Empty : Detail.Replace(Environment.NewLine, "<br />"); }
        }

        public Uri UrlTo
        {
            get
            {
                string theUrl = string.Concat("http://",
                    HttpContext.Current.Request.Url.Authority);

                theUrl += string.Concat("/news/", ContentKey);

                _urlTo = new Uri(theUrl);
                return _urlTo;
            }
            set { _urlTo = value; }
        }


        public string FullContentImageURL
        {
            get
            {
                return VirtualPathUtility.ToAbsolute(string.IsNullOrEmpty(ContentPhotoURL)
                    ? "~/content/contentinfo/default.png"
                    : ContentPhotoURL);
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
                                                      FromString.UrlKey(keyword)
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
    }
}