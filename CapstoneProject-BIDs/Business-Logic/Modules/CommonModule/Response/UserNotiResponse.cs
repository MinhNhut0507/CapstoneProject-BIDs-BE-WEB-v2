using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CommonModule.Response
{
    public class UserNotiResponse
    {
        public UserNotificationDetail UserNotificationDetail { get; set; }
        public Notification Notification { get; set; }
    }
}
