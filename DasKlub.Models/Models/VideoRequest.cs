using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class VideoRequest
    {
        [Key]
        public int videoRequestID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string requestURL { get; set; }
        public string statusType { get; set; }
        public string videoKey { get; set; }
    }
}
