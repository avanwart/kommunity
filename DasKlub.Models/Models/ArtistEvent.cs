using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class ArtistEvent
    {
        [Key]
        public int artistID { get; set; }

        public int eventID { get; set; }
        public byte? rankOrder { get; set; }
        public virtual Artist Artist { get; set; }
        public virtual Event Event { get; set; }
    }
}