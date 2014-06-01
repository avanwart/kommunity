using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class DirectMessage
    {
        [Key]
        public int directMessageID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int fromUserAccountID { get; set; }
        public int toUserAccountID { get; set; }
        public bool isRead { get; set; }
        public string message { get; set; }
        public bool isEnabled { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual UserAccountEntity UserAccount1 { get; set; }
    }
}