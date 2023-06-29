using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BanHistoryModule.Response
{
    public class BanHistoryResponseUser
    {
        public string UserName { get; set; }
        public string Reason { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
