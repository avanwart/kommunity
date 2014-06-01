using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ClickLog
    {
        [Key]
        public int clickLogID { get; set; }

        public string ipAddress { get; set; }
        public string clickType { get; set; }
        public string referringURL { get; set; }
        public string currentURL { get; set; }
        public int? productID { get; set; }
        public int? createdByUserID { get; set; }
        public DateTime createDate { get; set; }
    }
}