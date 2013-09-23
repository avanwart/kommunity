using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class HostedVideoLog
    {
        [Key]
        public int videoLogID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public string viewURL { get; set; }
        public string ipAddress { get; set; }
        public Nullable<int> secondsElapsed { get; set; }
        public string videoType { get; set; }
    }
}
