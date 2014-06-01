using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Artist
    {
        public Artist()
        {
            ArtistProperties = new List<ArtistProperty>();
            ArtistEvents = new List<ArtistEvent>();
            Songs = new List<Song>();
        }

        [Key]
        public int artistID { get; set; }

        public string name { get; set; }
        public string altName { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public virtual ICollection<ArtistProperty> ArtistProperties { get; set; }
        public virtual ICollection<ArtistEvent> ArtistEvents { get; set; }
        public virtual ICollection<Song> Songs { get; set; }
    }
}