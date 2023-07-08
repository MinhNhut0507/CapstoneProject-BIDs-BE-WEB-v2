﻿using Business_Logic.Modules.DescriptionModule.Response;
using Common.Helper;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.CategoryModule.Response
{
    public class CategoryResponseAdmin
    {
        public CategoryResponseAdmin()
        {
            Descriptions = new HashSet<DescriptionResponse>();
        }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public virtual ICollection<DescriptionResponse> Descriptions { get; set; }
        public DTODateTime UpdateDate { get; set; }
        public DTODateTime CreateDate { get; set; }
        public bool Status { get; set; }
    }
}
