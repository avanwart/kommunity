using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DasKlub.Models.Domain;

namespace DasKlub.Models.Funding
{
    public class Donation : StateInfo
    {
        public decimal Amount { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
