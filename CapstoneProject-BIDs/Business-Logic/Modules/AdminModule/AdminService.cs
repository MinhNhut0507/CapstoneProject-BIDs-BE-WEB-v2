using Business_Logic.Modules.AdminModule.Interface;
using Business_Logic.Modules.AdminModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;
using System.Net.Mail;
using System.Net;
using FluentValidation;
using System;
using System.Text;
using Data_Access.Enum;

namespace Business_Logic.Modules.AdminModule
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _AdminRepository;
        public AdminService(IAdminRepository AdminRepository)
        {
            _AdminRepository = AdminRepository;
        }

        public async Task<ICollection<Admin>> GetAll()
        {
            return await _AdminRepository.GetAll();
        }

        public async Task<Admin> GetAdminByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Admin = await _AdminRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Admin == null)
            {
                throw new Exception(ErrorMessage.AdminError.ADMIN_NOT_FOUND);
            }
            return Admin;
        }

        public async Task<Admin> GetAdminByName(string AdminName)
        {
            if (AdminName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Admin = await _AdminRepository.GetFirstOrDefaultAsync(x => x.Name == AdminName);
            if (Admin == null)
            {
                throw new Exception(ErrorMessage.AdminError.ADMIN_NOT_FOUND);
            }
            return Admin;
        }

        public async Task<Admin> GetAdminByEmail(string email)
        {
            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Admin = await _AdminRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            if (Admin == null)
            {
                throw new Exception(ErrorMessage.AdminError.ADMIN_NOT_FOUND);
            }
            return Admin;
        }

        public async Task<Admin> AddNewAdmin(CreateAdminRequest AdminRequest)
        {

            ValidationResult result = new CreateAdminRequestValidator().Validate(AdminRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            Admin AdminCheckEmail = _AdminRepository.GetFirstOrDefaultAsync(x => x.Email == AdminRequest.Email).Result;
            if (AdminCheckEmail != null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_EXITED);
            }
            Admin AdminCheckPhone = _AdminRepository.GetFirstOrDefaultAsync(x => x.Phone == AdminRequest.Phone).Result;
            if (AdminCheckPhone != null)
            {
                throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
            }

            if (!AdminRequest.Email.Contains("@"))
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_EMAIL_FORMAT);
            }
            if ((!AdminRequest.Phone.StartsWith("09") 
                && !AdminRequest.Phone.StartsWith("08")
                && !AdminRequest.Phone.StartsWith("07")
                && !AdminRequest.Phone.StartsWith("05")
                && !AdminRequest.Phone.StartsWith("03"))
                || AdminRequest.Phone.Length != 10)
            {
                throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
            }

            var newAdmin = new Admin();

            newAdmin.Id = Guid.NewGuid();
            newAdmin.Name = AdminRequest.AdminName;
            newAdmin.Email = AdminRequest.Email;
            newAdmin.Password = AdminRequest.Password;
            newAdmin.Address = AdminRequest.Address;
            newAdmin.Phone = AdminRequest.Phone;
            newAdmin.Status = true;

            await _AdminRepository.AddAsync(newAdmin);
            return newAdmin;
        }

        public async Task<Admin> UpdateAdmin(UpdateAdminRequest AdminRequest)
        {
            try
            {
                var AdminUpdate = GetAdminByID(AdminRequest.AdminId).Result;

                if (AdminUpdate == null)
                {
                    throw new Exception(ErrorMessage.AdminError.ADMIN_NOT_FOUND);
                }

                ValidationResult result = new UpdateAdminRequestValidator().Validate(AdminRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Admin AdminCheckPhone = _AdminRepository.GetFirstOrDefaultAsync(x => x.Phone == AdminRequest.Phone).Result;
                if (AdminCheckPhone != null)
                {
                    throw new Exception(ErrorMessage.CommonError.PHONE_IS_EXITED);
                }

                if ((!AdminRequest.Phone.StartsWith("09")
                    && !AdminRequest.Phone.StartsWith("08")
                    && !AdminRequest.Phone.StartsWith("07")
                    && !AdminRequest.Phone.StartsWith("05")
                    && !AdminRequest.Phone.StartsWith("03"))
                    || AdminRequest.Phone.Length != 10)
                {
                    throw new Exception(ErrorMessage.CommonError.WRONG_PHONE_FORMAT);
                }

                AdminUpdate.Name = AdminRequest.AdminName;
                AdminUpdate.Password = AdminRequest.Password;
                AdminUpdate.Address = AdminRequest.Address;
                AdminUpdate.Phone = AdminRequest.Phone;

                await _AdminRepository.UpdateAsync(AdminUpdate);
                return AdminUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}
