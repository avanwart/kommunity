using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class ArtistEvent
    {
        [Key]
        public int artistID { get; set; }
        public int eventID { get; set; }
        public Nullable<byte> rankOrder { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Event Event { get; set; }
    }
}
