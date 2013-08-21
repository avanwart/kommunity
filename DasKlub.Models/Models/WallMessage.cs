using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class WallMessage
    {
        public int wallMessageID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string message { get; set; }
        public bool isRead { get; set; }
        public int fromUserAccountID { get; set; }
        public int toUserAccountID { get; set; }
    }
}
