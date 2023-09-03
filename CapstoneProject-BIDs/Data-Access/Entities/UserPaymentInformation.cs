using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class UserPaymentInformation
    {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PayPalAccount { get; set; }
        public bool Status { get; set; }

        public virtual Users User { get; set; }
    }
}
