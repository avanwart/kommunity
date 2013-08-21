using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class PhotoItem
    {
        public PhotoItem()
        {
            this.StatusUpdates = new List<StatusUpdate>();
        }

        public int photoItemID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string title { get; set; }
        public string filePathRaw { get; set; }
        public string filePathThumb { get; set; }
        public string filePathStandard { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}
