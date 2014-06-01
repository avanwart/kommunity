using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class YouAre
    {
        public YouAre()
        {
            UserAccountDetails = new List<UserAccountDetailEntity>();
        }

        [Key]
        public int youAreID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string typeLetter { get; set; }
        public string name { get; set; }
        public virtual ICollection<UserAccountDetailEntity> UserAccountDetails { get; set; }
    }
}