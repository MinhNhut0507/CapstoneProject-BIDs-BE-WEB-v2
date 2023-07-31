using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionRuleModule.Response
{
    public class SessionRuleResponse
    {
        public Guid SessionRuleId { get; set; }
        public string Name { get; set; }
        public TimeSpan FreeTime { get; set; }
        public TimeSpan DelayTime { get; set; }
        public TimeSpan DelayFreeTime { get; set; }
        public DTODateTime CreateDate { get; set; }
        public DTODateTime UpdateDate { get; set; }
        public bool Status { get; set; }
    }
}
