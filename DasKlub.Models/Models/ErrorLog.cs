using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ErrorLog
    {
        [Key]
        public int errorLogID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public int? responseCode { get; set; }
    }
}