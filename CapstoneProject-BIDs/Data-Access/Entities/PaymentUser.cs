using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentUser
    {
        public Guid Id { get; set; }
        public Guid MethodId { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }

        public virtual PaymentMethodUser Method { get; set; }
        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
    }
}
