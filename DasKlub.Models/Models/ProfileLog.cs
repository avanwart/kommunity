using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class ProfileLog
    {
        [Key]
        public int profileLogID { get; set; }

        public int lookingUserAccountID { get; set; }
        public int? lookedAtUserAccountID { get; set; }
        public DateTime createDate { get; set; }
        public int? createdByUserID { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual UserAccountEntity UserAccount1 { get; set; }
    }
}