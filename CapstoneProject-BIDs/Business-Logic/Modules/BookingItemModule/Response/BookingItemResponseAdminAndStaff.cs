using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BookingItemModule.Response
{
    public class BookingItemResponseAdminAndStaff
    {
        public Guid BookingItemId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string StaffName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
