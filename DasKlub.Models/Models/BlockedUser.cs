using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class BlockedUser
    {
        [Key]
        public int blockedUserID { get; set; }

        public int? userAccountIDBlocking { get; set; }
        public int? userAccountIDBlocked { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
    }
}