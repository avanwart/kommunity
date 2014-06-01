using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class BlackIPID
    {
        [Key]
        public int blackIPID1 { get; set; }

        public string ipAddress { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
    }
}