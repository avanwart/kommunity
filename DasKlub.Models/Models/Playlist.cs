using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public partial class Playlist
    {
        public Playlist()
        {
            this.PlaylistVideos = new List<PlaylistVideo>();
        }

                [Key]

        public int playlistID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<System.DateTime> playlistBegin { get; set; }
        public string playListName { get; set; }
        public int userAccountID { get; set; }
        public bool autoPlay { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
    }
}
