using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlub.Lib.AppSpec.DasKlub.BOL;
using DasKlub.Web.Models.Domain;

namespace DasKlub.Models.Forum
{
    public class ForumPost : StateInfo
    {
        public int ForumPostID { get; set; }

        [Required]
        public string Detail { get; set; }

        public int ForumSubCategoryID { get; set; }

        public ForumSubCategory ForumSubCategory { get; set; }

        [NotMapped]
        public UserAccount UserAccount { get; set; }

        [NotMapped]
        public Uri ForumPostURL { get; set; }

        [NotMapped]
        public bool IsNewPost { get; set; }
    }
}
