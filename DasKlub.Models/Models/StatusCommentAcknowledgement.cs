using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class StatusCommentAcknowledgement
    {
        [Key]
        public int statusCommentAcknowledgementID { get; set; }

        public int userAccountID { get; set; }
        public int statusCommentID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string acknowledgementType { get; set; }
        public virtual StatusComment StatusComment { get; set; }
    }
}