using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Request;
using Business_Logic.Modules.UserModule.Response;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.UserModule.Interface
{
    public interface IUserService
    {
        public Task<ReturnUserList> AddNewUser(CreateUserRequest UserCreate);

        public Task<ReturnUserList> UpdateUser(UpdateUserRequest UserUpdate);
        public Task<ReturnUserList> UpdatePassword(UpdatePasswordRequest UserUpdate);
        public Task<ReturnUserList> UpdateRoleAccount(Guid id);

        public Task<ICollection<Users>> GetAll();

        public Task<ICollection<Users>> GetUsersIsActive();

        public Task<ICollection<Users>> GetUsersIsBan();

        public Task<ICollection<Users>> GetUsersIsWaitting();

        public Task<ReturnUserList> GetUserByID(Guid? id);

        public Task<ReturnUserList> GetUserByName(string Name);

        public Task<ReturnUserList> GetUserByEmail(string Email);

    }
}
