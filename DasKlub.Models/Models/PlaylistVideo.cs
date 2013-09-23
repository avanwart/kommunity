using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class PlaylistVideo
    {
        [Key]
        public int playlistVideoID { get; set; }
        public int playlistID { get; set; }
        public int videoID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<short> rankOrder { get; set; }
        public virtual Playlist Playlist { get; set; }
        public virtual Video Video { get; set; }
    }
}
