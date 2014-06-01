using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ContentType
    {
        public ContentType()
        {
            Contents = new List<Content>();
        }

        [Key]
        public int contentTypeID { get; set; }

        public string contentName { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? updatedByUserID { get; set; }
        public string contentCode { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}