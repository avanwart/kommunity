using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Vote
    {
        [Key]
        public int voteID { get; set; }

        public int userAccountID { get; set; }
        public DateTime createDate { get; set; }
        public int videoID { get; set; }
        public int score { get; set; }
    }
}