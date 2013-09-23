using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class MultiProperty
    {
        public MultiProperty()
        {
            this.Videos = new List<Video>();
        }

        [Key]
        public int multiPropertyID { get; set; }
        public int propertyTypeID { get; set; }
        public string name { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public string propertyContent { get; set; }
        public virtual PropertyType PropertyType { get; set; }
        public virtual ICollection<Video> Videos { get; set; }
    }
}
