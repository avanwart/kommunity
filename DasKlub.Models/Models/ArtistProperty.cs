using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ArtistProperty
    {
        [Key]
        public int artistPropertyID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int artistID { get; set; }
        public string propertyContent { get; set; }
        public string propertyType { get; set; }
        public virtual Artist Artist { get; set; }
    }
}