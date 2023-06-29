using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentMethodStaff
    {
        public PaymentMethodStaff()
        {
            PaymentStaffs = new HashSet<PaymentStaff>();
        }

        public Guid Id { get; set; }
        public Guid StaffId { get; set; }
        public string Number { get; set; }
        public string BankName { get; set; }
        public string OwnerName { get; set; }
        public bool Status { get; set; }

        public virtual Staff Staff { get; set; }
        public virtual ICollection<PaymentStaff> PaymentStaffs { get; set; }
    }
}
