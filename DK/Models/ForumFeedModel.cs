using System;
using BootBaronLib.AppSpec.DasKlub.BOL;
using DasKlub.Models.Forum;

namespace DasKlub.Web.Models
{
    public class ForumFeedModel
    {
        public UserAccount LastFrom { get; set; }

        public DateTime LastPosted { get; set; }

        public ForumCategory ForumCategory { get; set; }

        public ForumSubCategory ForumSubCategory { get; set; }

        public int PostsToThread { get; set; }
    }
}