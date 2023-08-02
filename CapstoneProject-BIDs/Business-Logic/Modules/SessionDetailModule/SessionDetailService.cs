using Business_Logic.Modules.CategoryModule;
using Business_Logic.Modules.CategoryModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation;
using FluentValidation.Results;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.SessionRuleModule.Interface;
using System.Net.Mail;
using System.Net;

namespace Business_Logic.Modules.SessionDetailModule
{
    public class SessionDetailService : ISessionDetailService
    {
        private readonly ISessionDetailRepository _SessionDetailRepository;
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ISessionService _SessionService;
        private readonly ISessionRuleService _SessionRuleService;
        private readonly IItemService _ItemService;
        private readonly IUserService _UserService;

        public SessionDetailService(ISessionDetailRepository SessionDetailRepository
            , ICategoryRepository CategoryRepository
            , ISessionService SessionService
            , IItemService ItemService
            , ISessionRuleService SessionRuleService
            , IUserService UserService)
        {
            _SessionDetailRepository = SessionDetailRepository;
            _CategoryRepository = CategoryRepository;
            _SessionService = SessionService;
            _ItemService = ItemService;
            _SessionRuleService = SessionRuleService;
            _UserService = UserService;
        }

        public async Task<ICollection<SessionDetail>> GetAll()
        {
            return await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: o => o.OrderByDescending(x => x.CreateDate).ToList());
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailIsActive()
        {
            return await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.Where(o => o.Status == true).ToList());
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailsIsInActive()
        {
            return await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.Where(o => o.Status == false).ToList());
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailByID(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var SessionDetail = await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.Where(o => o.Id == id).ToList());
            if (SessionDetail == null)
            {
                throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
            }
            return SessionDetail;
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailByUser(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var SessionDetail = await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.Where(o => o.UserId == id).ToList());
            if (SessionDetail == null)
            {
                throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
            }
            return SessionDetail;
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailBySession(Guid? id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.NAME_IS_NULL);
            }
            var SessionDetail = await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.Where(o => o.SessionId == id).ToList());
            if (SessionDetail == null)
            {
                throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
            }
            return SessionDetail;
        }

        public async Task<ICollection<SessionDetail>> GetSessionDetailBySessionForBidder(Guid? sessionId, Guid? userId)
        {
            if (sessionId == null || userId == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var SessionDetail = await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule",
                options: o => o.OrderBy(x => x.SessionId == sessionId && x.UserId == userId).ToList());
            if (SessionDetail == null)
            {
                throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
            }
            return SessionDetail;
        }

        public async Task<SessionDetail> Getwinner(Guid id)
        {
            var SessionDetail = await _SessionDetailRepository.GetAll(options: o => o.OrderBy(x => x.SessionId == id).ToList());
            var result = SessionDetail.OrderByDescending(x => x.CreateDate).ToList();
            if (SessionDetail == null)
            {
                throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
            }
            return SessionDetail.ElementAt(0);
        }

        public async Task<SessionDetail> IncreasePrice(CreateSessionDetailRequest SessionDetailRequest)
        {

            ValidationResult result = new CreateSessionDetailRequestValidator().Validate(SessionDetailRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var Session = await _SessionService.GetSessionByID(SessionDetailRequest.SessionId);
            //if (Session.ElementAt(0).Status != (int)SessionStatusEnum.NotStart)
            //{
            //    throw new Exception(ErrorMessage.SessionError.OUT_OF_DATE_BEGIN_ERROR);
            //}
            var SessionDetail = await _SessionDetailRepository.GetAll(includeProperties: "User,Session,Session.Item,Session.SessionRule"
                , options: x => x.OrderByDescending(o => o.UserId == SessionDetailRequest.UserId && o.SessionId == SessionDetailRequest.SessionId).ToList());
            var newSessionDetail = new SessionDetail();
            DateTime dateTime = DateTime.UtcNow;
            if (SessionDetail.Count == 0) 
            {
                var Item = await _ItemService.GetItemByID(Session.ElementAt(0).ItemId);

                newSessionDetail.Id = Guid.NewGuid();
                newSessionDetail.UserId = SessionDetailRequest.UserId;
                newSessionDetail.SessionId = SessionDetailRequest.SessionId;
                newSessionDetail.Price = Item.ElementAt(0).FirstPrice;
                newSessionDetail.CreateDate = dateTime.AddHours(7);
                newSessionDetail.Status = true;

                await _SessionDetailRepository.AddAsync(newSessionDetail);
                return newSessionDetail;
            }
            var SessionRule = await _SessionRuleService.GetSessionRuleByID(Session.ElementAt(0).SessionRuleId);
            var item = await _ItemService.GetItemByID(Session.ElementAt(0).ItemId);
            var ListSessionDetailSort = SessionDetail.OrderByDescending(o => o.CreateDate);

            if (Session.ElementAt(0).EndTime < DateTime.Now)
            {
                throw new Exception(ErrorMessage.SessionError.END_TIME_AUCTION);
            }

            if (((DateTime.Now - ListSessionDetailSort.ElementAt(0).CreateDate) < SessionRule.DelayTime)
                || (((Session.ElementAt(0).EndTime - DateTime.Now) > SessionRule.FreeTime)
                    && (DateTime.Now - ListSessionDetailSort.ElementAt(0).CreateDate) < SessionRule.DelayFreeTime))
            {
                throw new Exception(ErrorMessage.SessionError.TIME_ERROR);
            }

            newSessionDetail.Id = Guid.NewGuid();
            newSessionDetail.UserId = SessionDetailRequest.UserId;
            newSessionDetail.SessionId = SessionDetailRequest.SessionId;
            newSessionDetail.Price = Session.ElementAt(0).FinalPrice + item.ElementAt(0).StepPrice;
            newSessionDetail.CreateDate = dateTime.AddHours(7);
            newSessionDetail.Status = true;

            await _SessionDetailRepository.AddAsync(newSessionDetail);

            await _SessionService.UpdatePriceSession(newSessionDetail.SessionId, newSessionDetail.Price);

            return newSessionDetail;
        }

        public async Task<SessionDetail> Joinning(CreateSessionDetailRequest jonningRequest)
        {

            ValidationResult result = new CreateSessionDetailRequestValidator().Validate(jonningRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            var Session = await _SessionService.GetSessionByID(jonningRequest.SessionId);

            if (Session.ElementAt(0).Status != (int)SessionStatusEnum.NotStart)
            {
                throw new Exception(ErrorMessage.SessionError.OUT_OF_DATE_BEGIN_ERROR);
            }

            var checkDetail = await _SessionDetailRepository.GetFirstOrDefaultAsync(s => s.UserId == jonningRequest.UserId);

            if(checkDetail != null) 
            {
                throw new Exception(ErrorMessage.SessionError.JOIN_ERROR);
            }

            var Item = await _ItemService.GetItemByID(Session.ElementAt(0).ItemId);

            var newSessionDetail = new SessionDetail();

            newSessionDetail.Id = Guid.NewGuid();
            newSessionDetail.UserId = jonningRequest.UserId;
            newSessionDetail.SessionId = jonningRequest.SessionId;
            newSessionDetail.Price = Item.ElementAt(0).FirstPrice;
            DateTime dateTime = DateTime.UtcNow;
            newSessionDetail.CreateDate = dateTime.AddHours(7);
            newSessionDetail.Status = true;

            await _SessionDetailRepository.AddAsync(newSessionDetail);

            var user = await _UserService.GetUserByID(newSessionDetail.UserId);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = user.Email;
            string subject = "BIDs - Đấu Giá";

            string content = "Tài khoản " 
                + user.Email 
                + " đã đăng ký tham gia thành công buổi đấu giá của sản phẩm "
                + Item.ElementAt(0).Name
                + " diễn ra vào ngày "
                + Session.ElementAt(0).BeginTime + " đến ngày "
                + Session.ElementAt(0).EndTime
                + ". Vui lòng đợi tới khi cuộc đấu giá bắt đầu.";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(_gmail);
            mail.To.Add(user.Email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;

            mail.Priority = MailPriority.High;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            return newSessionDetail;
        }

        public async Task<SessionDetail> UpdateSessionDetail(UpdateSessionDetailRequest SessionDetailRequest)
        {
            try
            {
                var SessionDetailUpdate = await _SessionDetailRepository.GetFirstOrDefaultAsync(x => x.Id == SessionDetailRequest.SessionDetailId);

                if (SessionDetailUpdate == null)
                {
                    throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
                }

                ValidationResult result = new UpdateSessionDetailRequestValidator().Validate(SessionDetailRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                SessionDetailUpdate.Status = SessionDetailRequest.Status;

                await _SessionDetailRepository.UpdateAsync(SessionDetailUpdate);
                return SessionDetailUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<SessionDetail> DeleteSessionDetail(Guid? SessionDetailDeleteID)
        {
            try
            {
                if (SessionDetailDeleteID == null)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }

                SessionDetail SessionDetailDelete = _SessionDetailRepository.GetFirstOrDefaultAsync(x => x.Id == SessionDetailDeleteID && x.Status == true).Result;

                if (SessionDetailDelete == null)
                {
                    throw new Exception(ErrorMessage.AuctionHistoryError.AUCTION_HISTORY_NOT_FOUND);
                }

                SessionDetailDelete.Status = false;
                await _SessionDetailRepository.UpdateAsync(SessionDetailDelete);
                return SessionDetailDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at delete type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

    }
}
