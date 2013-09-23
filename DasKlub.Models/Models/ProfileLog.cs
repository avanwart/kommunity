using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public partial class ProfileLog
    {
                [Key]

        public int profileLogID { get; set; }
        public int lookingUserAccountID { get; set; }
        public Nullable<int> lookedAtUserAccountID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual UserAccountEntity UserAccount1 { get; set; }
    }
}
