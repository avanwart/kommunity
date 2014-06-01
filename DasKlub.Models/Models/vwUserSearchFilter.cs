using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class vwUserSearchFilter
    {
        [Key]
        public int userAccountID { get; set; }

        public int? youAreID { get; set; }
        public int? relationshipStatusID { get; set; }
        public int? interestedInID { get; set; }
        public string country { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public DateTime? birthDate { get; set; }
        public string defaultLanguage { get; set; }
        public bool? isOnline { get; set; }
        public DateTime? lastActivityDate { get; set; }
        public bool showOnMap { get; set; }
    }
}