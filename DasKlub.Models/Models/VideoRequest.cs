using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class VideoRequest
    {
        [Key]
        public int videoRequestID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string requestURL { get; set; }
        public string statusType { get; set; }
        public string videoKey { get; set; }
    }
}