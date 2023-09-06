using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Business_Logic.Modules.UserPaymentInformationModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.UserPaymentInformationModule
{
    public class UserPaymentInformationService : IUserPaymentInformationService
    {
        private readonly IUserPaymentInformationRepository _UserPaymentInformationRepository;
        private readonly IUserService _UserService;
        public UserPaymentInformationService(IUserPaymentInformationRepository UserPaymentInformationRepository
            ,IUserService UserService)
        {
            _UserPaymentInformationRepository = UserPaymentInformationRepository;
            _UserService = UserService;
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
            if (id == Guid.Empty)
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

        public async Task<UserPaymentInformation> GetUserPaymentInformationByUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserPaymentInformation = await _UserPaymentInformationRepository.GetFirstOrDefaultAsync(s => s.UserId == id);
            if (UserPaymentInformation == null)
            {
                throw new Exception(ErrorMessage.UserPaymentInformationError.USER_PAYMENT_INFORMATION_NOT_FOUND);
            }
            return UserPaymentInformation;
        }

        public async Task<bool> CheckUserPaymentInformationByUser(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var UserPaymentInformation = await _UserPaymentInformationRepository.GetFirstOrDefaultAsync(s => s.UserId == id);
            if (UserPaymentInformation == null)
            {
                return false;
            }
            return true;
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

            var user = await _UserService.GetUserByID(UserPaymentInformationRequest.UserId);

            await _UserService.UpdateRoleAccount(user.Email);

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
