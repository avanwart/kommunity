using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class StatusUpdateNotification
    {
        [Key]
        public int statusUpdateNotificationID { get; set; }
        public int statusUpdateID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public bool isRead { get; set; }
        public Nullable<int> userAccountID { get; set; }
        public string responseType { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
    }
}
