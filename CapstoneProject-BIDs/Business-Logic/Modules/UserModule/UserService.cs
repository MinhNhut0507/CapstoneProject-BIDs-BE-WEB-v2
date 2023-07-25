using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Business_Logic.Modules.UserModule.Response;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
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

        public async Task<ReturnUserList> GetUserByID(Guid? id)
        {
            var returnUser = new ReturnUserList();
            if (id == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.ID_IS_NULL;
                return returnUser;
            }
            var User = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (User == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                return returnUser;
            }
            returnUser.Success = true;
            returnUser.Error = null;
            returnUser.Users = new List<Users>()
            {
                User
            };
            return returnUser;
        }

        public async Task<ReturnUserList> GetUserByName(string userName)
        {
            var returnUser = new ReturnUserList();
            if (userName == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.ID_IS_NULL;
                return returnUser;
            }
            var User = await _UserRepository.GetFirstOrDefaultAsync(x => x.Name == userName);
            if (User == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                return returnUser;
            }
            returnUser.Success = true;
            returnUser.Error = null;
            returnUser.Users = new List<Users>()
            {
                User
            };
            return returnUser;
        }

        public async Task<ReturnUserList> GetUserByEmail(string email)
        {
            var returnUser = new ReturnUserList();
            if (email == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.ID_IS_NULL;
                return returnUser;
            }
            var User = await _UserRepository.GetFirstOrDefaultAsync(x => x.Email== email);
            if (User == null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                return returnUser;
            }
            returnUser.Success = true;
            returnUser.Error = null;
            returnUser.Users = new List<Users>()
            {
                User
            };
            return returnUser;
        }

        public async Task<ReturnUserList> AddNewUser(CreateUserRequest userRequest)
        {
            var returnUser = new ReturnUserList();
            ValidationResult result = new CreateUserRequestValidator().Validate(userRequest);
            if (!result.IsValid)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.INVALID_REQUEST;
                return returnUser;
            }

            Users userCheckEmail = _UserRepository.GetFirstOrDefaultAsync(x => x.Email == userRequest.Email).Result;
            if (userCheckEmail != null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.EMAIL_IS_EXITED;
                return returnUser;
            }
            Users userCheckPhone = _UserRepository.GetFirstOrDefaultAsync(x => x.Phone == userRequest.Phone).Result;
            if (userCheckPhone != null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.PHONE_IS_EXITED;
                return returnUser;
            }
            Users userCheckCCCDNumber = _UserRepository.GetFirstOrDefaultAsync(x => x.Cccdnumber == userRequest.Cccdnumber).Result;
            if (userCheckCCCDNumber != null)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.CCCD_NUMBER_IS_EXITED;
                return returnUser;
            }

            if (!userRequest.Email.Contains("@"))
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.WRONG_EMAIL_FORMAT;
                return returnUser;
            }
            if ((!userRequest.Phone.StartsWith("09")
                && !userRequest.Phone.StartsWith("08")
                && !userRequest.Phone.StartsWith("07")
                && !userRequest.Phone.StartsWith("05")
                && !userRequest.Phone.StartsWith("03"))
                || userRequest.Phone.Length != 10)
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.WRONG_PHONE_FORMAT;
                return returnUser;
            }
            if (userRequest.Cccdnumber.Length != 12
                || !userRequest.Cccdnumber.StartsWith("0"))
            {
                returnUser.Success = false;
                returnUser.Error = ErrorMessage.CommonError.WRONG_CCCD_NUMBER_FORMAT;
                return returnUser;
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
            newUser.CreateDate = DateTime.Now;
            newUser.UpdateDate = DateTime.Now;
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

            returnUser.Success = true;
            returnUser.Error = null;
            returnUser.Users = new List<Users>()
            {
                newUser
            };
            return returnUser;
        }

        public async Task<ReturnUserList> UpdateUser(UpdateUserRequest userRequest)
        {
            try
            {
                var returnUser = new ReturnUserList();
                var userUpdate = await _UserRepository.GetFirstOrDefaultAsync(u => u.Id == userRequest.UserId);

                if (userUpdate == null)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                    return returnUser;
                }

                ValidationResult result = new UpdateUserRequestValidator().Validate(userRequest);
                if (!result.IsValid)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.CommonError.INVALID_REQUEST;
                    return returnUser;
                }

                Users userCheckPhone = _UserRepository.GetFirstOrDefaultAsync(x => x.Phone == userRequest.Phone).Result;
                if (userCheckPhone != null)
                {
                    if(userCheckPhone.Id != userUpdate.Id)
                    {
                        returnUser.Success = false;
                        returnUser.Error = ErrorMessage.CommonError.PHONE_IS_EXITED;
                        return returnUser;
                    }
                }

                if ((!userRequest.Phone.StartsWith("09")
                    && !userRequest.Phone.StartsWith("08")
                    && !userRequest.Phone.StartsWith("07")
                    && !userRequest.Phone.StartsWith("05")
                    && !userRequest.Phone.StartsWith("03"))
                    || userRequest.Phone.Length != 10)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.CommonError.WRONG_PHONE_FORMAT;
                    return returnUser;
                }

                userUpdate.Name = userRequest.UserName;
                userUpdate.Password = userRequest.Password;
                userUpdate.Address = userRequest.Address;
                userUpdate.Phone = userRequest.Phone;
                userUpdate.Avatar = userRequest.Avatar;
                //userUpdate.Status = userRequest.Status;
                userUpdate.UpdateDate = DateTime.Now;

                await _UserRepository.UpdateAsync(userUpdate);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa được cập nhập thông tin",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);

                returnUser.Success = true;
                returnUser.Error = null;
                returnUser.Users = new List<Users>()
                {
                    userUpdate
                };
                return returnUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<ReturnUserList> UpdateRoleAccount(Guid id)
        {
            try
            {
                var returnUser = new ReturnUserList();
                var user = await _UserRepository.GetFirstOrDefaultAsync(u => u.Id == id && u.Status == (int)UserStatusEnum.Acctive);

                if (user == null)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                    return returnUser;
                }

                user.Role = (int)RoleEnum.Auctioneer;
                user.UpdateDate = DateTime.Now;

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

                returnUser.Success = true;
                returnUser.Error = null;
                returnUser.Users = new List<Users>()
                {
                    user
                };
                return returnUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReturnUserList> UpdatePassword(UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var returnUser = new ReturnUserList();
                var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == updatePasswordRequest.Id
                    && x.Password == updatePasswordRequest.OldPassword
                    && x.Status == 1);
                if (user == null)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.UserError.USER_NOT_FOUND;
                    return returnUser;
                }

                ValidationResult result = new UpdatePasswordRequestValidator().Validate(updatePasswordRequest);
                if (!result.IsValid)
                {
                    returnUser.Success = false;
                    returnUser.Error = ErrorMessage.CommonError.INVALID_REQUEST;
                    return returnUser;
                }

                user.Password = updatePasswordRequest.NewPassword;
                user.UpdateDate = DateTime.Now;

                await _UserRepository.UpdateAsync(user);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa cập nhập mật khẩu",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);

                returnUser.Success = true;
                returnUser.Error = null;
                returnUser.Users = new List<Users>()
                {
                    user
                };
                return returnUser;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
