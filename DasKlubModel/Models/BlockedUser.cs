using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class BlockedUser
    {
        public int blockedUserID { get; set; }
        public Nullable<int> userAccountIDBlocking { get; set; }
        public Nullable<int> userAccountIDBlocked { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
