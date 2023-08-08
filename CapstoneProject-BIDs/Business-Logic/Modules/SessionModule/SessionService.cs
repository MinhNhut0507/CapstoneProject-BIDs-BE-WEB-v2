using Business_Logic.Modules.FeeModule.Interface;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionModule.Request;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using System;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.SessionModule.Response;

namespace Business_Logic.Modules.SessionModule
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _SessionRepository;
        private readonly IItemService _ItemService;
        private readonly IFeeService _FeeService;
        private readonly IBookingItemService _BookingItemService;
        public SessionService(ISessionRepository SessionRepository
            , IItemService ItemService
            , IFeeService FeeService
            , IBookingItemService BookingItemService)
        {
            _SessionRepository = SessionRepository;
            _ItemService = ItemService;
            _FeeService = FeeService;
            _BookingItemService = BookingItemService;
        }

        public async Task<ICollection<Session>> GetAll()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions", options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsNotStartAndInStage()
        {
            var list = await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.NotStart || x.Status == (int)SessionStatusEnum.InStage).ToList());
            return list;
        }

        public async Task<ICollection<Session>> GetSessionsIsNotStart()
        {
            var list =  await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.NotStart).ToList());
            return list;
        }

        public async Task<ICollection<Session>> GetSessionsIsInStage()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.InStage).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsComplete()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.Complete).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsHaventPay()
        {
            var list =  await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.HaventTranferYet).ToList());
            
            return list;
        }

        public async Task<ICollection<Session>> GetSessionsIsFail()
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Status == (int)SessionStatusEnum.Fail).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsNotStartByUser(Guid id)
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.User,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)SessionStatusEnum.NotStart).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsInStageByUser(Guid id)
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.User,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)SessionStatusEnum.InStage).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsCompleteByUser(Guid id)
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.User,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)SessionStatusEnum.Complete).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsHaventPayByUser(Guid id)
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.User,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)SessionStatusEnum.HaventTranferYet).ToList());
        }

        public async Task<ICollection<Session>> GetSessionsIsFailByUser(Guid id)
        {
            return await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.User,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)SessionStatusEnum.Fail).ToList());
        }

        public async Task<ICollection<Session>> GetSessionByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var Session = await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
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
            var Session = await _SessionRepository.GetAll(includeProperties: "Fee,Item,SessionRule,Item.Category,Item.Images,Item.ItemDescriptions,Item.Category.Descriptions"
                , options: o => o.Where(x => x.Name == SessionName).ToList());
            if (Session == null)
            {
                throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
            }
            return Session;
        }

        public async Task<Session> AddNewSession(CreateSessionRequest SessionRequest)
        {

            ValidationResult result = new CreateSessionRequestValidator().Validate(SessionRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var checkSession = await _SessionRepository.GetFirstOrDefaultAsync(x => x.ItemId == SessionRequest.ItemId);

            if(checkSession != null)
            {
                throw new Exception(ErrorMessage.SessionError.SESSION_EXISTED);
            }

            var BeginTime = SessionRequest.BeginTime;

            var EndTime = SessionRequest.EndTime;

            if (EndTime < BeginTime)
            {
                throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_END_ERROR);
            }

            if (BeginTime < DateTime.UtcNow)
            {
                throw new Exception(ErrorMessage.SessionError.DATE_TIME_LATE_ERROR);
            }

            //if (BeginTime <  DateTime.UtcNow.AddDays(1))
            //{
            //    throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_ERROR);
            //}

            TimeSpan timeSpan = (EndTime - BeginTime);
            TimeSpan checkTimeMax = new TimeSpan(7, 0, 0, 0);
            TimeSpan checkTimeMin = new TimeSpan(0, 3, 0, 0);

            if (timeSpan > checkTimeMax)
            {
                throw new Exception(ErrorMessage.SessionError.AUCTION_TIME_MAX_ERROR);
            }

            //if (timeSpan < checkTimeMin)
            //{
            //    throw new Exception(ErrorMessage.SessionError.AUCTION_TIME_MIN_ERROR);
            //}

            var item = await _ItemService.GetItemByID(SessionRequest.ItemId);
            var fee = await _FeeService.GetAll();

            var newSession = new Session();

            newSession.Id = Guid.NewGuid();
            newSession.ItemId = SessionRequest.ItemId;
            newSession.Name = SessionRequest.SessionName;
            foreach(var i in fee)
            {
                if(i.Min <= item.ElementAt(0).FirstPrice && i.Max >= item.ElementAt(0).FirstPrice)
                {
                    newSession.FeeId = i.Id;
                    break;
                }
            }
            newSession.SessionRuleId = SessionRequest.SessionRuleId;
            newSession.BeginTime = SessionRequest.BeginTime;
            newSession.EndTime = SessionRequest.EndTime;
            newSession.FinalPrice = item.ElementAt(0).FirstPrice;
            DateTime dateTime = DateTime.UtcNow;
            newSession.CreateDate = dateTime.AddHours(7);
            newSession.UpdateDate = dateTime.AddHours(7);
            newSession.Status = (int)SessionStatusEnum.NotStart;
            
            await _SessionRepository.AddAsync(newSession);

            var bookingItem = await _BookingItemService.GetBookingItemByItem(newSession.ItemId);
            UpdateBookingItemRequest updateBookingItemRequest = new UpdateBookingItemRequest()
            {
                Id = bookingItem.ElementAt(0).Id,
                Status = (int)BookingItemEnum.Accepted
            };
            await _BookingItemService.UpdateStatusBookingItem(updateBookingItemRequest);

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
                    if(SessionCheck.Id != SessionUpdate.Id)
                        throw new Exception(ErrorMessage.SessionError.SESSION_EXISTED);
                }

                var BeginTime = SessionRequest.BeginTime;

                var EndTime = SessionRequest.EndTime;

                if (EndTime < BeginTime)
                {
                    throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_END_ERROR);
                }

                if (BeginTime < DateTime.UtcNow)
                {
                    throw new Exception(ErrorMessage.SessionError.DATE_TIME_LATE_ERROR);
                }

                //if (BeginTime < DateTime.UtcNow.AddDays(1))
                //{
                //    throw new Exception(ErrorMessage.SessionError.DATE_TIME_BEGIN_ERROR);
                //}

                TimeSpan timeSpan = (EndTime - BeginTime);
                TimeSpan checkTimeMax = new TimeSpan(7, 0, 0, 0);
                TimeSpan checkTimeMin = new TimeSpan(0, 3, 0, 0);

                if (timeSpan > checkTimeMax)
                {
                    throw new Exception(ErrorMessage.SessionError.AUCTION_TIME_MAX_ERROR);
                }

                //if (timeSpan < checkTimeMin)
                //{
                //    throw new Exception(ErrorMessage.SessionError.AUCTION_TIME_MIN_ERROR);
                //}

                SessionUpdate.Name = SessionRequest.SessionName;
                SessionUpdate.SessionRuleId = SessionRequest.SessionRuleId;
                SessionUpdate.BeginTime = BeginTime;
                SessionUpdate.EndTime = EndTime;
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

                await _SessionRepository.UpdateAsync(SessionUpdate);

                //CreateUserNotificationDetailRequest request = new CreateUserNotificationDetailRequest()
                //{
                //    Messages = "Buổi đấu giá cho sản phẩm " + item.ElementAt(0).Name + " vừa được tạo thành công và sẽ bắt đầu vào lúc "
                //    + SessionUpdate.BeginTime + " và sẽ diễn ra trong vòng " + SessionUpdate.AuctionTime + ".",
                //    TypeId = (int)NotificationEnum.AccountNoti,
                //    UserId = userUpdate.Id,
                //    NotificationId = Guid.NewGuid()
                //};
                //await _userNotificationDetailService.AddNewUserNotificationDetail(request);


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
                var SessionUpdate = await _SessionRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (SessionUpdate == null)
                {
                    throw new Exception(ErrorMessage.SessionError.SESSION_NOT_FOUND);
                }

                SessionUpdate.FinalPrice = Price;
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

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
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

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
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

                await _SessionRepository.UpdateAsync(SessionUpdate);
                return SessionUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<Session> UpdateSessionStatusFail(UpdateSessionStatusRequest SessionRequest)
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

                SessionUpdate.Status = (int)SessionStatusEnum.Fail;
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

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
                DateTime dateTime = DateTime.UtcNow;
                SessionUpdate.UpdateDate = dateTime.AddHours(7);

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
