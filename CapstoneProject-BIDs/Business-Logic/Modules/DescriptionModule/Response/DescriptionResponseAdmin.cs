using Data_Access.Entities;
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
        public bool Status { get; set; }
    }
}
