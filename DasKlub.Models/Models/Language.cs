using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Language
    {
        public Language()
        {
            this.UserAccounts = new List<UserAccount>();
        }

        public int languageID { get; set; }
        public string languageType { get; set; }
        public string languageName { get; set; }
        public virtual ICollection<UserAccount> UserAccounts { get; set; }
    }
}
