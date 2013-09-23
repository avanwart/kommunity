using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public partial class Language
    {
        public Language()
        {
            this.UserAccounts = new List<UserAccountEntity>();
        }

                [Key]

        public int languageID { get; set; }
        public string languageType { get; set; }
        public string languageName { get; set; }
        public virtual ICollection<UserAccountEntity> UserAccounts { get; set; }
    }
}
