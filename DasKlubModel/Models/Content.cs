using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Content
    {
        public Content()
        {
            this.ContentComments = new List<ContentComment>();
        }

        public int contentID { get; set; }
        public Nullable<int> siteDomainID { get; set; }
        public string contentKey { get; set; }
        public string title { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string detail { get; set; }
        public string metaDescription { get; set; }
        public string metaKeywords { get; set; }
        public Nullable<int> contentTypeID { get; set; }
        public Nullable<System.DateTime> releaseDate { get; set; }
        public Nullable<double> rating { get; set; }
        public string contentPhotoURL { get; set; }
        public string contentPhotoThumbURL { get; set; }
        public string contentVideoURL { get; set; }
        public string outboundURL { get; set; }
        public bool isEnabled { get; set; }
        public string currentStatus { get; set; }
        public string language { get; set; }
        public string contentVideoURL2 { get; set; }
        public virtual ContentType ContentType { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<ContentComment> ContentComments { get; set; }
    }
}
