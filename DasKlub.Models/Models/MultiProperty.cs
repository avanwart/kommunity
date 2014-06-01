using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class MultiProperty
    {
        public MultiProperty()
        {
            Videos = new List<Video>();
        }

        [Key]
        public int multiPropertyID { get; set; }

        public int propertyTypeID { get; set; }
        public string name { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public string propertyContent { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}