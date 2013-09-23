using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class StatusCommentAcknowledgement
    {
        [Key]
        public int statusCommentAcknowledgementID { get; set; }
        public int userAccountID { get; set; }
        public int statusCommentID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string acknowledgementType { get; set; }
        public virtual StatusComment StatusComment { get; set; }
    }
}
