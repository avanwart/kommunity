using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Size
    {
        [Key]
        public int sizeID { get; set; }
        public string sizeName { get; set; }
        public Nullable<int> sizeTypeID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public Nullable<byte> rankOrder { get; set; }
        public virtual SizeType SizeType { get; set; }
    }
}
