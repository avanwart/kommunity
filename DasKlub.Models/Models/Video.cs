using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Video
    {
        public Video()
        {
            ContestVideos = new List<ContestVideo>();
            PlaylistVideos = new List<PlaylistVideo>();
            UserAccountVideos = new List<UserAccountVideo>();
            VideoSongs = new List<VideoSong>();
            MultiProperties = new List<MultiProperty>();
        }

        [Key]
        public int videoID { get; set; }

        public string videoKey { get; set; }
        public string providerKey { get; set; }
        public string providerUserKey { get; set; }
        public string providerCode { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public bool isEnabled { get; set; }
        public int? statusID { get; set; }
        public double? duration { get; set; }
        public double? intro { get; set; }
        public double? lengthFromStart { get; set; }
        public byte? volumeLevel { get; set; }
        public bool enableTrim { get; set; }
        public DateTime? publishDate { get; set; }
        public virtual ICollection<ContestVideo> ContestVideos { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
        public virtual ICollection<UserAccountVideo> UserAccountVideos { get; set; }
        public virtual ICollection<VideoSong> VideoSongs { get; set; }
        public virtual ICollection<MultiProperty> MultiProperties { get; set; }
    }
}