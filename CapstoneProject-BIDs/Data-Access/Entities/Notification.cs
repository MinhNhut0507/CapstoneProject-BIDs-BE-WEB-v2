using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class Notification
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public bool? Status { get; set; }
    }
}
