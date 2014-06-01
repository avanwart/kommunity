using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class WallMessage
    {
        [Key]
        public int wallMessageID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string message { get; set; }
        public bool isRead { get; set; }
        public int fromUserAccountID { get; set; }
        public int toUserAccountID { get; set; }
    }
}