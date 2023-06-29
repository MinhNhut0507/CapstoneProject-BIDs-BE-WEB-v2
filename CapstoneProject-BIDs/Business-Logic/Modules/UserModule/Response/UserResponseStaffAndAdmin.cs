using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule.Response
{
    public class UserResponseStaffAndAdmin
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Cccdnumber { get; set; }
        public string CccdfrontImage { get; set; }
        public string CccdbackImage { get; set; }
        public int Status { get; set; }
    }
}
