using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Zone
    {
        public Zone()
        {
            StatusUpdates = new List<StatusUpdate>();
        }

        [Key]
        public int zoneID { get; set; }

        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string name { get; set; }
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}