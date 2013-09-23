using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class ContestVideo
    {
        public ContestVideo()
        {
            this.ContestVideoVotes = new List<ContestVideoVote>();
        }

        [Key]
        public int contestVideoID { get; set; }
        public int videoID { get; set; }
        public int contestID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<byte> contestRank { get; set; }
        public string subContest { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual Video Video { get; set; }
        public virtual ICollection<ContestVideoVote> ContestVideoVotes { get; set; }
    }
}
