using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Status
    {
        [Key]
        public int statusID { get; set; }

        public string statusDescription { get; set; }
        public string statusCode { get; set; }
    }
}