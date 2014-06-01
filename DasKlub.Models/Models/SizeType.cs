using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class SizeType
    {
        public SizeType()
        {
            Sizes = new List<Size>();
        }

        [Key]
        public int sizeTypeID { get; set; }

        public string name { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}