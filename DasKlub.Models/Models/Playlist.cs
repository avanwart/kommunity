using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class Playlist
    {
        public Playlist()
        {
            PlaylistVideos = new List<PlaylistVideo>();
        }

        [Key]
        public int playlistID { get; set; }

        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public DateTime? playlistBegin { get; set; }
        public string playListName { get; set; }
        public int userAccountID { get; set; }
        public bool autoPlay { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
    }
}