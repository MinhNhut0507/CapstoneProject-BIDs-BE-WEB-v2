using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.DescriptionModule.Response
{
    public class DescriptionResponseAdmin
    {
        public Guid DescriptionId { get; set; }
        public string DescriptionName { get; set; }
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
