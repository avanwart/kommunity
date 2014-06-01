using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class UserConnection
    {
        [Key]
        public int userConnectionID { get; set; }

        public int fromUserAccountID { get; set; }
        public int toUserAccountID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string statusType { get; set; }
        public bool isConfirmed { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual UserAccountEntity UserAccount1 { get; set; }
    }
}