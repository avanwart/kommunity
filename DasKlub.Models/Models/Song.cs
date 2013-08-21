using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Song
    {
        public Song()
        {
            this.SongProperties = new List<SongProperty>();
            this.VideoSongs = new List<VideoSong>();
        }

        public int songID { get; set; }
        public Nullable<int> artistID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public string name { get; set; }
        public string songKey { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual ICollection<SongProperty> SongProperties { get; set; }
        public virtual ICollection<VideoSong> VideoSongs { get; set; }
    }
}
