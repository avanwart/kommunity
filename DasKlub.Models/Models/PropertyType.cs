using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class PropertyType
    {
        public PropertyType()
        {
            this.MultiProperties = new List<MultiProperty>();
        }

        public int propertyTypeID { get; set; }
        public string propertyTypeCode { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public string propertyTypeName { get; set; }
        public virtual ICollection<MultiProperty> MultiProperties { get; set; }
    }
}
