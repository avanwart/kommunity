using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ContestVideoVote
    {
        public int contestVideoVoteID { get; set; }
        public Nullable<int> userAccountID { get; set; }
        public Nullable<int> contestVideoID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public virtual ContestVideo ContestVideo { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
