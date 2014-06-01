using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class SongProperty
    {
        [Key]
        public int songPropertyID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int songID { get; set; }
        public string propertyContent { get; set; }
        public string propertyType { get; set; }
        public virtual Song Song { get; set; }
    }
}