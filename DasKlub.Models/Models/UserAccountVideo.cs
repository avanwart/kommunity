using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class UserAccountVideo
    {
        [Key]
        [Column(Order = 0)]
        public int videoID { get; set; }

        [Key]
        [Column(Order = 1)]
        public int userAccountID { get; set; }

        public DateTime createDate { get; set; }
        public string videoType { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual Video Video { get; set; }
    }
}