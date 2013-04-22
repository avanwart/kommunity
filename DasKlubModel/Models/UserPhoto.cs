using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class UserPhoto
    {
        public int userPhotoID { get; set; }
        public int userAccountID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> updatedByUserID { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public string picURL { get; set; }
        public string thumbPicURL { get; set; }
        public string description { get; set; }
        public Nullable<byte> rankOrder { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
