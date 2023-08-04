using Business_Logic.Modules.ImageModule.Response;
using Business_Logic.Modules.ItemDescriptionModule.Response;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.SessionModule.Response
{
    public class SessionWinnerResponse
    {
        
        public SessionResponseComplete sessionResponseCompletes { get; set; }
        public string winner { get; set; }
    }
}
