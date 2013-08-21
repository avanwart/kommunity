using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            this.PlaylistVideos = new List<PlaylistVideo>();
        }

        public int playlistID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<System.DateTime> playlistBegin { get; set; }
        public string playListName { get; set; }
        public int userAccountID { get; set; }
        public bool autoPlay { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
    }
}
