using Business_Logic.Modules.PaymentStaffModule.Interface;
using Business_Logic.Modules.PaymentStaffModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace Business_Logic.Modules.PaymentStaffModule
{
    public class PaymentStaffService : IPaymentStaffService
    {
        private readonly IPaymentStaffRepository _PaymentStaffRepository;
        private readonly IStaffRepository _StaffRepository;
        public PaymentStaffService(IPaymentStaffRepository PaymentStaffRepository
            ,IStaffRepository staffRepository)
        {
            _PaymentStaffRepository = PaymentStaffRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<PaymentStaff>> GetAll()
        {
            var result = await _PaymentStaffRepository.GetAll(includeProperties: "Session,Session.SessionDetails,PaymentInformation,Staff");
            return result;
        }

        public async Task<ICollection<PaymentStaff>> GetPaymentStaffByID(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var PaymentStaff = await _PaymentStaffRepository.GetAll(includeProperties: "Session,Session.SessionDetails,PaymentInformation,Staff",
                options: o => o.Where(x => x.Id == id).ToList());
            if (PaymentStaff == null)
            {
                throw new Exception(ErrorMessage.PaymentStaffError.PAYMENT_STAFF_NOT_FOUND);
            }
            return PaymentStaff;
        }

        public async Task<ICollection<PaymentStaff>> GetPaymentStaffBySession(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var PaymentStaff = await _PaymentStaffRepository.GetAll(includeProperties: "Session,Session.SessionDetails,PaymentInformation,Staff",
                options: o => o.Where(x => x.SessionId == id).ToList());
            if (PaymentStaff == null)
            {
                throw new Exception(ErrorMessage.PaymentStaffError.PAYMENT_STAFF_NOT_FOUND);
            }
            return PaymentStaff;
        }

        public async Task<PaymentStaff> AddNewPaymentStaff(CreatePaymentStaffRequest PaymentStaffRequest)
        {

            ValidationResult result = new CreatePaymentStaffRequestValidator().Validate(PaymentStaffRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newPaymentStaff = new PaymentStaff();

            newPaymentStaff.Id = Guid.NewGuid();
            newPaymentStaff.StaffId = PaymentStaffRequest.StaffId;
            newPaymentStaff.UserPaymentInformationId = PaymentStaffRequest.UserPaymentInformationId;
            newPaymentStaff.SessionId = PaymentStaffRequest.SessionId;
            newPaymentStaff.Amount = PaymentStaffRequest.Amount;
            newPaymentStaff.PaymentDate = PaymentStaffRequest.PaymentDate;
            newPaymentStaff.PaymentDetail = PaymentStaffRequest.PaymentDetail;
            newPaymentStaff.PayPalTransactionId = PaymentStaffRequest.PayPalTransactionId;
            newPaymentStaff.Status = PaymentStaffRequest.Status;

            await _PaymentStaffRepository.AddAsync(newPaymentStaff);
            return newPaymentStaff;
        }

        public async Task<PaymentStaff> AddNewReturnDeposit(CreateReturnDepositRequest PaymentStaffRequest)
        {

            ValidationResult result = new CreateReturnDepositRequestValidator().Validate(PaymentStaffRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newPaymentStaff = new PaymentStaff();

            newPaymentStaff.Id = Guid.NewGuid();
            newPaymentStaff.StaffId = PaymentStaffRequest.StaffId;
            newPaymentStaff.PayPalRecieveAccount = PaymentStaffRequest.PayPalRecieveAccount;
            newPaymentStaff.SessionId = PaymentStaffRequest.SessionId;
            newPaymentStaff.Amount = PaymentStaffRequest.Amount;
            newPaymentStaff.PaymentDate = PaymentStaffRequest.PaymentDate;
            newPaymentStaff.PaymentDetail = PaymentStaffRequest.PaymentDetail;
            newPaymentStaff.PayPalTransactionId = PaymentStaffRequest.PayPalTransactionId;
            newPaymentStaff.Status = PaymentStaffRequest.Status;

            await _PaymentStaffRepository.AddAsync(newPaymentStaff);
            return newPaymentStaff;
        }

        public async Task<PaymentStaff> UpdatePaymentStaff(UpdatePaymentStaffStatusRequest PaymentStaffRequest)
        {
            try
            {
                var PaymentStaffUpdate = await _PaymentStaffRepository.GetFirstOrDefaultAsync(x => x.PayPalTransactionId == PaymentStaffRequest.TransactionId);

                if (PaymentStaffUpdate == null)
                {
                    throw new Exception(ErrorMessage.PaymentStaffError.PAYMENT_STAFF_NOT_FOUND);
                }

                ValidationResult result = new UpdatePaymentStaffStatusRequestValidator().Validate(PaymentStaffRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }


                PaymentStaffUpdate.Status = PaymentStaffRequest.Status;

                await _PaymentStaffRepository.UpdateAsync(PaymentStaffUpdate);
                return PaymentStaffUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }


    }
}
