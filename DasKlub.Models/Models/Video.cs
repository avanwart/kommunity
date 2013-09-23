using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Video
    {
        public Video()
        {
            this.ContestVideos = new List<ContestVideo>();
            this.PlaylistVideos = new List<PlaylistVideo>();
            this.UserAccountVideos = new List<UserAccountVideo>();
            this.VideoSongs = new List<VideoSong>();
            this.MultiProperties = new List<MultiProperty>();
        }
        [Key]
        public int videoID { get; set; }
        public string videoKey { get; set; }
        public string providerKey { get; set; }
        public string providerUserKey { get; set; }
        public string providerCode { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public bool isHidden { get; set; }
        public bool isEnabled { get; set; }
        public Nullable<int> statusID { get; set; }
        public Nullable<double> duration { get; set; }
        public Nullable<double> intro { get; set; }
        public Nullable<double> lengthFromStart { get; set; }
        public Nullable<byte> volumeLevel { get; set; }
        public bool enableTrim { get; set; }
        public Nullable<System.DateTime> publishDate { get; set; }
        public virtual ICollection<ContestVideo> ContestVideos { get; set; }
        public virtual ICollection<PlaylistVideo> PlaylistVideos { get; set; }
        public virtual ICollection<UserAccountVideo> UserAccountVideos { get; set; }
        public virtual ICollection<VideoSong> VideoSongs { get; set; }
        public virtual ICollection<MultiProperty> MultiProperties { get; set; }
    }
}
