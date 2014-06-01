using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class EventCycle
    {
        public EventCycle()
        {
            Events = new List<Event>();
        }

        [Key]
        public int eventCycleID { get; set; }

        public string cycleName { get; set; }
        public string eventCode { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}