using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class RssResource
    {
        public RssResource()
        {
            RSSItems = new List<RSSItem>();
        }

        [Key]
        public int rssResourceID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string rssResourceURL { get; set; }
        public string resourceName { get; set; }
        public string providerKey { get; set; }
        public bool isEnabled { get; set; }
        public int? artistID { get; set; }
        public virtual ICollection<RSSItem> RSSItems { get; set; }
    }
}