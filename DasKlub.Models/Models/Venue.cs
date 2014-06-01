using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Venue
    {
        public Venue()
        {
            Events = new List<Event>();
        }

        [Key]
        public int venueID { get; set; }

        public string venueName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string postalCode { get; set; }
        public string countryISO { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string venueURL { get; set; }
        public bool isEnabled { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public string phoneNumber { get; set; }
        public string venueType { get; set; }
        public string description { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}