using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class VideoLog
    {
        [Key]
        public int videoLogID { get; set; }

        public DateTime? createDate { get; set; }
        public int videoID { get; set; }
        public string ipAddress { get; set; }
    }
}