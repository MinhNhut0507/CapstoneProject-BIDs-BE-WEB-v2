using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Notification
    {
        public Notification()
        {
            StaffNotificationDetails = new HashSet<StaffNotificationDetail>();
            UserNotificationDetails = new HashSet<UserNotificationDetail>();
        }

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool? Status { get; set; }

        public virtual ICollection<StaffNotificationDetail> StaffNotificationDetails { get; set; }
        public virtual ICollection<UserNotificationDetail> UserNotificationDetails { get; set; }

    }
}
