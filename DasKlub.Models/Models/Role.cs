using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Role
    {
        public Role()
        {
            this.UserAccounts = new List<UserAccount>();
        }

        public int roleID { get; set; }
        public string roleName { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public Nullable<int> createdByEndUserID { get; set; }
        public Nullable<int> updatedByEndUserID { get; set; }
        public string description { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
