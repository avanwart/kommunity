using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class ContentType
    {
        public ContentType()
        {
            this.Contents = new List<Content>();
        }

        [Key]
        public int contentTypeID { get; set; }
        public string contentName { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public string contentCode { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}
