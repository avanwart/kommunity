using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class VideoSong
    {
        [Key]
        public int videoID { get; set; }

        public int songID { get; set; }
        public byte? rankOrder { get; set; }
        public virtual Song Song { get; set; }
        public virtual Video Video { get; set; }
    }
}