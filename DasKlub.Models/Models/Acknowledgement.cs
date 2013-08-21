using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Acknowledgement
    {
        public int acknowledgementID { get; set; }
        public int userAccountID { get; set; }
        public int statusUpdateID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string acknowledgementType { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
    }
}
