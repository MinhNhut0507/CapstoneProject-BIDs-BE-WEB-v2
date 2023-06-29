﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class AdminNotificationDetail
    {
        public Guid NotificationId { get; set; }
        public int TypeId { get; set; }
        public Guid AdminId { get; set; }
        public string Messages { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Notification Notification { get; set; }
        public virtual NotificationType Type { get; set; }
    }
}
