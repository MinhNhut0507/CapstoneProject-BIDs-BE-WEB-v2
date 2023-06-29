using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;
using System.Net.Mail;
using System.Net;
using FluentValidation;
using System;
using System.Text;
using Data_Access.Enum;

namespace Business_Logic.Modules.UserModule
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _UserRepository;
        public UserService(IUserRepository UserRepository)
        {
            _UserRepository = UserRepository;
        }

        public async Task<ICollection<User>> GetAll()
        {
            return await _UserRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<User>> GetUsersIsActive()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Acctive, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<User>> GetUsersIsBan()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Ban, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<User>> GetUsersIsWaitting()
        {
            return await _UserRepository.GetUsersBy(x => x.Status == (int)UserStatusEnum.Waitting, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<User> GetUserByID(Guid? id)
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

        public async Task<User> GetUserByName(string userName)
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

        public async Task<User> GetUserByEmail(string email)
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

        public async Task<User> AddNewUser(CreateUserRequest userRequest)
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

        public async Task<User> UpdateUser(UpdateUserRequest userRequest)
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

                userUpdate.Name = userRequest.UserName;
                userUpdate.Password = userRequest.Password;
                userUpdate.Email = userRequest.Email;
                userUpdate.Address = userRequest.Address;
                userUpdate.Phone = userRequest.Phone;
                userUpdate.Avatar = userRequest.Avatar;
                //userUpdate.Status = userRequest.Status;
                userUpdate.UpdateDate = DateTime.Now;

                await _UserRepository.UpdateAsync(userUpdate);
                return userUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        //public async Task NotiForUser(NotiForUserRequest notiForUser)
        //{
        //    try
        //    {
        //        var notiUser = GetUserByID(notiForUser.UserId).Result;

        //        if (notiUser == null)
        //        {
        //            throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
        //        }

        //        ValidationResult result = new NotiForUserRequestValidator().Validate(notiForUser);
        //        if (!result.IsValid)
        //        {
        //            throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
        //        }


        //        notiUser.Notification = notiForUser.Notification;
        //        await _UserRepository.UpdateAsync(notiUser);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error at delete type: " + ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
