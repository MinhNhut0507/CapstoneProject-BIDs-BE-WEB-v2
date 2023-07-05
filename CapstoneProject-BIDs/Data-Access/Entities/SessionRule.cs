using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access.Entities
{
    public partial class SessionRule
    {
        public SessionRule()
        {
            Sessions = new HashSet<Session>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int IncreaseTime { get; set; }
        public TimeSpan FreeTime { get; set; }
        public TimeSpan DelayTime { get; set; }
        public TimeSpan DelayFreeTime { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }
    }
}
