using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class StatusComment
    {
        public StatusComment()
        {
            this.StatusCommentAcknowledgements = new List<StatusCommentAcknowledgement>();
        }
        [Key]
        public int statusCommentID { get; set; }
        public int statusUpdateID { get; set; }
        public int userAccountID { get; set; }
        public string statusType { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public string message { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
        public virtual ICollection<StatusCommentAcknowledgement> StatusCommentAcknowledgements { get; set; }
    }
}
