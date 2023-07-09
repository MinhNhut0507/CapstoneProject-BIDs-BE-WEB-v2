using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Request;
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
        public Task<Users> AddNewUser(CreateUserRequest UserCreate);

        public Task<Users> UpdateUser(UpdateUserRequest UserUpdate);
        public Task<Users> UpdatePassword(UpdatePasswordRequest UserUpdate);
        public Task<Users> UpdateRoleAccount(Guid id);

        public Task<ICollection<Users>> GetAll();

        public Task<ICollection<Users>> GetUsersIsActive();

        public Task<ICollection<Users>> GetUsersIsBan();

        public Task<ICollection<Users>> GetUsersIsWaitting();

        public Task<Users> GetUserByID(Guid? id);

        public Task<Users> GetUserByName(string Name);

        public Task<Users> GetUserByEmail(string Email);

    }
}
