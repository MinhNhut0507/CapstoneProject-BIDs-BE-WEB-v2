﻿using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.LoginModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.StaffModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using System.Net;
using System.Net.Mail;

namespace Business_Logic.Modules.StaffModule
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _StaffRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IBanHistoryService _BanHistoryService;
        public StaffService(IStaffRepository StaffRepository
            , IUserRepository UserRepository
            , IBanHistoryService BanHistoryService)
        {
            _StaffRepository = StaffRepository;
            _UserRepository = UserRepository;
            _BanHistoryService = BanHistoryService;
        }

        public async Task<ICollection<Staff>> GetAll()
        {
            return await _StaffRepository.GetAll(options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<Staff>> GetStaffsIsValid()
        {
            return _StaffRepository.GetStaffsBy(x => x.Status == true, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<Staff> GetStaffByID(Guid? id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Staff == null)
            {
                throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
            }
            return Staff;
        }

        public async Task<Staff> GetStaffByName(string StaffName)
        {
            if (StaffName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Name == StaffName);
            if (Staff == null)
            {
                throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
            }
            return Staff;
        }

        public async Task<Staff> GetStaffByEmail(string email)
        {
            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            if (Staff == null)
            {
                throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
            }
            return Staff;
        }

        public async Task<Staff> AddNewStaff(CreateStaffRequest StaffRequest)
        {

            ValidationResult result = new CreateStaffRequestValidator().Validate(StaffRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            Staff staffCheckEmail = _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == StaffRequest.Email).Result;
            if (staffCheckEmail != null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_EXITED);
            }
            Staff staffTestPhone = _StaffRepository.GetFirstOrDefaultAsync(x => x.Phone == StaffRequest.Phone).Result;
            if (staffTestPhone != null)
            {
                throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
            }

            if (!StaffRequest.Email.Contains("@"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_EMAIL_FORMAT);
            }
            if ((!StaffRequest.Phone.StartsWith("09") 
                && !StaffRequest.Phone.StartsWith("08")
                && !StaffRequest.Phone.StartsWith("07")
                && !StaffRequest.Phone.StartsWith("05")
                && !StaffRequest.Phone.StartsWith("03"))
                || StaffRequest.Phone.Length != 10)
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
            }

            var newStaff = new Staff();

            newStaff.Id = Guid.NewGuid();
            newStaff.Name = StaffRequest.StaffName;
            newStaff.Email = StaffRequest.Email;
            newStaff.Password = StaffRequest.Password;
            newStaff.Address = StaffRequest.Address;
            newStaff.Phone = StaffRequest.Phone;
            newStaff.DateOfBirth = StaffRequest.DateOfBirth;
            DateTime dateTime = DateTime.UtcNow;
            newStaff.CreateDate = dateTime.AddHours(7);
            newStaff.UpdateDate = dateTime.AddHours(7);
            newStaff.Status = true;

            await _StaffRepository.AddAsync(newStaff);
            return newStaff;
        }

        public async Task<Staff> UpdateStaff(UpdateStaffRequest StaffRequest)
        {
            try
            {
                var StaffUpdate = GetStaffByID(StaffRequest.StaffId).Result;

                if (StaffUpdate == null)
                {
                    throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
                }

                ValidationResult result = new UpdateStaffRequestValidator().Validate(StaffRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Staff staffCheckPhone = _StaffRepository.GetFirstOrDefaultAsync(x => x.Phone == StaffRequest.Phone).Result;
                if (staffCheckPhone != null)
                {
                    if(staffCheckPhone.Id != StaffUpdate.Id)
                    {
                        throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
                    }
                }

                if ((!StaffRequest.Phone.StartsWith("09")
                    && !StaffRequest.Phone.StartsWith("08")
                    && !StaffRequest.Phone.StartsWith("07")
                    && !StaffRequest.Phone.StartsWith("05")
                    && !StaffRequest.Phone.StartsWith("03"))
                    || StaffRequest.Phone.Length != 10)
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
                }

                StaffUpdate.Name = StaffRequest.StaffName;
                StaffUpdate.Address = StaffRequest.Address;
                StaffUpdate.Phone = StaffRequest.Phone;
                DateTime dateTime = DateTime.UtcNow;
                StaffUpdate.UpdateDate = dateTime.AddHours(7);

                await _StaffRepository.UpdateAsync(StaffUpdate);

                //CreateStaffNotificationDetailRequest request = new CreateStaffNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa được cập nhập thông tin",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    StaffId = StaffUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _staffNotificationDetailService.AddNewStaffNotificationDetail(request);

                return StaffUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Staff> UpdatePassword(UpdatePasswordRequest updatePasswordRequest)
        {
            try
            {
                var staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Id == updatePasswordRequest.Id
                    && x.Password == updatePasswordRequest.OldPassword);
                if (staff == null)
                {
                    throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
                }

                ValidationResult result = new UpdatePasswordRequestValidator().Validate(updatePasswordRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                staff.Password = updatePasswordRequest.NewPassword;
                DateTime dateTime = DateTime.UtcNow;
                staff.UpdateDate = dateTime.AddHours(7);

                await _StaffRepository.UpdateAsync(staff);

                //CreateStaffNotificationDetailRequest request = new CreateStaffNotificationDetailRequest()
                //{
                //    Messages = "Tài khoản vừa được cập nhập mật khẩu",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    StaffId = StaffUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _staffNotificationDetailService.AddNewStaffNotificationDetail(request);

                return staff;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Staff> DeleteStaff(Guid? StaffDeleteID)
        {
            try
            {
                if (StaffDeleteID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Staff StaffDelete = _StaffRepository.GetFirstOrDefaultAsync(x => x.Id == StaffDeleteID && x.Status == true).Result;

                if (StaffDelete == null)
                {
                    throw new Exception(ErrorMessage.StaffError.STAFF_NOT_FOUND);
                }

                StaffDelete.Status = false;
                await _StaffRepository.UpdateAsync(StaffDelete);
                return StaffDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> AcceptCreateAccount(Guid? accountCreateID)
        {
            try
            {
                if (accountCreateID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Users UserCreate = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == accountCreateID && x.Status == 0);

                if (UserCreate == null)
                {
                    throw new Exception(ErrorMessage.UserError.ACCOUNT_CREATE_NOT_FOUND);
                }

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = UserCreate.Email;
                string subject = "[BIDs] - Dịch vụ tài khoản";
                string content = "Tài khoản" + UserCreate.Name + " đã được khởi tạo thành công, chúc bạn có những phút giây sử dụng hệ thống vui vẻ. Cảm ơn bạn đã tin tưởng và sử dụng BIDs - Hệ Thống Đấu Giá Trực Tuyến.";

                UserCreate.Status = (int)UserStatusEnum.Acctive;
                UserCreate.Role = (int)RoleEnum.User;
                await _UserRepository.UpdateAsync(UserCreate);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(UserCreate.Email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return UserCreate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at accept create account type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> DenyCreate(Guid? accountCreateID, string reason)
        {
            try
            {
                if (accountCreateID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Users UserCreate = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == accountCreateID && x.Status == 0);

                if (UserCreate == null)
                {
                    throw new Exception(ErrorMessage.UserError.ACCOUNT_CREATE_NOT_FOUND);
                }

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = UserCreate.Email;
                string subject = "[BIDs] - Dịch vụ tài khoản";
                string content = "Tài khoản" + UserCreate.Name + " khởi tạo không thành công vì thông tin bạn cung cấp không chính xác!"
                    + " Cụ thể lỗi ở việc "
                    + reason
                    + ". Bạn hãy cung cấp đúng thông tin hơn trong lần tiếp theo.";

                UserCreate.Status = (int)UserStatusEnum.Deny;
                await _UserRepository.RemoveAsync(UserCreate);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(UserCreate.Email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return UserCreate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at accept create account type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> BanUser(Guid? UserBanID, string Reason)
        {
            try
            {
                if (UserBanID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                var UserBan = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == UserBanID && x.Status == (int)UserStatusEnum.Acctive);

                if (UserBan == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = UserBan.Email;
                string subject = "[BIDs] - Dịch vụ tài khoản";
                string content = "Tài khoản" + UserBan.Name + "đã bị khóa vì vi phạm điều luật của hệ thống chúng tôi, bạn sẽ không thể sử dụng dịch vụ của hệ thống chúng tôi! ";

                UserBan.Status = (int)UserStatusEnum.Ban;
                await _UserRepository.UpdateAsync(UserBan);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(UserBan.Email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return UserBan;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at ban user type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Users> UnbanUser(Guid? UserUnbanID)
        {
            try
            {
                if (UserUnbanID == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Users UserUnban = await _UserRepository.GetFirstOrDefaultAsync(x => x.Id == UserUnbanID && x.Status == (int)UserStatusEnum.Ban);

                if (UserUnban == null)
                {
                    throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
                }

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = UserUnban.Email;
                string subject = "[BIDs] - Dịch vụ tài khoản";
                string content = "Tài khoản" + UserUnban.Name + "đã được mở khóa, mong bạn sẽ có những trải nghiệm tốt tại hệ thống. ";

                UserUnban.Status = (int)UserStatusEnum.Acctive;
                await _UserRepository.UpdateAsync(UserUnban);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(UserUnban.Email);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return UserUnban;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at ban user type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
