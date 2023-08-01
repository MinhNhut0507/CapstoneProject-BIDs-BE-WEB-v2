using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class NotificationType
    {
        public NotificationType()
        {
            StaffNotificationDetails = new HashSet<StaffNotificationDetail>();
            UserNotificationDetails = new HashSet<UserNotificationDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<StaffNotificationDetail> StaffNotificationDetails { get; set;}
        public virtual ICollection<UserNotificationDetail> UserNotificationDetails { get; set; }
    }
}
