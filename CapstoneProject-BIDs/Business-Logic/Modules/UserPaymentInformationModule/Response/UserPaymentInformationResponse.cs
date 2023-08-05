using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserPaymentInformationModule.Response
{
    public class UserPaymentInformationResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PayPalAccount { get; set; }
        public bool Status { get; set; }
    }
}
