using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DasKlub.Web.Models.Domain
{
    public class StateInfo
    {
        public int CreatedByUserID { get; set; }

        public int? UpdatedByUserID { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}
