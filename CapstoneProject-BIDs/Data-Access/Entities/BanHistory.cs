using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class BanHistory
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Reason { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }

        public virtual User User { get; set; }
    }
}
