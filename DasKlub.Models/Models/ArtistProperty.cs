using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ArtistProperty
    {
        public int artistPropertyID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public int artistID { get; set; }
        public string propertyContent { get; set; }
        public string propertyType { get; set; }
        public virtual Artist Artist { get; set; }
    }
}
