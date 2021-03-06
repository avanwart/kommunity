using System;
using System.ComponentModel.DataAnnotations;
using DasKlub.Models.Models;

namespace DasKlubModel.Models
{
    public class UserAddress
    {
        [Key]
        public int userAddressID { get; set; }

        public string firstName { get; set; }
        public string middleName { get; set; }
        public string lastName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string addressLine3 { get; set; }
        public string city { get; set; }
        public string region { get; set; }
        public string postalCode { get; set; }
        public string countryISO { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public int? userAccountID { get; set; }
        public string addressStatus { get; set; }
        public string choice1 { get; set; }
        public string choice2 { get; set; }
        public virtual UserAccountEntity UserAccountEntity { get; set; }
    }
}