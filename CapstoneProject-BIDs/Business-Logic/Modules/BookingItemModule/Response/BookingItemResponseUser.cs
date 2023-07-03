using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BookingItemModule.Response
{
    public class BookingItemResponseUser
    {
        public string ItemName { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
