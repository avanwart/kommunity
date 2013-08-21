using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ErrorLog
    {
        public int errorLogID { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string message { get; set; }
        public string url { get; set; }
        public Nullable<int> responseCode { get; set; }
    }
}
