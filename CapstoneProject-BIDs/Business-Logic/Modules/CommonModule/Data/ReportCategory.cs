using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportCategory
    {
        public int TotalCount { get; set; }
        public int InStageCount { get; set; }
        public int NotStartCount { get; set; }
        public int HaventTranferCount { get; set; }
        public int FailCount { get; set; }
        public int CompleteCount { get; set; }
        public int ReceiveCount { get; set; }
        public int ErrorItemCount { get; set; }
    }
}
