using Business_Logic.Modules.DescriptionModule.Response;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CategoryModule.Response
{
    public class CategoryResponseUserAndStaff
    {
        public string CategoryName { get; set; }
        public ICollection<DescriptionResponse> Description { get; set; }
    }
}
