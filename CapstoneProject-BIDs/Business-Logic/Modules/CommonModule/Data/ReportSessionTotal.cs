using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportSessionTotal
    {
        public int TotalCount { get; set; }
        public double TotalPayment { get; set; }
        public int TotalCountNotStart { get; set; }
        public double TotalPaymentNotStart { get; set; }
        public int TotalCountInStage { get; set; }
        public double TotalPaymentInStage { get; set; }
        public int TotalCountHaventTranfer { get; set; }
        public double TotalPaymentHaventTranfer { get; set; }
        public int TotalCountComplete { get; set; }
        public double TotalPaymentComplete { get; set; }
        public int TotalCountFail { get; set; }
        public double TotalPaymentFail { get; set; }
        public int TotalCountReceived { get; set; }
        public double TotalPaymentReceived { get; set; }
        public int TotalCountErrorItem { get; set; }
        public double TotalPaymentErrorItem { get; set; }
        public int TotalCountDelete { get; set; }
        public double TotalPaymentDelete { get; set; }
    }
}
