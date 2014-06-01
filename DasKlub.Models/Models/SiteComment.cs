using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class SiteComment
    {
        [Key]
        public int siteCommentID { get; set; }

        public string detail { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
    }
}