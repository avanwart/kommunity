using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class ContentComment
    {
        [Key]
        public int contentCommentID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string statusType { get; set; }
        public string detail { get; set; }
        public int? contentID { get; set; }
        public string fromName { get; set; }
        public string fromEmail { get; set; }
        public string ipAddress { get; set; }
        public virtual Content Content { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
    }
}