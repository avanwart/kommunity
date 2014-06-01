using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class Content
    {
        public Content()
        {
            ContentComments = new List<ContentComment>();
        }

        [Key]
        public int contentID { get; set; }

        public int? siteDomainID { get; set; }
        public string contentKey { get; set; }
        public string title { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string detail { get; set; }
        public string metaDescription { get; set; }
        public string metaKeywords { get; set; }
        public int? contentTypeID { get; set; }
        public DateTime? releaseDate { get; set; }
        public double? rating { get; set; }
        public string contentPhotoURL { get; set; }
        public string contentPhotoThumbURL { get; set; }
        public string contentVideoURL { get; set; }
        public string outboundURL { get; set; }
        public bool isEnabled { get; set; }
        public string currentStatus { get; set; }
        public string language { get; set; }
        public string contentVideoURL2 { get; set; }
        public virtual ContentType ContentType { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual ICollection<ContentComment> ContentComments { get; set; }
    }
}