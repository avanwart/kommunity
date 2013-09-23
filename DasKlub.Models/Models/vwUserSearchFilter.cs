using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class vwUserSearchFilter
    {
        [Key]
        public int userAccountID { get; set; }
        public Nullable<int> youAreID { get; set; }
        public Nullable<int> relationshipStatusID { get; set; }
        public Nullable<int> interestedInID { get; set; }
        public string country { get; set; }
        public Nullable<decimal> latitude { get; set; }
        public Nullable<decimal> longitude { get; set; }
        public Nullable<System.DateTime> birthDate { get; set; }
        public string defaultLanguage { get; set; }
        public Nullable<bool> isOnline { get; set; }
        public Nullable<System.DateTime> lastActivityDate { get; set; }
        public bool showOnMap { get; set; }
    }
}
