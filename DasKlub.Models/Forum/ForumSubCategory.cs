using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using DasKlub.Lib.BOL;
using DasKlub.Lib.Operational;
using DasKlub.Models.Domain;

namespace DasKlub.Models.Forum
{
    public class ForumSubCategory : StateInfo
    {
        #region members

        private ICollection<ForumPost> _forumPost;

        #endregion

        #region constructors

        public ForumSubCategory()
        {
            _forumPost = new Collection<ForumPost>();
        }
        #endregion

        #region properties

        public int ForumSubCategoryID { get; set; }

        public ForumCategory ForumCategory { get; set; }

        public int ForumCategoryID { get; set; }

        [StringLength(150)]
        [Required]
        public string Key { get; set; }

        private string _title = string.Empty;
        
        [StringLength(150)]
        [Required]
        public string Title
        {
            get {
                if (_title != null)
                    _title = _title.Trim();
                return _title; }
            set { _title = value; }
        }

        private string _description = string.Empty;

        [Required]
        public string Description
        {
            get
            {
                if (_description != null)
                    _description = _description.Trim();
                return _description;
            }
            set { _description = value; }
        }

        [NotMapped]
        public UserAccount UserAccount { get; set; }


        [NotMapped]
        public Uri SubForumURL
        {
            get
            {
                return ForumCategory != null ? new Uri(Utilities.URLAuthority() +
                    VirtualPathUtility.ToAbsolute(string.Format("~/forum/{0}/{1}", ForumCategory.Key, Key))) : null;
            }
        }

        #endregion

        [NotMapped]
        public int TotalPosts { get; set; }

        [NotMapped]
        public ForumPost LatestForumPost { get; set; }
    }
}
