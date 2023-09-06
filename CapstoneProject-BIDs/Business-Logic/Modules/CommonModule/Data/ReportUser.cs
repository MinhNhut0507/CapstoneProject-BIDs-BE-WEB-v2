using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportUser
    {
        public int TotalCount { get; set; }
        public int TotalAccountWaiting { get; set; }
        public int TotalAccountBanned { get; set; }
        public int TotalAccountAccepted { get; set; }
        public int TotalAccountRejected { get; set;}
    }
}
