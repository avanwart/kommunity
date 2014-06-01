using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class ContestVideoVote
    {
        [Key]
        public int contestVideoVoteID { get; set; }

        public int? userAccountID { get; set; }
        public int? contestVideoID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public virtual ContestVideo ContestVideo { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
    }
}