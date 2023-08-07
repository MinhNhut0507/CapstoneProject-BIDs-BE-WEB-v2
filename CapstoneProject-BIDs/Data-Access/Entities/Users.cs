using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Users
    {
        public Users()
        {
            BanHistories = new HashSet<BanHistory>();
            Items = new HashSet<Item>();
            UserPaymentInformations = new HashSet<UserPaymentInformation>();
            PaymentUsers = new HashSet<PaymentUser>();
            SessionDetails = new HashSet<SessionDetail>();
            UserNotificationDetails = new HashSet<UserNotificationDetail>();
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
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<SessionDetail> SessionDetails { get; set; }
        public virtual ICollection<UserPaymentInformation> UserPaymentInformations { get; set; }
        public virtual ICollection<PaymentUser> PaymentUsers { get; set; }
        public virtual ICollection<UserNotificationDetail> UserNotificationDetails { get; set; }

    }
}
