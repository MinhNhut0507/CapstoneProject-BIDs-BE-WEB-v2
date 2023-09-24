using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.DescriptionModule.Response
{
    public class DescriptionResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}
