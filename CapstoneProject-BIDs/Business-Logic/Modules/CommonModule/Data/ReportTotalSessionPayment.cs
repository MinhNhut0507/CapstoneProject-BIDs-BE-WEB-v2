using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportTotalSessionPayment
    {
        public int TotalCount { get; set; }
        public double TotalPrice { get; set; }
        public double TotalSend { get; set; }
        public double TotalReceive { get; set; }
    }
}
