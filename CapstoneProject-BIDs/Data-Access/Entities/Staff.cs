using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Staff
    {
        public Staff()
        {
            BookingItems = new HashSet<BookingItem>();
            PaymentMethodStaffs = new HashSet<PaymentMethodStaff>();
            PaymentStaffs = new HashSet<PaymentStaff>();
        }

        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<BookingItem> BookingItems { get; set; }
        public virtual ICollection<PaymentMethodStaff> PaymentMethodStaffs { get; set; }
        public virtual ICollection<PaymentStaff> PaymentStaffs { get; set; }
    }
}
