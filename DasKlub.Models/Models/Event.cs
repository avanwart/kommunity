using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Event
    {
        public Event()
        {
            ArtistEvents = new List<ArtistEvent>();
        }

        [Key]
        public int eventID { get; set; }

        public string name { get; set; }
        public int venueID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public DateTime? localTimeBegin { get; set; }
        public string notes { get; set; }
        public string ticketURL { get; set; }
        public DateTime? localTimeEnd { get; set; }
        public int eventCycleID { get; set; }
        public string rsvpURL { get; set; }
        public bool isReoccuring { get; set; }
        public bool isEnabled { get; set; }
        public string eventDetailURL { get; set; }
        public virtual ICollection<ArtistEvent> ArtistEvents { get; set; }
        public virtual EventCycle EventCycle { get; set; }
        public virtual Venue Venue { get; set; }
    }
}