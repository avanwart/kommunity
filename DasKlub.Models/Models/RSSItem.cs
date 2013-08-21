using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class RSSItem
    {
        public int rssItemID { get; set; }
        public int rssResourceID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string authorName { get; set; }
        public string commentsURL { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTime> pubDate { get; set; }
        public string title { get; set; }
        public string languageName { get; set; }
        public Nullable<int> artistID { get; set; }
        public string link { get; set; }
        public string guidLink { get; set; }
        public virtual RssResource RssResource { get; set; }
    }
}
