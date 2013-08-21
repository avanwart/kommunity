using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class SiteComment
    {
        public int siteCommentID { get; set; }
        public string detail { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
    }
}
