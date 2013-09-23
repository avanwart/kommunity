using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Venue
    {
        public Venue()
        {
            this.Events = new List<Event>();
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
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string venueURL { get; set; }
        public bool isEnabled { get; set; }
        public Nullable<decimal> latitude { get; set; }
        public Nullable<decimal> longitude { get; set; }
        public string phoneNumber { get; set; }
        public string venueType { get; set; }
        public string description { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
