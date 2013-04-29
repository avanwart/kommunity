using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using BootBaronLib.Operational;
using DasKlub.Models.Domain;

namespace DasKlub.Models.Forum
{
    public class ForumCategory : StateInfo
    {
        #region members 

        #endregion

        #region constructors

        public ForumCategory( )
        {
            ForumSubCategory = new Collection<ForumSubCategory>();
        }

        #endregion

        #region properties

        public int ForumCategoryID { get; set; }

        [StringLength(50)]
        [Required]
        public string Title { get; set; }

        [StringLength(50)]
        [Required]
        public string Key { get; set; }

        [Required]
        public string Description { get; set; }

        protected virtual ICollection<ForumSubCategory> ForumSubCategory { get; set; }

        [NotMapped]
        public Uri ForumURL
        {
            get
            {
                return new Uri(Utilities.URLAuthority() + VirtualPathUtility.ToAbsolute(string.Format("~/forum/{0}", Key)));
            }
        }


        [NotMapped]
        public ForumPost LatestForumPost { get; set; }

        [NotMapped]
        public int TotalPosts { get; set; }

        #endregion

    }
}
