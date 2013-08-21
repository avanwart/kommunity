using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class WishList
    {
        public int productID { get; set; }
        public int createdByUserID { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
