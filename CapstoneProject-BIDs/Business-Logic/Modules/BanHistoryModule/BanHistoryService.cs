using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using FluentValidation.Results;

namespace Business_Logic.Modules.BanHistoryModule
{
    public class BanHistoryService : IBanHistoryService
    {
        private readonly IBanHistoryRepository _BanHistoryRepository;
        private readonly IUserService _UserService;
        private readonly IStaffService _StaffService;
        public BanHistoryService(IBanHistoryRepository BanHistoryRepository, IUserService UserService, IStaffService StaffService)
        {
            _BanHistoryRepository = BanHistoryRepository;
            _UserService = UserService;
            _StaffService = StaffService;
        }

        public async Task<ICollection<BanHistory>> GetAll()
        {
            return await _BanHistoryRepository.GetAll(includeProperties: "User" ,options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public Task<ICollection<BanHistory>> GetBanHistorysIsValid()
        {
            return _BanHistoryRepository.GetBanHistorysBy(x => x.Status == true, options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<BanHistory> GetBanHistoryByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BanHistory = await _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            if (BanHistory == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return BanHistory;
        }

        public async Task<ICollection<BanHistory>> GetBanHistoryByUserID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BanHistory = await _BanHistoryRepository.GetBanHistorysBy(x => x.UserId == id);
            if (BanHistory == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return BanHistory;
        }

        public async Task<ICollection<BanHistory>> GetBanHistoryByUserName(string userName)
        {
            if (userName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            User user = await _UserService.GetUserByName(userName);
            var BanHistory = await _BanHistoryRepository.GetBanHistorysBy(x => x.UserId == user.Id);
            if (BanHistory == null)
            {
                throw new Exception(ErrorMessage.UserError.USER_NOT_FOUND);
            }
            return BanHistory;
        }

        public async Task<BanHistory> AddNewBanHistory(CreateBanHistoryRequest BanHistoryRequest)
        {

            ValidationResult result = new CreateBanHistoryRequestValidator().Validate(BanHistoryRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var newBanHistory = new BanHistory();

            newBanHistory.Id = Guid.NewGuid();
            newBanHistory.UserId = BanHistoryRequest.UserId;
            newBanHistory.Reason = BanHistoryRequest.Reason;
            newBanHistory.CreateDate = DateTime.Now;
            newBanHistory.UpdateDate = DateTime.Now;
            newBanHistory.Status = true;

            await _BanHistoryRepository.AddAsync(newBanHistory);
            await _StaffService.BanUser(newBanHistory.Id);
            return newBanHistory;
        }

        public async Task<BanHistory> UpdateBanHistory(UpdateBanHistoryRequest BanHistoryRequest)
        {
            try
            {
                var BanHistoryUpdate = _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.Id == BanHistoryRequest.BanHistoryId).Result;

                if (BanHistoryUpdate == null)
                {
                    throw new Exception(ErrorMessage.BanHistoryError.BAN_HISTORY_NOT_FOUND);
                }

                ValidationResult result = new UpdateBanHistoryRequestValidator().Validate(BanHistoryRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                BanHistoryUpdate.Reason = BanHistoryRequest.Reason;
                BanHistoryUpdate.UpdateDate = DateTime.Now;
                BanHistoryUpdate.Status = BanHistoryRequest.Status;

                await _BanHistoryRepository.UpdateAsync(BanHistoryUpdate);
                return BanHistoryUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        //public async Task DeleteBanHistory(Guid? BanHistoryDeleteID)
        //{
        //    try
        //    {
        //        if (BanHistoryDeleteID == null)
        //        {
        //            throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
        //        }

        //        BanHistory BanHistoryDelete = _BanHistoryRepository.GetFirstOrDefaultAsync(x => x.BanId == BanHistoryDeleteID && x.Status == true).Result;

        //        if (BanHistoryDelete == null)
        //        {
        //            throw new Exception(ErrorMessage.BanHistoryError.BAN_HISTORY_NOT_FOUND);
        //        }

        //        BanHistoryDelete.Status = false;

        //        await _StaffService.UnbanUser(_UserService.);

        //        await _BanHistoryRepository.UpdateAsync(BanHistoryDelete);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error at delete type: " + ex.Message);
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
