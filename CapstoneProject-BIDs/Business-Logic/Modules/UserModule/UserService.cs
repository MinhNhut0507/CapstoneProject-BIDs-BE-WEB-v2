﻿using Business_Logic.Modules.CommonModule.Interface;
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
using System.Text;

namespace Business_Logic.Modules.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        public UserService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public async Task<ICollection<Users>> GetAll()
        {
            return await _UserRepository.GetAll(includeProperties: "UserPaymentInformations"
                , options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
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
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Waiting, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Users>> GetUserByID(Guid? id)
        {
            return await _UserRepository.GetAll(includeProperties: "UserPaymentInformations"
                , options: o => o.Where(x => x.Id == id).ToList());
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
            var user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Email == email, includeProperties: "UserPaymentInformations");
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

            Users userCheckName = _UserRepository.GetFirstOrDefaultAsync(x => x.Name == userRequest.UserName).Result;

            if (userCheckName != null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_EXITED);
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

            if (userRequest.UserName.Length > 50)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_OUT_OF_LENGHT);
            }

            if ((!userRequest.Phone.StartsWith("09")
                && !userRequest.Phone.StartsWith("08")
                && !userRequest.Phone.StartsWith("07")
                && !userRequest.Phone.StartsWith("05")
                && !userRequest.Phone.StartsWith("03")
                && !userRequest.Phone.StartsWith("02"))
                || userRequest.Phone.Length != 10)
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
            }

            if (userRequest.Cccdnumber.Length != 12
                || !userRequest.Cccdnumber.StartsWith("0"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_CCCD_NUMBER_FORMAT);
            }

            if (!userRequest.Avatar.Contains(".jpg")
                && !userRequest.Avatar.Contains(".png")
                && !userRequest.Avatar.Contains(".heic"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
            }

            if (!userRequest.CccdfrontImage.Contains(".jpg")
                && !userRequest.CccdfrontImage.Contains(".png")
                && !userRequest.CccdfrontImage.Contains(".heic"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
            }

            if (!userRequest.CccdbackImage.Contains(".jpg")
                && !userRequest.CccdbackImage.Contains(".png")
                && !userRequest.CccdfrontImage.Contains(".heic"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
            }

            if (userRequest.Password.Length > 50)
            {
                throw new Exception(ErrorMessage.CommonError.PASSWORD_OUT_OF_LENGHT);
            }

            var newUser = new Users();

            newUser.Id = Guid.NewGuid();
            newUser.Name = userRequest.UserName;
            newUser.Avatar = userRequest.Avatar;
            newUser.Role = (int)RoleEnum.Guest;
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
            newUser.Status = (int)UserStatusEnum.Waiting;

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
                var userUpdate = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == userRequest.UserId);

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
                    if(userCheckPhone.Id != userRequest.UserId)
                    {
                        throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
                    }
                }
                Users userCheckName = _UserRepository.GetFirstOrDefaultAsync(x => x.Name == userRequest.UserName).Result;
                if (userCheckName != null)
                {
                    if (userCheckName.Id != userRequest.UserId)
                    {
                        throw new Exception(ErrorMessage.CommonError.NAME_IS_EXITED);
                    }
                }

                if ((!userRequest.Phone.StartsWith("09")
                    && !userRequest.Phone.StartsWith("08")
                    && !userRequest.Phone.StartsWith("07")
                    && !userRequest.Phone.StartsWith("05")
                    && !userRequest.Phone.StartsWith("03")
                    && !userRequest.Phone.StartsWith("02"))
                    || userRequest.Phone.Length != 10)
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
                }

                if (!userRequest.Avatar.Contains(".jpg")
                && !userRequest.Avatar.Contains(".png")
                && !userRequest.Avatar.Contains(".heic"))
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_IMAGE_FORMAT);
                }

                if (userRequest.UserName.Length > 50)
                {
                    throw new Exception(ErrorMessage.CommonError.NAME_OUT_OF_LENGHT);
                }

                if (userRequest.Password.Length > 50)
                {
                    throw new Exception(ErrorMessage.CommonError.PASSWORD_OUT_OF_LENGHT);
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

                return userUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Users> UpdateRoleAccount(string email)
        {
            try
            {
                Users user = await _UserRepository.GetFirstOrDefaultAsync(x => x.Email == email && x.Status != (int)UserStatusEnum.Ban);
                if (user == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                user.Role = (int)RoleEnum.User;
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

                if (updatePasswordRequest.NewPassword.Length > 50)
                {
                    throw new Exception(ErrorMessage.CommonError.PASSWORD_OUT_OF_LENGHT);
                }

                user.Password = updatePasswordRequest.NewPassword;
                DateTime dateTime = DateTime.UtcNow;
                user.UpdateDate = dateTime.AddHours(7);

                await _UserRepository.UpdateAsync(user);

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
