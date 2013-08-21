using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Vote
    {
        public int voteID { get; set; }
        public int userAccountID { get; set; }
        public System.DateTime createDate { get; set; }
        public int videoID { get; set; }
        public int score { get; set; }
    }
}
