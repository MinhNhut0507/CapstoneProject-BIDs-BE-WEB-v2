using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.StaffModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.StaffModule.Interface
{
    public interface IStaffService
    {
        public Task<Staff> AddNewStaff(CreateStaffRequest StaffCreate);

        public Task<Staff> UpdateStaff(UpdateStaffRequest StaffUpdate);
        public Task<Staff> UpdatePassword(UpdatePasswordRequest StaffUpdate);

        public Task<Staff> DeleteStaff(Guid? StaffDeleteID);

        public Task<ICollection<Staff>> GetAll();

        public Task<Staff> GetStaffByID(Guid? id);

        public Task<Staff> GetStaffByName(string Name);

        public Task<Staff> GetStaffByEmail(string Email);

        public Task<Users> AcceptCreateAccount(Guid? CreateAccountID);

        public Task<Users> DenyCreate(Guid? CreateAccountID);

        public Task<Users> BanUser(Guid? BanUserID);

        public Task<Users> UnbanUser(Guid? UnbanUserID);

    }
}
