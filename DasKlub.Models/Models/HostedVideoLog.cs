using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class HostedVideoLog
    {
        [Key]
        public int videoLogID { get; set; }

        public DateTime? createDate { get; set; }
        public string viewURL { get; set; }
        public string ipAddress { get; set; }
        public int? secondsElapsed { get; set; }
        public string videoType { get; set; }
    }
}