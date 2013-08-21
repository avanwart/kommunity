using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ProfileLog
    {
        public int profileLogID { get; set; }
        public int lookingUserAccountID { get; set; }
        public Nullable<int> lookedAtUserAccountID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserAccount UserAccount1 { get; set; }
    }
}
