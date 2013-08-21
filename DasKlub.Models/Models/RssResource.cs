using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class RssResource
    {
        public RssResource()
        {
            this.RSSItems = new List<RSSItem>();
        }

        public int rssResourceID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string rssResourceURL { get; set; }
        public string resourceName { get; set; }
        public string providerKey { get; set; }
        public bool isEnabled { get; set; }
        public Nullable<int> artistID { get; set; }
        public virtual ICollection<RSSItem> RSSItems { get; set; }
    }
}
