using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class InterestedIn
    {
        public InterestedIn()
        {
            this.UserAccountDetails = new List<UserAccountDetailEntity>();
        }
        [Key]
        public int interestedInID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string typeLetter { get; set; }
        public string name { get; set; }
        public virtual ICollection<UserAccountDetailEntity> UserAccountDetails { get; set; }
    }
}
