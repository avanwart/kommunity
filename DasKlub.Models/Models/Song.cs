using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Song
    {
        public Song()
        {
            SongProperties = new List<SongProperty>();
            VideoSongs = new List<VideoSong>();
        }

        [Key]
        public int songID { get; set; }

        public int? artistID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public string name { get; set; }
        public string songKey { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ICollection<SongProperty> SongProperties { get; set; }
        public virtual ICollection<VideoSong> VideoSongs { get; set; }
    }
}