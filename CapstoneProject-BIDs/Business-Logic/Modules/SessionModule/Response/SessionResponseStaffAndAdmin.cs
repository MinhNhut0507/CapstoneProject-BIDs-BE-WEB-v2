﻿using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Response
{
    public class SessionResponseStaffAndAdmin
    {
        public Guid SessionId { get; set; }
        public Guid FeeId { get; set; }
        public string FeeName { get; set; }
        public string SessionName { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime BeginTime { get; set; }
        public TimeSpan AuctionTime { get; set; }
        public DateTime EndTime { get; set; }
        public double FinalPrice { get; set; }
        public int Status { get; set; }
    }
}