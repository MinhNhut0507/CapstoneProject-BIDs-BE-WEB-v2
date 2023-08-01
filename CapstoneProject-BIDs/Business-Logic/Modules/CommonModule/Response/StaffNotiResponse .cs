using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Response
{
    public class StaffNotiResponse
    {
        public StaffNotificationDetail StaffNotificationDetail { get; set; }
        public Notification Notification { get; set; }
    }
}
