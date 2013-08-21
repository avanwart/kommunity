using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Color
    {
        public int colorID { get; set; }
        public string name { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public Nullable<int> siteDomainID { get; set; }
    }
}
