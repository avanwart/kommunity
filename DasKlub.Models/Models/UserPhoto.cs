using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class UserPhoto
    {
        [Key]
        public int userPhotoID { get; set; }

        public int userAccountID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? updatedByUserID { get; set; }
        public int? createdByUserID { get; set; }
        public string picURL { get; set; }
        public string thumbPicURL { get; set; }
        public string description { get; set; }
        public byte? rankOrder { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
    }
}