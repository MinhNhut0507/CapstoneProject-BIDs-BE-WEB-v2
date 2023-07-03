using System;
using System.Collections.Generic;

#nullable disable

namespace Data_Access.Entities
{
    public partial class BookingItem
    {
        public Guid Id { get; set; }
        public Guid ItemId { get; set; }
        public Guid StaffId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; }

        public virtual Item Item { get; set; }
        public virtual Staff Staff { get; set; }
    }
}
