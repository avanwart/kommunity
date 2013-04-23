using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BootBaronLib.AppSpec.DasKlub.BOL;
using DasKlub.Models.Domain;

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
