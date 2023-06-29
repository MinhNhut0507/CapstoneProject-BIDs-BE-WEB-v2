using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionDetailModule.Response
{
    public class SessionDetailResponseStaffAndAdmin
    {
        public Guid SessionDetailId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public Guid SessionId { get; set; }
        public string SessionName { get; set; }
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
