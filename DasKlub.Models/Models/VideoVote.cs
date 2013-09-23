using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class VideoVote
    {
        [Key]
        public int videoVoteID { get; set; }
        public string ipAddress { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string singlePick1 { get; set; }
        public string singlePick2 { get; set; }
        public string singlePick3 { get; set; }
        public string groupPick1 { get; set; }
        public string groupPick2 { get; set; }
        public string groupPick3 { get; set; }
    }
}
