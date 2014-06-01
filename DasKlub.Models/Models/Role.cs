using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class Role
    {
        public Role()
        {
            UserAccounts = new List<UserAccountEntity>();
        }

        [Key]
        public int roleID { get; set; }

        public string roleName { get; set; }
        public DateTime? createDate { get; set; }
        public DateTime? updatedDate { get; set; }
        public int? createdByEndUserID { get; set; }
        public int? updatedByEndUserID { get; set; }
        public string description { get; set; }
        public virtual ICollection<UserAccountEntity> UserAccounts { get; set; }
    }
}