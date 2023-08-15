using Azure.Messaging;
using Business_Logic.Modules.LoginModule.InterFace;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Business_Logic.Modules.LoginModule.Data;
using Microsoft.Extensions.Configuration;
using Business_Logic.Modules.AdminModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Data_Access.Enum;

namespace Business_Logic.Modules.LoginModule
{
    public class LoginService : ILoginService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IStaffRepository _StaffRepository;
        private readonly IAdminRepository _AdminRepository;
        private readonly IConfiguration _configuration;
        public LoginService(IUserRepository UserRepository
            , IStaffRepository StaffRepository
            , IConfiguration configuration
            , IAdminRepository adminRepository)
        {
            _UserRepository = UserRepository;
            _StaffRepository = StaffRepository;
            _configuration = configuration;
            _AdminRepository = adminRepository;
        }
        public ReturnAccountLogin Login(LoginRequest loginRequest)
        {
            ValidationResult result = new LoginRequestValidator().Validate(loginRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var UserCheckLogin =  _UserRepository.GetFirstOrDefaultAsync(x => x.Email == loginRequest.Email
            && x.Password == loginRequest.Password && x.Status == (int)UserStatusEnum.Acctive).Result;
            var StaffCheckLogin = _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == loginRequest.Email
            && x.Password == loginRequest.Password).Result;
            var AdminCheckLogin = _AdminRepository.GetFirstOrDefaultAsync(x => x.Email == loginRequest.Email
            && x.Password == loginRequest.Password).Result;
            ReturnAccountLogin accountLogin = new ReturnAccountLogin();
            if (StaffCheckLogin != null)
            {
                accountLogin.Id = StaffCheckLogin.Id;
                accountLogin.Email = loginRequest.Email;
                accountLogin.Role = "Staff";
                return accountLogin;
            }
            if (AdminCheckLogin != null)
            {
                accountLogin.Id = AdminCheckLogin.Id;
                accountLogin.Email = loginRequest.Email;
                accountLogin.Role = "Admin";
                return accountLogin;
            }
            if (UserCheckLogin != null)
            {
                accountLogin.Id = UserCheckLogin.Id;
                accountLogin.Email = loginRequest.Email;
                if(UserCheckLogin.Role == (int)RoleEnum.User)
                {
                    accountLogin.Role = "User";
                }
                else
                {
                    throw new Exception("Tài khoản của bạn đang chờ xác nhận từ nhân viên hệ thống. Vui lòng kiểm tra email để không bỏ lỡ thông tin về tài khoản của bạn từ hệ thống");
                }
                return accountLogin;
            }
            return null;
        }
        public ClaimsPrincipal EncrypToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            //Decode JWT
            var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecurityKey"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            //Access the claim 
            return claims;
        }

        public async Task ResetPassword(string email)
        {
            try
            {
                if (email == null)
                {
                    throw new Exception(ErrorMessage.CommonError.EMAIL_IS_NULL);
                }

                var userReset = _UserRepository.GetFirstOrDefaultAsync(x => x.Email == email).Result;

                var staffReset = _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == email).Result;

                if (userReset == null && staffReset == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }


                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";
                string randomPassword = RandomString(10);

                string sendto = email;
                string subject = "[BIDs] - Khôi phục mật khẩu";

                string content = "";

                if(userReset != null)
                {
                    content = "Mật khẩu mới cho tài khoản đăng nhập " + userReset.Email + " : " + randomPassword + ".\r\nĐường link quay lại trang đăng nhập: ";
                    userReset.Password = randomPassword;
                    await _UserRepository.UpdateAsync(userReset);
                }
                else
                {
                    content = "Mật khẩu mới cho tài khoản đăng nhập " + staffReset.Email + " : " + randomPassword + ".\r\nĐường link quay lại trang đăng nhập: ";
                    staffReset.Password = randomPassword;
                    await _StaffRepository.UpdateAsync(staffReset);
                }

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public async Task sendemail(string email)
        {
            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = email;
            string subject = "[BIDs] - Khôi phục mật khẩu";

            string content = "abc";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(_gmail);
            mail.To.Add(email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;

            mail.Priority = MailPriority.High;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }
    }
}
