using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public partial class StatusUpdate
    {
        public StatusUpdate()
        {
            this.Acknowledgements = new List<Acknowledgement>();
            this.StatusComments = new List<StatusComment>();
            this.StatusUpdateNotifications = new List<StatusUpdateNotification>();
        }

                [Key]

        public int statusUpdateID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public int userAccountID { get; set; }
        public string message { get; set; }
        public string statusType { get; set; }
        public Nullable<int> photoItemID { get; set; }
        public Nullable<int> zoneID { get; set; }
        public bool isMobile { get; set; }
        public virtual ICollection<Acknowledgement> Acknowledgements { get; set; }
        public virtual PhotoItem PhotoItem { get; set; }
        public virtual ICollection<StatusComment> StatusComments { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual Zone Zone { get; set; }
        public virtual ICollection<StatusUpdateNotification> StatusUpdateNotifications { get; set; }
    }
}
