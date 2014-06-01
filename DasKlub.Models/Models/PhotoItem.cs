using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class PhotoItem
    {
        public PhotoItem()
        {
            StatusUpdates = new List<StatusUpdate>();
        }

        [Key]
        public int photoItemID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string title { get; set; }
        public string filePathRaw { get; set; }
        public string filePathThumb { get; set; }
        public string filePathStandard { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}