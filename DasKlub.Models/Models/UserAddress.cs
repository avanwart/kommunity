using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class UserAddress
    {
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
        public Nullable<int> updatedByUserID { get; set; }
        public System.DateTime createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createdByUserID { get; set; }
        public Nullable<int> userAccountID { get; set; }
        public string addressStatus { get; set; }
        public string choice1 { get; set; }
        public string choice2 { get; set; }
        public virtual UserAccount UserAccount { get; set; }
    }
}
