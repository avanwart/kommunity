using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using BootBaronLib.AppSpec.DasKlub.BOL;
using BootBaronLib.Operational;
using DasKlub.Web.Models.Domain;

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

        [StringLength(150)]
        [Required]
        public string Title { get; set; }

        [StringLength(420)]
        [Required]
        public string Description { get; set; }

        [NotMapped]
        public UserAccount UserAccount { get; set; }


        [NotMapped]
        public Uri SubForumURL
        {
            get
            {
                return ForumCategory != null ? new Uri(Utilities.URLAuthority() + VirtualPathUtility.ToAbsolute(string.Format("~/forum/{0}/{1}", ForumCategory.Key, Key))) : null;
            }
        }

        #endregion

        [NotMapped]
        public int TotalPosts { get; set; }

        [NotMapped]
        public ForumPost LatestForumPost { get; set; }
    }
}
