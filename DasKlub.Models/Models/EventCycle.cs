using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class EventCycle
    {
        public EventCycle()
        {
            this.Events = new List<Event>();
        }

        [Key]
        public int eventCycleID { get; set; }
        public string cycleName { get; set; }
        public string eventCode { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
