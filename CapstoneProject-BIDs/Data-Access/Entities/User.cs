using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class User
    {
        public User()
        {
            BanHistories = new HashSet<BanHistory>();
            BookingItems = new HashSet<BookingItem>();
            Items = new HashSet<Item>();
            PaymentMethodUsers = new HashSet<PaymentMethodUser>();
            PaymentUsers = new HashSet<PaymentUser>();
        }

        public Guid Id { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Cccdnumber { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string CccdfrontImage { get; set; }
        public string CccdbackImage { get; set; }
        public int Status { get; set; }

        public virtual ICollection<BanHistory> BanHistories { get; set; }
        public virtual ICollection<BookingItem> BookingItems { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<PaymentMethodUser> PaymentMethodUsers { get; set; }
        public virtual ICollection<PaymentUser> PaymentUsers { get; set; }
    }
}
