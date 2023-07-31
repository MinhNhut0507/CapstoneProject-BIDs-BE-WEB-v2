using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.NotificationModule.Response
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool? Status { get; set; }
    }
}
