using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class PlaylistVideo
    {
        [Key]
        public int playlistVideoID { get; set; }

        public int playlistID { get; set; }
        public int videoID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public short? rankOrder { get; set; }
        public virtual Playlist Playlist { get; set; }
        public virtual Video Video { get; set; }
    }
}