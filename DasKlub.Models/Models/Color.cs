using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Color
    {
        [Key]
        public int colorID { get; set; }

        public string name { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public int? siteDomainID { get; set; }
    }
}