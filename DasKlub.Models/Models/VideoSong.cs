using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class VideoSong
    {
        [Key]
        public int videoID { get; set; }
        public int songID { get; set; }
        public Nullable<byte> rankOrder { get; set; }
        public virtual Song Song { get; set; }
        public virtual Video Video { get; set; }
    }
}
