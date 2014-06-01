using System;
using System.ComponentModel.DataAnnotations;

namespace DasKlubModel.Models
{
    public class Acknowledgement
    {
        [Key]
        public int acknowledgementID { get; set; }

        public int userAccountID { get; set; }
        public int statusUpdateID { get; set; }
        public int? updatedByUserID { get; set; }
        public DateTime createDate { get; set; }
        public DateTime? updateDate { get; set; }
        public int? createdByUserID { get; set; }
        public string acknowledgementType { get; set; }
        public virtual StatusUpdate StatusUpdate { get; set; }
    }
}