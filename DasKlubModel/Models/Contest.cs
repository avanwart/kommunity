using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Contest
    {
        public Contest()
        {
            this.ContestVideos = new List<ContestVideo>();
        }

        public int contestID { get; set; }
        public string name { get; set; }
        public System.DateTime deadLine { get; set; }
        public string description { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<System.DateTime> beginDate { get; set; }
        public string contestKey { get; set; }
        public virtual ICollection<ContestVideo> ContestVideos { get; set; }
    }
}
