using Common.Helper;
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
        public DTODateTime CreateDate { get; set; }
        public DTODateTime UpdateDate { get; set; }
        public string Status { get; set; }
    }
}
