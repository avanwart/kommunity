using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class SizeType
    {
        public SizeType()
        {
            this.Sizes = new List<Size>();
        }

        public int sizeTypeID { get; set; }
        public string name { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public virtual ICollection<Size> Sizes { get; set; }
    }
}
