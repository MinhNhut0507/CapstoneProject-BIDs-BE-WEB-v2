using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.PaymentStaffModule.Response
{
    public class PaymentStaffResponse
    {
        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public Guid SessionId { get; set; }
        public Guid UserPaymentInformationId { get; set; }
        public Guid PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public bool Status { get; set; }
    }
}
