using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentStaff
    {
        public Guid Id { get; set; }
        public Guid MethodId { get; set; }
        public Guid StaffId { get; set; }
        public Guid? SessionId { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public virtual PaymentMethodStaff Method { get; set; }
        public virtual Session Session { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
