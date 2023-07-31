﻿using Business_Logic.Modules.ImageModule.Response;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using Common.Helper;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Business_Logic.Modules.SessionModule.Response
{
    public class SessionResponse
    {
        public SessionResponse()
        {
            Images = new HashSet<ImageResponseOrther>();
            Descriptions = new HashSet<ItemDescriptionResponse>();
        }

        public Guid SessionId { get; set; }
        public int FeeId { get; set; }
        public string FeeName { get; set; }
        public string SessionName { get; set; }
        public ICollection<ImageResponseOrther> Images { get; set; }
        public ICollection<ItemDescriptionResponse> Descriptions { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public double ParticipationFee { get; set; }
        public double Deposit { get; set; }
        public string Description { get; set; }
        public TimeSpan FreeTime { get; set; }
        public TimeSpan DelayTime { get; set; }
        public TimeSpan DelayFreeTime { get; set; }
        public DateTime BeginTime { get; set; }
       public DateTime EndTime { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
        public double FinalPrice { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Status { get; set; }
    }
}
