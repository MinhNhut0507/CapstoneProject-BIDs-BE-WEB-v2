﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentStaff
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

        public virtual UserPaymentInformation PaymentInformation { get; set; }
        public virtual Session Session { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
