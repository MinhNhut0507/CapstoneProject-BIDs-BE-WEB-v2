using Business_Logic.Modules.DescriptionModule.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemModule.Response
{
    public class ItemResponseStaffAndAdmin
    {
        public Guid ItemId { get; set; }
        public string UserName { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public ICollection<DescriptionResponse> Descriptions { get; set; }
        public string DescriptionDetail { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public double FristPrice { get; set; }
        public double StepPrice { get; set; }
        public bool Deposit { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool Status { get; set; }
    }
}
