using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class StatusUpdateNotification
    {
        [Key]
        public int statusUpdateNotificationID { get; set; }

        public int statusUpdateID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public bool isRead { get; set; }
        public int? userAccountID { get; set; }
        public string responseType { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
    }
}