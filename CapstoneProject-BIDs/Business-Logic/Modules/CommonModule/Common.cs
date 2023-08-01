using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.UserModule.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Entities;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.CommonModule.Interface;
using Data_Access.Constant;
using Data_Access.Enum;
using Business_Logic.Modules.NotificationModule.Interface;
using Business_Logic.Modules.NotificationTypeModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.NotificationModule;
using Business_Logic.Modules.NotificationModule.Request;
using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Business_Logic.Modules.StaffNotificationDetailModule.Request;
using Business_Logic.Modules.CommonModule.Response;

namespace Business_Logic.Modules.CommonModule
{
    public class Common : ICommon
    {
        private readonly IUserService _UserService;
        private readonly IItemService _ItemService;
        private readonly IBanHistoryService _BanHistoryService;
        private readonly ISessionDetailService _SessionDetailService;
        private readonly ISessionService _SessionService;
        private readonly INotificationService _NotificationService;
        private readonly INotificationTypeService _NotificationTypeService;
        private readonly IStaffNotificationDetailService _StaffNotificationDetailService;
        private readonly IUserNotificationDetailService _UserNotificationDetailService;
        public Common(IUserService UserService
            , IItemService ItemService
            , IBanHistoryService BanHistoryService
            , ISessionDetailService SessionDetailService
            , ISessionService SessionService
            , INotificationService NotificationService
            , INotificationTypeService NotificationTypeService
            , IStaffNotificationDetailService StaffNotificationDetailService
            , IUserNotificationDetailService UserNotificationDetailService)
        {
            _UserService = UserService;
            _ItemService = ItemService;
            _SessionDetailService = SessionDetailService;
            _SessionService = SessionService;
            _BanHistoryService = BanHistoryService;
            _NotificationService = NotificationService;
            _NotificationTypeService = NotificationTypeService;
            _StaffNotificationDetailService = StaffNotificationDetailService;
            _UserNotificationDetailService = UserNotificationDetailService;
        }

        public async Task SendEmailBeginAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var SessionDetail = await _SessionDetailService.GetSessionDetailBySession(session.Id);


            for (int i = 0; i < SessionDetail.Count; i++)
            {
                var user = await _UserService.GetUserByID(SessionDetail.ElementAt(i).UserId);

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = user.Email;
                string subject = "BIDs - Đấu Giá";

                string content = "Cuộc đấu giá của sản phẩm "
                    + item.ElementAt(0).Name
                    + " đã bắt đầu và sẽ diễn ra trong thời gian từ ngày"
                    + session.BeginTime+ " đến ngày "
                    + session.EndTime + " theo giờ Việt Nam"
                    + ". Xin vui lòng truy cập hệ thống để có thể theo dõi những thông tin mới nhất.";

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
            }
        }

        public async Task SendEmailWinnerAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var SessionWinner = await _SessionDetailService.Getwinner(session.Id);
            var Winner = await _UserService.GetUserByID(SessionWinner.UserId);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = Winner.Email;
            string subject = "BIDs - Đấu Giá";

            string content = "Tài khoản "
                + Winner.Email
                + " đã đấu giá thành công sản phẩm "
                + item.ElementAt(0).Name
                + " với mức giá "
                + SessionWinner.Price
                + ". Xin vui lòng thanh toán trong vòng 3 ngày kể từ lúc nhận được thông báo này.";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(_gmail);
            mail.To.Add(Winner.Email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;

            mail.Priority = MailPriority.High;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);
        }

        public async Task SendEmailOutOfDateAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var SessionWinner = await _SessionDetailService.Getwinner(session.Id);
            var Winner = await _UserService.GetUserByID(SessionWinner.UserId);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = Winner.Email;
            string subject = "BIDs - Đấu Giá";

            string content = "Tài khoản "
                + Winner.Email
                + " đã đấu giá thành công sản phẩm "
                + item.ElementAt(0).Name
                + " với mức giá "
                + SessionWinner.Price
                + " nhưng chưa thanh toán trong thời gian quy định."
                + " Hệ thống sẽ khóa tài khoản vĩnh viễn vì tài khoản "
                + Winner.Email
                + " đã vi phạm nghiêm trọng luật của hệ thống.";

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

            mail.From = new MailAddress(_gmail);
            mail.To.Add(Winner.Email);
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = content;

            mail.Priority = MailPriority.High;

            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            CreateBanHistoryRequest createBanHistoryRequest = new CreateBanHistoryRequest()
            {
                UserId = Winner.Id,
                Reason = "Đấu giá thành công nhưng chưa thanh toán"
            };
            await _BanHistoryService.AddNewBanHistory(createBanHistoryRequest);
        }

        public async Task<ICollection<Session>> GetSessionInStageByAuctioneer(Guid id)
        {
            if(id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            ICollection<Session> listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            ICollection<Session> checkSession = new List<Session>();
            foreach(var x in sessionDetail)
            {
                checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.InStage)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    foreach (var y in listSession)
                    {
                        if (y.Id == checkSession.ElementAt(0).Id)
                            continue;
                        if(checkSession.ElementAt(0).Status == (int)SessionStatusEnum.InStage)
                            listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionCompleteByAuctioneer(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            ICollection<Session> listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            ICollection<Session> checkSession = new List<Session>();
            foreach (var x in sessionDetail)
            {
                checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    foreach (var y in listSession)
                    {
                        if (y.Id == checkSession.ElementAt(0).Id)
                            continue;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                            listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionHaventTranferByAuctioneer(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            ICollection<Session> listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            ICollection<Session> checkSession = new List<Session>();
            foreach (var x in sessionDetail)
            {
                checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.HaventTranferYet)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    foreach (var y in listSession)
                    {
                        if (y.Id == checkSession.ElementAt(0).Id)
                            continue;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.HaventTranferYet)
                            listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<Users> GetUserWinning(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var sessionDetailWinner = await _SessionDetailService.Getwinner(id);
            var Winner = await _UserService.GetUserByID(sessionDetailWinner.UserId);
            return Winner;
        }

        public async Task<UserNotiResponse> UserNotification(int ExDay, int TypeId, string message, Guid UserId)
        {
            var CreateNotification = new CreateNotificationRequest()
            {
                ExpireDate = ExDay
            };
            var Notification = await _NotificationService.AddNewNotification(CreateNotification);
            var CreateUserNotification = new CreateUserNotificationDetailRequest()
            {
                UserId = UserId,
                NotificationId = Notification.Id,
                TypeId = TypeId,
                Messages = message
            };
            var userNoti =  await _UserNotificationDetailService.AddNewUserNotificationDetail(CreateUserNotification);
            var response = new UserNotiResponse()
            {
                UserNotificationDetail = userNoti,
                Notification = Notification
            };
            return response;
        }

        public async Task<StaffNotiResponse> StaffNotification(int ExDay, int TypeId, string message, Guid StaffId)
        {
            var CreateNotification = new CreateNotificationRequest()
            {
                ExpireDate = ExDay
            };
            var Notification = await _NotificationService.AddNewNotification(CreateNotification);
            var CreateStaffNotification = new CreateStaffNotificationDetailRequest()
            {
                StaffId = StaffId,
                NotificationId = Notification.Id,
                TypeId = TypeId,
                Messages = message
            };
            var staffNoti = await _StaffNotificationDetailService.AddNewStaffNotificationDetail(CreateStaffNotification);

            var response = new StaffNotiResponse()
            {
                StaffNotificationDetail = staffNoti,
                Notification = Notification
            };
            return response;
        }
    }
}
