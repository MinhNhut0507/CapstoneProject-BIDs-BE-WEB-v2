using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.SendEmailModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;

namespace Business_Logic.Modules.SessionModule
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _SessionRepository;
        private readonly IItemService _ItemService;
        public SessionService(ISessionRepository SessionRepository
            , IItemService ItemService)
        {
            _SessionRepository = SessionRepository;
            _ItemService = ItemService;
        }

        public async Task<ICollection<Session>> GetAll()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule", options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsNotStart()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.NotStart).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsInStage()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.InStage).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsComplete()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.Complete).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsHaventPay()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.HaventTranferYet).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsOutOfDate()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.OutOfDate).ToList());
        }

        public async Task<ICollection<Session>> GetSessionByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Session = await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (Session == null)
            {
                throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
            }
            return Session;
        }

        public async Task<ICollection<Session>> GetSessionByName(string SessionName)
        {
            if (SessionName == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var Session = await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule"
                , options: o => o.Where(x => x.Name == SessionName).ToList());
            if (Session == null)
            {
                throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
            }
            return Session;
        }

        //public async Task<Session> GetSessionByType(string Category)
        //{
        //    if (Category == null)
        //    {
        //        throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
        //    }
        //    var type = _UserService.GetFirstOrDefaultAsync(x => x.CategoryName == Category).Result;
        //    var Session = await _SessionRepository.GetFirstOrDefaultAsync(x => x.CategoryId == type.CategoryId);
        //    if (Session == null)
        //    {
        //        throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
        //    }
        //    return Session;
        //}

        public async Task<Session> AddNewSession(CreateSessionRequest SessionRequest)
        {

            ValidationResult result = new CreateSessionRequestValidator().Validate(SessionRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            if(SessionRequest.EndTime < SessionRequest.BeginTime)
            {
                throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_END_ERROR);
            }

            if (SessionRequest.BeginTime < DateTime.UtcNow)
            {
                throw new Exception(ErrorMessage.SessionError.DATE_TIME_LATE_ERROR);
            }

            if (SessionRequest.BeginTime <  DateTime.UtcNow.AddDays(1))
            {
                throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_ERROR);
            }

            var item = await _ItemService.GetItemByID(SessionRequest.ItemId);

            var newSession = new Session();

            newSession.Id = Guid.NewGuid();
            newSession.ItemId = SessionRequest.ItemId;
            newSession.Name = SessionRequest.SessionName;
            newSession.FeeId = SessionRequest.FeeId;
            newSession.SessionRuleId = SessionRequest.SessionRuleId;
            newSession.BeginTime = SessionRequest.BeginTime;
            newSession.AuctionTime = SessionRequest.EndTime - SessionRequest.BeginTime;
            newSession.EndTime = SessionRequest.EndTime;
            newSession.FinalPrice = item.ElementAt(0).FirstPrice;
            newSession.CreateDate = DateTime.Now;
            newSession.UpdateDate = DateTime.Now;
            newSession.Status = (int)SessionStatusEnum.NotStart;

            await _SessionRepository.AddAsync(newSession);
            return newSession;
        }

        public async Task<Session> UpdateSession(UpdateSessionRequest SessionRequest)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRequest.SessionID 
                && x.Status == (int)SessionStatusEnum.NotStart);

                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionRequestValidator().Validate(SessionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                Session SessionCheck = _SessionRepository.GetFirstOrDefaultAsync(x => x.Name == SessionRequest.SessionName).Result;

                if (SessionCheck != null)
                {
                    if(SessionCheck.Id != SessionRequest.SessionID)
                        throw new Exception(ErrorMessage.SessionError.SESSION_EXISTED);
                }

                if (SessionRequest.EndTime < SessionRequest.BeginTime)
                {
                    throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_END_ERROR);
                }

                if (SessionRequest.BeginTime < DateTime.UtcNow)
                {
                    throw new Exception(ErrorMessage.SessionError.DATE_TIME_LATE_ERROR);
                }

                if (SessionRequest.BeginTime < DateTime.UtcNow.AddDays(1))
                {
                    throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_ERROR);
                }

                SessionUpdate.Name = SessionRequest.SessionName;
                SessionUpdate.FeeId = SessionRequest.FeeId;
                SessionUpdate.SessionRuleId = SessionRequest.SessionRuleId;
                SessionUpdate.BeginTime = SessionRequest.BeginTime;
                SessionUpdate.AuctionTime = SessionRequest.EndTime - SessionRequest.BeginTime;
                SessionUpdate.EndTime = SessionRequest.EndTime;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdatePriceSession(Guid id, double Price)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == id
                && x.Status == (int)SessionStatusEnum.InStage);
                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                SessionUpdate.FinalPrice = Price;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> UpdateSessionStatusNotStart(UpdateSessionStatusRequest SessionRequest)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRequest.SessionID);

                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionStatusRequestValidator().Validate(SessionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionUpdate.Status = (int)SessionStatusEnum.InStage;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> UpdateSessionStatusInStage(UpdateSessionStatusRequest SessionRequest)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRequest.SessionID);

                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionStatusRequestValidator().Validate(SessionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionUpdate.Status = (int)SessionStatusEnum.HaventTranferYet;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> UpdateSessionStatusHaventTranfer(UpdateSessionStatusRequest SessionRequest)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRequest.SessionID);

                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionStatusRequestValidator().Validate(SessionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionUpdate.Status = (int)SessionStatusEnum.OutOfDate;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> UpdateSessionStatusComplete(UpdateSessionStatusRequest SessionRequest)
        {
            try
            {
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionRequest.SessionID);

                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionStatusRequestValidator().Validate(SessionRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionUpdate.Status = (int)SessionStatusEnum.Complete;
                SessionUpdate.UpdateDate = DateTime.Now;

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> DeleteSession(Guid? SessionDeleteID)
        {
            try
            {
                if (SessionDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                Session SessionDelete = _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == SessionDeleteID && x.Status != (int)SessionStatusEnum.Delete).Result;

                if (SessionDelete == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                SessionDelete.Status = (int)SessionStatusEnum.Delete;
                await _SessionRepository.UpdateAsync(SessionDelete);
                return SessionDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
