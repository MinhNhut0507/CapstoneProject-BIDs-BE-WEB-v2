using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.UserPaymentInformationModule
{
    public class UserPaymentInformationService : IUserPaymentInformationService
    {
        private readonly IUserPaymentInformationRepository _UserPaymentInformationRepository;
        private readonly IStaffRepository _StaffRepository;
        public UserPaymentInformationService(IUserPaymentInformationRepository UserPaymentInformationRepository
            ,IStaffRepository staffRepository)
        {
            _UserPaymentInformationRepository = UserPaymentInformationRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<UserPaymentInformation>> GetAll()
        {
            var result = await _UserPaymentInformationRepository.GetAll(includeProperties: "PaymentStaffs,User");
            return result;
        }

        public Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationsIsValid()
        {
            return _UserPaymentInformationRepository.GetAll(options: o => o.Where(x => x.Status == true).ToList());
        }

        public async Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserPaymentInformation = await _UserPaymentInformationRepository.GetAll(includeProperties: "PaymentStaffs,User",
                options: o => o.Where(x => x.Id == id).ToList());
            if (UserPaymentInformation == null)
            {
                throw new Exception(ErrorMessage.UserPaymentInformationError.USER_PAYMENT_INFORMATION_NOT_FOUND);
            }
            return UserPaymentInformation;
        }

        public async Task<ICollection<UserPaymentInformation>> GetUserPaymentInformationByUser(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserPaymentInformation = await _UserPaymentInformationRepository.GetAll(includeProperties: "PaymentStaffs,User",
                options: o => o.Where(x => x.UserId == id).ToList());
            if (UserPaymentInformation == null)
            {
                throw new Exception(ErrorMessage.UserPaymentInformationError.USER_PAYMENT_INFORMATION_NOT_FOUND);
            }
            return UserPaymentInformation;
        }

        public async Task<UserPaymentInformation> AddNewUserPaymentInformation(CreateUserPaymentInformationRequest UserPaymentInformationRequest)
        {

            ValidationResult result = new CreateUserPaymentInformationRequestValidator().Validate(UserPaymentInformationRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newUserPaymentInformation = new UserPaymentInformation();

            newUserPaymentInformation.Id = Guid.NewGuid();
            newUserPaymentInformation.UserId = UserPaymentInformationRequest.UserId;
            newUserPaymentInformation.PayPalAccount = UserPaymentInformationRequest.PayPalAccount;
            newUserPaymentInformation.Status = true;

            await _UserPaymentInformationRepository.AddAsync(newUserPaymentInformation);
            return newUserPaymentInformation;
        }

        public async Task<UserPaymentInformation> UpdateUserPaymentInformation(UpdateUserPaymentInformationRequest UserPaymentInformationRequest)
        {
            try
            {
                var UserPaymentInformationUpdate = _UserPaymentInformationRepository.GetFirstOrDefaultAsync(x => x.Id == UserPaymentInformationRequest.Id).Result;

                if (UserPaymentInformationUpdate == null)
                {
                    throw new Exception(ErrorMessage.UserPaymentInformationError.USER_PAYMENT_INFORMATION_NOT_FOUND);
                }

                ValidationResult result = new UpdateUserPaymentInformationRequestValidator().Validate(UserPaymentInformationRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                UserPaymentInformationUpdate.PayPalAccount = UserPaymentInformationRequest.PayPalAccount;
                UserPaymentInformationUpdate.Status = UserPaymentInformationRequest.Status;

                await _UserPaymentInformationRepository.UpdateAsync(UserPaymentInformationUpdate);
                return UserPaymentInformationUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
