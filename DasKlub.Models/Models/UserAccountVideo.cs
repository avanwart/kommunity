using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class UserAccountVideo
    {
        public int videoID { get; set; }
        public int userAccountID { get; set; }
        public System.DateTime createDate { get; set; }
        public string videoType { get; set; }
        public virtual UserAccount UserAccount { get; set; }
        public virtual Video Video { get; set; }
    }
}
