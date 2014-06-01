using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ContestVideo
    {
        public ContestVideo()
        {
            ContestVideoVotes = new List<ContestVideoVote>();
        }

        [Key]
        public int contestVideoID { get; set; }

        public int videoID { get; set; }
        public int contestID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public byte? contestRank { get; set; }
        public string subContest { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual Video Video { get; set; }
        public virtual ICollection<ContestVideoVote> ContestVideoVotes { get; set; }
    }
}