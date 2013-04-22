using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class InterestedIn
    {
        public InterestedIn()
        {
            this.UserAccountDetails = new List<UserAccountDetail>();
        }

        public int interestedInID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string typeLetter { get; set; }
        public string name { get; set; }
        public virtual ICollection<UserAccountDetail> UserAccountDetails { get; set; }
    }
}
