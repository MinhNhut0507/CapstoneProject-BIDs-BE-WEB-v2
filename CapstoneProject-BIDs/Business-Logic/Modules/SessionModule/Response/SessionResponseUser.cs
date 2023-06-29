using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Response
{
    public class SessionResponseUser
    {
        public string FeeName { get; set; }
        public string SessionName { get; set; }
        public string ItemName { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime AuctionTime { get; set; }
        public DateTime EndTime { get; set; }
        public double? FinailPrice { get; set; }
    }
}
