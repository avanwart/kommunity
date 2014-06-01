using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Contest
    {
        public Contest()
        {
            ContestVideos = new List<ContestVideo>();
        }

        [Key]
        public int contestID { get; set; }

        public string name { get; set; }
        public DateTime deadLine { get; set; }
        public string description { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public DateTime? beginDate { get; set; }
        public string contestKey { get; set; }
        public virtual ICollection<ContestVideo> ContestVideos { get; set; }
    }
}