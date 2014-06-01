using System;

namespace DasKlub.Models.Domain
{
    public class StateInfo
    {
        public int CreatedByUserID { get; set; }

        public int? UpdatedByUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}