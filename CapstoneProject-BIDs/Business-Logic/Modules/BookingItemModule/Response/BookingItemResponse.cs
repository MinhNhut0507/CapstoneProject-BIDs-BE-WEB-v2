using Business_Logic.Modules.ImageModule.Response;
using Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.BookingItemModule.Response
{
    public class BookingItemResponse
    {
        public BookingItemResponse() 
        {
            Images = new HashSet<ImageResponseOrther>();
        }

        public Guid BookingItemId { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string StaffName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        public string CategoryName { get; set; }
        public string DescriptionDetail { get; set; }
        public int Quantity { get; set; }
        public ICollection<ImageResponseOrther> Images { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
        public bool Deposit { get; set; }
    }
}
