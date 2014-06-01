using System;
using DasKlub.Models.Forum;

namespace DasKlub.Web.Models
{
    public class ForumFeedModel
    {
        public DateTime LastPosted { get; set; }

        public ForumCategory ForumCategory { get; set; }

        public ForumSubCategory ForumSubCategory { get; set; }

        public int PostCount { get; set; }

        /// <summary>
        ///     When it's an authenciated user, say if it's new to them
        /// </summary>
        public bool IsNewPost { get; set; }

        /// <summary>
        ///     Use this property when they are authenticated, the URL should be
        ///     to the last post on the last page
        /// </summary>
        public Uri URLTo { get; set; }

        public string UserName { get; set; }
    }
}