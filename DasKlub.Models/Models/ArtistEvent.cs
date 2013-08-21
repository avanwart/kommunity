using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ArtistEvent
    {
        public int artistID { get; set; }
        public int eventID { get; set; }
        public Nullable<byte> rankOrder { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Event Event { get; set; }
    }
}
