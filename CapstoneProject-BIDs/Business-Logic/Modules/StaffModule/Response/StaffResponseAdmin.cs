﻿using Common.Helper;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffModule.Response
{
    public class StaffResponseAdmin
    {
        public Guid StaffId { get; set; }
        public string StaffName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DTODateOfBirth DateOfBirth { get; set; }
        public DTODateTime CreateDate { get; set; }
        public DTODateTime UpdateDate { get; set; }
        public bool Status { get; set; }
    }
}
