using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BanHistoryModule.Response
{
    public class BanHistoryResponseAdminAndStaff
    {
        public Guid BanId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Reason { get; set; }
        public DTODateTime UpdateDate { get; set; }
        public DTODateTime CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
