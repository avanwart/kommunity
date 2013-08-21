using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class VideoLog
    {
        public int videoLogID { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public int videoID { get; set; }
        public string ipAddress { get; set; }
    }
}
