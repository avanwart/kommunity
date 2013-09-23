using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Artist
    {
        public Artist()
        {
            this.ArtistProperties = new List<ArtistProperty>();
            this.ArtistEvents = new List<ArtistEvent>();
            this.Songs = new List<Song>();
        }

        [Key]
        public int artistID { get; set; }
        public string name { get; set; }
        public string altName { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public virtual ICollection<ArtistProperty> ArtistProperties { get; set; }
        public virtual ICollection<ArtistEvent> ArtistEvents { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}
