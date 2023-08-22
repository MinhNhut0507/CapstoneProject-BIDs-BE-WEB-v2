using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentUser
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public string PayPalTransactionId { get; set; }
        public string PaymentDetail { get; set; }
        public double Amount { get; set; }
        public string PayPalAccount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; set; }

        public virtual Session Session { get; set; }
        public virtual Users User { get; set; }
    }
}
