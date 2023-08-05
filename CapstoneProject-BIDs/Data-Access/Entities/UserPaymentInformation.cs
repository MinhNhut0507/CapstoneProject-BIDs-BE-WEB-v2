using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class UserPaymentInformation
    {
        public UserPaymentInformation()
        {
            PaymentStaffs = new HashSet<PaymentStaff>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PayPalAccount { get; set; }
        public bool Status { get; set; }

        public virtual Users User { get; set; }
        public virtual ICollection<PaymentStaff> PaymentStaffs { get; set; }
    }
}
