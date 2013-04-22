using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Category
    {
        public int categoryID { get; set; }
        public string categoryKey { get; set; }
        public int departmentID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
    }
}
