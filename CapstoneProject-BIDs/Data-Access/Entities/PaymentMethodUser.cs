using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class PaymentMethodUser
    {
        public PaymentMethodUser()
        {
            PaymentUsers = new HashSet<PaymentUser>();
        }

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Number { get; set; }
        public string BankName { get; set; }
        public string OwnerName { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<PaymentUser> PaymentUsers { get; set; }
    }
}
