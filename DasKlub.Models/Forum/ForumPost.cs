using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlub.Lib.BOL;
using DasKlub.Models.Domain;

namespace DasKlub.Models.Forum
{
    public class ForumPost : StateInfo
    {
        public int ForumPostID { get; set; }
        
        private string _detail = string.Empty;

        [Required]
        public string Detail
        {
            get
            {
                if (_detail != null)
                    _detail = _detail.Trim();
                return _detail;
            }
            set { _detail = value; }
        }

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
