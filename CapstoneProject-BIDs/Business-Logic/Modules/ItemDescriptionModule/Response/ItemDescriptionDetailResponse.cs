using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemDescriptionModule.Response
{
    public class ItemDescriptionDetailResponse
    {
        public Guid ItemDescriptionId { get; set; }
        public string CategoryName { get; set; }
        public string DescriptionName { get; set; }
        public string Detail { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }   
        public int Status { get; set; }
    }
}
