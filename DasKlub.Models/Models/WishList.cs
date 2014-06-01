using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class WishList
    {
        [Key]
        public int productID { get; set; }

        public int createdByUserID { get; set; }
        public DateTime createDate { get; set; }
    }
}