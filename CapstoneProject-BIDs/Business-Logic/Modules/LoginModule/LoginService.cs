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
            && x.Password == loginRequest.Password).Result;
            var StaffCheckLogin = _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == loginRequest.Email
            && x.Password == loginRequest.Password).Result;
            var AdminCheckLogin = _AdminRepository.GetFirstOrDefaultAsync(x => x.Email == loginRequest.Email
            && x.Password == loginRequest.Password).Result;
            ReturnAccountLogin accountLogin = new ReturnAccountLogin();
            if (StaffCheckLogin != null)
            {
                accountLogin.Email = loginRequest.Email;
                accountLogin.Role = "Staff";
                return accountLogin;
            }
            if (AdminCheckLogin != null)
            {
                accountLogin.Email = loginRequest.Email;
                accountLogin.Role = "Admin";
                return accountLogin;
            }
            if (UserCheckLogin != null)
            {
                accountLogin.Email = loginRequest.Email;
                if(UserCheckLogin.Role == (int)RoleEnum.Auctioneer)
                {
                    accountLogin.Role = "Auctioneer";
                }
                else
                {
                    accountLogin.Role = "Bidder";
                }
                return accountLogin;
            }
            throw new Exception(ErrorMessage.LoginError.WRONG_ACCOUNT_NAME_OR_PASSWORD);
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

        public async Task<User> CreateAccount(CreateUserRequest userRequest)
        {

            ValidationResult result = new CreateUserRequestValidator().Validate(userRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            User userCheckEmail = _UserRepository.GetFirstOrDefaultAsync(x => x.Email == userRequest.Email).Result;
            if (userCheckEmail != null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_EXITED);
            }
            User userCheckPhone = _UserRepository.GetFirstOrDefaultAsync(x => x.Phone == userRequest.Phone).Result;
            if (userCheckPhone != null)
            {
                throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
            }
            User userCheckCCCDNumber = _UserRepository.GetFirstOrDefaultAsync(x => x.Cccdnumber == userRequest.Cccdnumber).Result;
            if (userCheckCCCDNumber != null)
            {
                throw new Exception(ErrorMessage.CommonError.CCCD_NUMBER_IS_EXITED);
            }

            if (!userRequest.Email.Contains("@"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_EMAIL_FORMAT);
            }
            if ((!userRequest.Phone.StartsWith("09")
                && !userRequest.Phone.StartsWith("08")
                && !userRequest.Phone.StartsWith("07")
                && !userRequest.Phone.StartsWith("05")
                && !userRequest.Phone.StartsWith("03"))
                || userRequest.Phone.Length != 10)
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
            }
            if (userRequest.Cccdnumber.Length != 12
                || !userRequest.Cccdnumber.StartsWith("0"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_CCCD_NUMBER_FORMAT);
            }

            var newUser = new User();

            newUser.Id = Guid.NewGuid();
            newUser.Name = userRequest.UserName;
            newUser.Avatar = userRequest.UserName;
            newUser.Role = (int)RoleEnum.Bidder;
            newUser.Email = userRequest.Email;
            newUser.Password = userRequest.Password;
            newUser.Address = userRequest.Address;
            newUser.Phone = userRequest.Phone;
            newUser.DateOfBirth = userRequest.DateOfBirth;
            newUser.Cccdnumber = userRequest.Cccdnumber;
            newUser.CccdfrontImage = userRequest.CccdfrontImage;
            newUser.CccdbackImage = userRequest.CccdbackImage;
            newUser.CreateDate = DateTime.Now;
            newUser.UpdateDate = DateTime.Now;
            newUser.Status = (int)UserStatusEnum.Waitting;

            await _UserRepository.AddAsync(newUser);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = userRequest.Email;
            string subject = "BIDs - Tạo Tài Khoản";

            string content = "Tài khoản " + userRequest.Email + " đã được tạo thành công và đang đợi xét duyệt từ nhân viên hệ thống";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(_gmail);
            mail.To.Add(userRequest.Email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;

            mail.Priority = MailPriority.High;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
            return newUser;
        }
    }
}
