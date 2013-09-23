using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public partial class Color
    {
        [Key]
        public int colorID { get; set; }
        public string name { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public Nullable<int> siteDomainID { get; set; }
    }
}
