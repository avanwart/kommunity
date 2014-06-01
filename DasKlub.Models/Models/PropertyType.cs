using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class PropertyType
    {
        public PropertyType()
        {
            MultiProperties = new List<MultiProperty>();
        }

        [Key]
        public int propertyTypeID { get; set; }

        public string propertyTypeCode { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public string propertyTypeName { get; set; }
        public virtual ICollection<MultiProperty> MultiProperties { get; set; }
    }
}