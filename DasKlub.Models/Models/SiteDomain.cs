using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class SiteDomain
    {
        [Key]
        public int siteDomainID { get; set; }

        public string propertyType { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public string language { get; set; }
        public string description { get; set; }
    }
}