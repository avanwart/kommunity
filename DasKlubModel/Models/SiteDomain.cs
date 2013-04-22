using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class SiteDomain
    {
        public int siteDomainID { get; set; }
        public string propertyType { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public string language { get; set; }
        public string description { get; set; }
    }
}