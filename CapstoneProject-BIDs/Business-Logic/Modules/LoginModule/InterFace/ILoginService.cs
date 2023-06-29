using Business_Logic.Modules.LoginModule.Data;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Request;
using Data_Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic.Modules.LoginModule.InterFace
{
    public interface ILoginService
    {
        //public Task<string> Login(LoginRequest loginRequest);
        //public bool CheckLogin(Login login);
        //public Task<bool> Login(Login login);
        //public Task<TokenModel> GenerateToken(LoginRespone account);
        //public ClaimsPrincipal EncrypToken(string token);
        public ReturnAccountLogin Login(LoginRequest loginRequest);
        public ClaimsPrincipal EncrypToken(string Token);
        public Task ResetPassword(string email);
        public Task sendemail(string email);
        public Task<User> CreateAccount(CreateUserRequest createUserRequest);
    }
}
