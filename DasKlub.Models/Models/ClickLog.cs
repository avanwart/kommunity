using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class ClickLog
    {
        public int clickLogID { get; set; }
        public string ipAddress { get; set; }
        public string clickType { get; set; }
        public string referringURL { get; set; }
        public string currentURL { get; set; }
        public Nullable<int> productID { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public System.DateTime createDate { get; set; }
    }
}
