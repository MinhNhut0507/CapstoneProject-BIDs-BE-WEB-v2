using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using System.Net;
using System.Net.Mail;

namespace Business_Logic.Modules.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IUserNotificationDetailService _userNotificationDetailService;
        public UserService(IUserRepository UserRepository
            , IUserNotificationDetailService userNotificationDetailService)
        {
            _UserRepository = UserRepository;
            _userNotificationDetailService = userNotificationDetailService;
        }

        public async Task<ICollection<Users>> GetAll()
        {
            return await _UserRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Users>> GetUsersIsActive()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Acctive, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Users>> GetUsersIsBan()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Ban, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Users>> GetUsersIsWaitting()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Waitting, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<Users> GetUserByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return user;
        }

        public async Task<Users> GetUserByName(string userName)
        {
            if (userName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Name == userName);
            if (user == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return user;
        }

        public async Task<Users> GetUserByEmail(string email)
        {
            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return user;
        }

        public async Task<Users> AddNewUser(CreateUserRequest userRequest)
        {

            ValidationResult result = new CreateUserRequestValidator().Validate(userRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            Users userCheckEmail = _UserRepository.GetFirstOrDefaultAsync(x => x.Email == userRequest.Email).Result;

            if (userCheckEmail != null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_EXITED);
            }

            Users userCheckPhone = _UserRepository.GetFirstOrDefaultAsync(x => x.Phone == userRequest.Phone).Result;

            if (userCheckPhone != null)
            {
                throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
            }

            Users userCheckCCCDNumber = _UserRepository.GetFirstOrDefaultAsync(x => x.Cccdnumber == userRequest.Cccdnumber).Result;

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

            if(!userRequest.Avatar.EndsWith(".jpg")
                && !userRequest.Avatar.EndsWith(".png")
                && !userRequest.Avatar.EndsWith(".gif"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
            }

            var newUser = new Users();

            newUser.Id = Guid.NewGuid();
            newUser.Name = userRequest.UserName;
            newUser.Avatar = userRequest.Avatar;
            newUser.Role = (int)RoleEnum.Bidder;
            newUser.Email = userRequest.Email;
            newUser.Password = userRequest.Password;
            newUser.Address = userRequest.Address;
            newUser.Phone = userRequest.Phone;
            newUser.DateOfBirth = userRequest.DateOfBirth;
            newUser.Cccdnumber = userRequest.Cccdnumber;
            newUser.CccdfrontImage = userRequest.CccdfrontImage;
            newUser.CccdbackImage = userRequest.CccdbackImage;
            DateTime dateTime = DateTime.UtcNow;
            newUser.CreateDate = dateTime.AddHours(7);
            newUser.UpdateDate = dateTime.AddHours(7);
            newUser.Status = (int)UserStatusEnum.Waitting;

            await _UserRepository.AddAsync(newUser);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = userRequest.Email;
            string subject = "BIDs - Tạo Tài Khoản";

            string content = "Tài khoản " + userRequest.Email + " đã được tạo thành công và đang đợi xét duyệt từ nhân viên hệ thống trong vòng 48h.";

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

        public async Task<Users> UpdateUser(UpdateUserRequest userRequest)
        {
            try
            {
                var userUpdate = GetUserByID(userRequest.UserId).Result;

                if (userUpdate == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                ValidationResult result = new UpdateUserRequestValidator().Validate(userRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Users userCheckPhone = _UserRepository.GetFirstOrDefaultAsync(x => x.Phone == userRequest.Phone).Result;
                if (userCheckPhone != null)
                {
                    if(userCheckPhone.Id != userUpdate.Id)
                    {
                        throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
                    }
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

                if (!userRequest.Avatar.EndsWith(".jpg")
                && !userRequest.Avatar.EndsWith(".png")
                && !userRequest.Avatar.EndsWith(".gif"))
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
                }

                userUpdate.Name = userRequest.UserName;
                userUpdate.Password = userRequest.Password;
                userUpdate.Address = userRequest.Address;
                userUpdate.Phone = userRequest.Phone;
                userUpdate.Avatar = userRequest.Avatar;
                //userUpdate.Status = userRequest.Status;
                DateTime dateTime = DateTime.UtcNow;
                userUpdate.UpdateDate = dateTime.AddHours(7);

                await _UserRepository.UpdateAsync(userUpdate);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa được cập nhập thông tin",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);

                return userUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Users> UpdateRoleAccount(Guid id)
        {
            try
            {
                Users user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == id && x.Status == (int)UserStatusEnum.Acctive);
                if (user == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                user.Role = (int)RoleEnum.Auctioneer;
                DateTime dateTime = DateTime.UtcNow;
                user.UpdateDate = dateTime.AddHours(7);

                await _UserRepository.UpdateAsync(user);

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = user.Email;
                string subject = "BIDs - Nâng Cấp Tài Khoản";

                string content = "Tài khoản " + user.Email + " đã được nâng cấp. Bây giờ bạn có thể bán đấu giá các sản phẩm trên hệ thống";

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(user.Email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa được nâng cấp quyền",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> UpdatePassword(UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                Users user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == updatePasswordRequest.Id
                    && x.Password == updatePasswordRequest.OldPassword
                    && x.Status != -1
                    && x.Status != 2);
                if (user == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                ValidationResult result = new UpdatePasswordRequestValidator().Validate(updatePasswordRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                user.Password = updatePasswordRequest.NewPassword;
                DateTime dateTime = DateTime.UtcNow;
                user.UpdateDate = dateTime.AddHours(7);

                await _UserRepository.UpdateAsync(user);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa cập nhập mật khẩu",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);

                return user;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
