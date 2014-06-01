using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class StatusUpdate
    {
        public StatusUpdate()
        {
            Acknowledgements = new List<Acknowledgement>();
            StatusComments = new List<StatusComment>();
            StatusUpdateNotifications = new List<StatusUpdateNotification>();
        }

        [Key]
        public int statusUpdateID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int userAccountID { get; set; }
        public string message { get; set; }
        public string statusType { get; set; }
        public int? photoItemID { get; set; }
        public int? zoneID { get; set; }
        public bool isMobile { get; set; }
        public virtual ICollection<Acknowledgement> Acknowledgements { get; set; }
        public virtual PhotoItem PhotoItem { get; set; }
        public virtual ICollection<StatusComment> StatusComments { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<StatusUpdateNotification> StatusUpdateNotifications { get; set; }
    }
}