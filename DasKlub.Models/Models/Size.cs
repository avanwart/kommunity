using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Size
    {
        [Key]
        public int sizeID { get; set; }

        public string sizeName { get; set; }
        public int? sizeTypeID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public byte? rankOrder { get; set; }
        public virtual SizeType SizeType { get; set; }
    }
}