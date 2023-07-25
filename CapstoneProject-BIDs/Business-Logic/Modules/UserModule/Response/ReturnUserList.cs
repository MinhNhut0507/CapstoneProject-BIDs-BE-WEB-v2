using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule.Response
{
    public class ReturnUserList
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public ICollection<Users>? Users { get; set; }
    }
}
