using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Category
    {
        [Key]
        public int categoryID { get; set; }

        public string categoryKey { get; set; }
        public int departmentID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
    }
}