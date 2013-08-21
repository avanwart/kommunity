using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ContentComment
    {
        public int contentCommentID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string statusType { get; set; }
        public string detail { get; set; }
        public Nullable<int> contentID { get; set; }
        public string fromName { get; set; }
        public string fromEmail { get; set; }
        public string ipAddress { get; set; }
        public virtual Content Content { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
