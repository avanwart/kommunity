using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class UserAccountMet
    {
        [Key]
        public int userAccountRequester { get; set; }

        public int userAccounted { get; set; }
        public bool haveMet { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual UserAccountEntity UserAccount1 { get; set; }
    }
}