using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class UserAccountMet
    {
        public int userAccountRequester { get; set; }
        public int userAccounted { get; set; }
        public bool haveMet { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual UserAccount UserAccount1 { get; set; }
    }
}
