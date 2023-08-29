using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Data
{
    public class ReportPaymentUser
    {
        public ReportPaymentUser()
        {
            PaymentReport = new HashSet<PaymentReport>();
        }

        public double TotalSend { get; set; }
        public double TotalReceive { get; set; }
        public ICollection<PaymentReport> PaymentReport { get; set; }
    }

    public class PaymentReport
    {
        public DateTime PaymentTime { get; set; }
        public double PaymentTotal { get; set; }
        public string PaymentID { get; set; }
        public string SessionName { get; set; }
        public string PaymentContent { get; set; }
        public string PayPalAccount { get; set; }
        public bool IsReceive { get; set; }
    }
}
