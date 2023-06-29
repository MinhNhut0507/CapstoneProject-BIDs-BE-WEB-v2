using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class SessionDetail
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }
        public double Price { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

        public virtual Session Session { get; set; }
        public virtual User User { get; set; }
    }
}
