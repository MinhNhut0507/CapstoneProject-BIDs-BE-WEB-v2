using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportSessionTotal
    {
        public int Total { get; set; }
        public int TotalComplete { get; set; }
        public int TotalFail { get; set; }
        public double TotalPrice { get; set; }
    }
}
