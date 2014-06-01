using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class StatusComment
    {
        public StatusComment()
        {
            StatusCommentAcknowledgements = new List<StatusCommentAcknowledgement>();
        }

        [Key]
        public int statusCommentID { get; set; }

        public int statusUpdateID { get; set; }
        public int userAccountID { get; set; }
        public string statusType { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string message { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
        public virtual ICollection<StatusCommentAcknowledgement> StatusCommentAcknowledgements { get; set; }
    }
}