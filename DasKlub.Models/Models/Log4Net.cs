using System;
using System.Collections.Generic;

namespace DasKlubModel.Models
{
    public partial class Log4Net
    {
        public int Id { get; set; }
        public System.DateTime Date { get; set; }
        public string Thread { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Location { get; set; }
    }
}
