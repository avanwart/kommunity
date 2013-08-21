using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class DirectMessage
    {
        public int directMessageID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public int fromUserAccountID { get; set; }
        public int toUserAccountID { get; set; }
        public bool isRead { get; set; }
        public string message { get; set; }
        public bool isEnabled { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserAccount UserAccount1 { get; set; }
    }
}
