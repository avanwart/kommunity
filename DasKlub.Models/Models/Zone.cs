using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Zone
    {
        public Zone()
        {
            this.StatusUpdates = new List<StatusUpdate>();
        }

        [Key]
        public int zoneID { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public string name { get; set; }
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}
