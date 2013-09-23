using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public partial class Role
    {
        public Role()
        {
            this.UserAccounts = new List<UserAccountEntity>();
        }

                [Key]

        public int roleID { get; set; }
        public string roleName { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updatedDate { get; set; }
        public Nullable<int> createdByEndUserID { get; set; }
        public Nullable<int> updatedByEndUserID { get; set; }
        public string description { get; set; }
        public virtual ICollection<UserAccountEntity> UserAccounts { get; set; }
    }
}
