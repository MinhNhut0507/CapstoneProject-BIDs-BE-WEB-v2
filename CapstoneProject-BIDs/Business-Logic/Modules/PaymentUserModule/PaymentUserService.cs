using Business_Logic.Modules.PaymentUserModule.Interface;
using Business_Logic.Modules.PaymentUserModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.PaymentUserModule
{
    public class PaymentUserService : IPaymentUserService
    {
        private readonly IPaymentUserRepository _PaymentUserRepository;
        private readonly IStaffRepository _StaffRepository;
        public PaymentUserService(IPaymentUserRepository PaymentUserRepository
            ,IStaffRepository staffRepository)
        {
            _PaymentUserRepository = PaymentUserRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<PaymentUser>> GetAll()
        {
            var result = await _PaymentUserRepository.GetAll(includeProperties: "Session,Session.SessionDetails,User");
            return result;
        }

        public async Task<ICollection<PaymentUser>> GetPaymentUserByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var PaymentUser = await _PaymentUserRepository.GetAll(includeProperties: "Session,Session.SessionDetails,User",
                options: o => o.Where(x => x.Id == id).ToList());
            if (PaymentUser == null)
            {
                throw new Exception(ErrorMessage.PaymentUserError.PAYMENT_USER_NOT_FOUND);
            }
            return PaymentUser;
        }

        public async Task<ICollection<PaymentUser>> GetPaymentUserBySession(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var PaymentUser = await _PaymentUserRepository.GetAll(includeProperties: "Session,Session.SessionDetails,User",
                options: o => o.Where(x => x.SessionId == id).ToList());
            if (PaymentUser == null)
            {
                throw new Exception(ErrorMessage.PaymentUserError.PAYMENT_USER_NOT_FOUND);
            }
            return PaymentUser;
        }

        public async Task<PaymentUser> AddNewPaymentUser(CreatePaymentUserRequest PaymentUserRequest)
        {

            ValidationResult result = new CreatePaymentUserRequestValidator().Validate(PaymentUserRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newPaymentUser = new PaymentUser();

            newPaymentUser.Id = Guid.NewGuid();
            newPaymentUser.UserId = PaymentUserRequest.UserId;
            newPaymentUser.SessionId = PaymentUserRequest.SessionId;
            newPaymentUser.Amount = PaymentUserRequest.Amount;
            newPaymentUser.PaymentDate = PaymentUserRequest.PaymentDate;
            newPaymentUser.PaymentDetail = PaymentUserRequest.PaymentDetail;
            newPaymentUser.PayPalTransactionId = PaymentUserRequest.PayPalTransactionId;
            newPaymentUser.Status = PaymentUserRequest.Status;

            await _PaymentUserRepository.AddAsync(newPaymentUser);
            return newPaymentUser;
        }

        public async Task<PaymentUser> UpdatePaymentUser(UpdatePaymentUserStatusRequest PaymentUserRequest)
        {
            try
            {
                var PaymentUserUpdate = await _PaymentUserRepository.GetFirstOrDefaultAsync(x => x.PayPalTransactionId == PaymentUserRequest.TransactionId);

                if (PaymentUserUpdate == null)
                {
                    throw new Exception(ErrorMessage.PaymentUserError.PAYMENT_USER_NOT_FOUND);
                }

                ValidationResult result = new UpdatePaymentUserStatusRequestValidator().Validate(PaymentUserRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                PaymentUserUpdate.Status = PaymentUserRequest.Status;

                await _PaymentUserRepository.UpdateAsync(PaymentUserUpdate);
                return PaymentUserUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
