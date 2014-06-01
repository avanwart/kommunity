using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class RSSItem
    {
        [Key]
        public int rssItemID { get; set; }

        public int rssResourceID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string authorName { get; set; }
        public string commentsURL { get; set; }
        public string description { get; set; }
        public DateTime? pubDate { get; set; }
        public string title { get; set; }
        public string languageName { get; set; }
        public int? artistID { get; set; }
        public string link { get; set; }
        public string guidLink { get; set; }
        public virtual RssResource RssResource { get; set; }
    }
}