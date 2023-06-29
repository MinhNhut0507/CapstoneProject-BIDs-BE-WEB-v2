using Business_Logic.Modules.FeeModule.Interface;
using Business_Logic.Modules.FeeModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.FeeModule
{
    public class FeeService : IFeeService
    {
        private readonly IFeeRepository _FeeRepository;
        public FeeService(IFeeRepository FeeRepository)
        {
            _FeeRepository = FeeRepository;
        }

        public async Task<ICollection<Fee>> GetAll()
        {
            return await _FeeRepository.GetAll();
        }

        public Task<ICollection<Fee>> GetFeesIsValid()
        {
            return _FeeRepository.GetFeesBy(x => x.Status == true);
        }

        public async Task<Fee> GetFeeByID(int id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Fee = await _FeeRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (Fee == null)
            {
                throw new Exception(ErrorMessage.FeeError.FEE_NOT_FOUND);
            }
            return Fee;
        }

        public async Task<Fee> GetFeeByName(string FeeName)
        {
            if (FeeName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Fee = await _FeeRepository.GetFirstOrDefaultAsync(x => x.Name == FeeName);
            if (Fee == null)
            {
                throw new Exception(ErrorMessage.FeeError.FEE_NOT_FOUND);
            }
            return Fee;
        }

        public async Task<Fee> AddNewFee(CreateFeeRequest FeeRequest)
        {

            ValidationResult result = new CreateFeeRequestValidator().Validate(FeeRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            Fee FeeCheck = _FeeRepository.GetFirstOrDefaultAsync(x => x.Name == FeeRequest.Name).Result;

            if (FeeCheck != null)
            {
                throw new Exception(ErrorMessage.FeeError.FEE_EXISTED);
            }

            var newFee = new Fee();

            newFee.Name = FeeRequest.Name;
            newFee.DepositFee = FeeRequest.DepositFee;
            newFee.Surcharge = FeeRequest.Surcharge;
            newFee.ParticipationFee = FeeRequest.ParticipationFee;
            newFee.Min = FeeRequest.Min;
            newFee.Max = FeeRequest.Max;
            newFee.UpdateDate = DateTime.Now;
            newFee.CreateDate = DateTime.Now;
            newFee.Status = true;

            await _FeeRepository.AddAsync(newFee);
            return newFee;
        }

        public async Task<Fee> UpdateFee(UpdateFeeRequest FeeRequest)
        {
            try
            {
                var FeeUpdate = GetFeeByID(FeeRequest.FeeId).Result;

                if (FeeUpdate == null)
                {
                    throw new Exception(ErrorMessage.FeeError.FEE_NOT_FOUND);
                }

                ValidationResult result = new UpdateFeeRequestValidator().Validate(FeeRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Fee FeeCheck = _FeeRepository.GetFirstOrDefaultAsync(x => x.Name == FeeRequest.Name).Result;

                if (FeeCheck != null)
                {
                    throw new Exception(ErrorMessage.FeeError.FEE_EXISTED);
                }

                FeeUpdate.Name = FeeRequest.Name;
                FeeUpdate.DepositFee = FeeRequest.DepositFee;
                FeeUpdate.Surcharge = FeeRequest.Surcharge;
                FeeUpdate.ParticipationFee = FeeRequest.ParticipationFee;
                FeeUpdate.Min = FeeRequest.Min;
                FeeUpdate.Max = FeeRequest.Max;
                FeeUpdate.UpdateDate = DateTime.Now;
                FeeUpdate.Status = FeeRequest.Status;

                await _FeeRepository.UpdateAsync(FeeUpdate);
                return FeeUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<Fee> DeleteFee(int FeeDeleteID)
        {
            try
            {
                if (FeeDeleteID <= 0)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Fee FeeDelete = _FeeRepository.GetFirstOrDefaultAsync(x => x.Id == FeeDeleteID && x.Status == true).Result;

                if (FeeDelete == null)
                {
                    throw new Exception(ErrorMessage.FeeError.FEE_NOT_FOUND);
                }

                FeeDelete.Status = false;
                await _FeeRepository.UpdateAsync(FeeDelete);
                return FeeDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
