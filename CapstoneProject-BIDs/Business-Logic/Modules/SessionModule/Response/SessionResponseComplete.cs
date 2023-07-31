using Business_Logic.Modules.ImageModule.Response;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using Common.Helper;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Response
{
    public class SessionResponseComplete
    {
        public SessionResponseComplete()
        {
            Images = new HashSet<ImageResponseOrther>();
            Descriptions = new HashSet<ItemDescriptionResponse>();
        }

        public Guid SessionId { get; set; }
        public string FeeName { get; set; }
        public string Email { get; set; }
        public string SessionName { get; set; }
        public ICollection<ImageResponseOrther> Images { get; set; }
        public ICollection<ItemDescriptionResponse> Descriptions { get; set; }
        public double ParticipationFee { get; set; }
        public double Deposit { get; set; }
        public string ItemName { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public double FinalPrice { get; set; }
        public int Status { get; set; }
    }
}
