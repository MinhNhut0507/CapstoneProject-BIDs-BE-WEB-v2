using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffNotificationDetailModule.Response
{
    public class StaffNotificationDetailResponseAdmin
    {
        public Guid NotificationId { get; set; }
        public string Message { get; set; }
        public Guid StaffId { get; set; }
        public string TypeName { get; set; }
    }
}
