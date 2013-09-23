using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class WishList
    {
        [Key]
        public int productID { get; set; }
        public int createdByUserID { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
