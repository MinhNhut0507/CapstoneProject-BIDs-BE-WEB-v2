﻿using Business_Logic.Modules.DescriptionModule.Response;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.ItemModule.Response
{
    public class ItemResponseUser
    {
        public ItemResponseUser()
        {
            Descriptions = new HashSet<ItemDescriptionResponse>();
        }
        public string UserName { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public ICollection<ItemDescriptionResponse> Descriptions { get; set; }
        public string DescriptionDetail { get; set; }
        public int Quantity { get; set; }
        public string Image { get; set; }
        public double FirstPrice { get; set; }
        public double StepPrice { get; set; }
        public bool Deposit { get; set; }
    }
}