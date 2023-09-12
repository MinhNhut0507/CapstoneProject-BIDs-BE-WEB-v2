using Business_Logic.Modules.BanHistoryModule.Interface;
using Business_Logic.Modules.BanHistoryModule.Request;
using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.CommonModule.Data;
using Business_Logic.Modules.CommonModule.Interface;
using Business_Logic.Modules.CommonModule.Response;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.NotificationModule.Interface;
using Business_Logic.Modules.NotificationModule.Request;
using Business_Logic.Modules.NotificationTypeModule.Interface;
using Business_Logic.Modules.SessionDetailModule.Interface;
using Business_Logic.Modules.SessionModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule.Interface;
using Business_Logic.Modules.StaffNotificationDetailModule.Request;
using Business_Logic.Modules.UserModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Interface;
using Business_Logic.Modules.UserNotificationDetailModule.Request;
using Business_Logic.Modules.UserPaymentInformationModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;

namespace Business_Logic.Modules.CommonModule
{
    public class Common : ICommon
    {
        private readonly IUserService _UserService;
        private readonly IItemService _ItemService;
        private readonly IBookingItemService _BookingItemService;
        private readonly IBanHistoryService _BanHistoryService;
        private readonly ISessionDetailService _SessionDetailService;
        private readonly ISessionService _SessionService;
        private readonly INotificationService _NotificationService;
        private readonly INotificationTypeService _NotificationTypeService;
        private readonly IStaffNotificationDetailService _StaffNotificationDetailService;
        private readonly IUserNotificationDetailService _UserNotificationDetailService;
        private readonly IUserPaymentInformationService _UserPaymentInformationService;
        private string UTCCode = "";
        public Common(IUserService UserService
            , IItemService ItemService
            , IBookingItemService BookingItemService
            , IBanHistoryService BanHistoryService
            , ISessionDetailService SessionDetailService
            , ISessionService SessionService
            , INotificationService NotificationService
            , INotificationTypeService NotificationTypeService
            , IStaffNotificationDetailService StaffNotificationDetailService
            , IUserNotificationDetailService UserNotificationDetailService
            , IUserPaymentInformationService UserPaymentInformationService)
        {
            _UserService = UserService;
            _ItemService = ItemService;
            _BookingItemService = BookingItemService;
            _SessionDetailService = SessionDetailService;
            _SessionService = SessionService;
            _BanHistoryService = BanHistoryService;
            _NotificationService = NotificationService;
            _NotificationTypeService = NotificationTypeService;
            _StaffNotificationDetailService = StaffNotificationDetailService;
            _UserNotificationDetailService = UserNotificationDetailService;
            _UserPaymentInformationService = UserPaymentInformationService;
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
                    + " đã bắt đầu và sẽ diễn ra trong thời gian từ ngày "
                    + session.BeginTime + " đến ngày "
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

        public async Task<Users> SendEmailWinnerAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var check = await CheckSessionIncrease(session.Id);
            var SessionWinner = new SessionDetail();
            var Winner = new Users();
            if (check == true)
            {
                SessionWinner = await _SessionDetailService.Getwinner(session.Id);
                Winner = await _UserService.GetUserByID(SessionWinner.UserId);
            }
            else
            {
                Winner = await GetUserWinningByJoining(session.Id);
            }

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = Winner.Email;
            string subject = "BIDs - Đấu Giá";

            string content = "Tài khoản "
                + Winner.Email
                + " đã đấu giá thành công sản phẩm "
                + item.ElementAt(0).Name
                + " với mức giá "
                + session.FinalPrice
                + ". Xin vui lòng thanh toán trong vòng 3 ngày kể từ lúc nhận được thông báo này."
                + " Nếu không sẽ coi như bạn từ chối thanh toán, tài khoản của bạn sẽ bị khóa"
                + " và bạn sẽ không được nhận lại phí đặt cọc khi tham gia đấu giá."
                + " Bạn có thể từ chối thanh toán ngay bằng cách từ chối thanh toán trong mục phiên đấu giá thắng cuộc.";

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
            return Winner;
        }

        public async Task<Users> SendEmailCompleteAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var check = await CheckSessionIncrease(session.Id);
            var SessionWinner = new SessionDetail();
            var Winner = new Users();
            if (check == true)
            {
                SessionWinner = await _SessionDetailService.Getwinner(session.Id);
                Winner = await _UserService.GetUserByID(SessionWinner.UserId);
            }
            else
            {
                Winner = await GetUserWinningByJoining(session.Id);
            }

            var owner = await _UserService.GetUserByID(item.ElementAt(0).UserId);

            string _gmail = "bidauctionfloor@gmail.com";
            string _password = "gnauvhbfubtgxjow";

            string sendto = Winner.Email;
            string subject = "BIDs - Đấu Giá";

            string content = "Tài khoản "
                + Winner.Email
                + " đã thanh toán thành công cho sản phẩm "
                + item.ElementAt(0).Name
                + " với mức giá "
                + session.FinalPrice
                + ".<br>Thông tin của người sở hữu sản phẩm là : <br>"
                + "+ Email: " + owner.Email + "<br>+ Số điện thoại : " + owner.Phone + "<br>+ Địa chỉ: " + owner.Address
                + ".<br>Thông tin của người đấu giá thành công là : <br>"
                + "+ Email: " + Winner.Email + "<br>+ Số điện thoại : " + Winner.Phone + "<br>+ Địa chỉ: " + Winner.Address
                + "<br>LƯU Ý: Hai bên vui lòng liên hệ với nhau qua thông tin đã cung cấp để có thể giao dịch sản phẩm một cách thuận tiện nhất."
                + "<br>LƯU Ý CHO NGƯỜI ĐẤU GIÁ THÀNH CÔNG: Sau khi đã nhận được hàng, vui lòng vào trang lịch sử đấu giá thành công và chọn vào 'Đã nhận hàng' hoặc 'Nhận hàng lỗi'";

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
            return Winner;
        }

        public async Task SendEmailFailAuction(Session session)
        {
            var item = await _ItemService.GetItemByID(session.ItemId);
            var check = await CheckSessionJoining(session.Id);
            var SessionWinner = new SessionDetail();
            var Winner = new Users();
            if (check == true)
            {
                SessionWinner = await _SessionDetailService.Getwinner(session.Id);
                Winner = await _UserService.GetUserByID(SessionWinner.UserId);
            }
            else
            {
                Winner = await GetUserWinningByJoining(session.Id);
            }


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
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.InStage)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.InStage)
                            if (i == listSession.Count - 1)
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
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionCompleteByWinner(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var winner = await GetUserWinning(x.SessionId);
                if (winner.Id != x.UserId)
                    continue;
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionReceivedByWinner(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var winner = await GetUserWinning(x.SessionId);
                if (winner.Id != x.UserId)
                    continue;
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Received)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionErrorItemByWinner(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var winner = await GetUserWinning(x.SessionId);
                if (winner.Id != x.UserId)
                    continue;
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.ErrorItem)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionReceivedByAuctioneer(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Received)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionErrorItemByAuctioneer(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.Complete)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.ErrorItem)
                            if (i == listSession.Count - 1)
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
            var listSession = new List<Session>();
            var sessionDetail = await _SessionDetailService.GetSessionDetailByUser(id);
            foreach (var x in sessionDetail)
            {
                var checkSession = await _SessionService.GetSessionByID(x.SessionId);
                if (listSession.Count() == 0)
                {
                    if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.HaventTranferYet)
                        listSession.Add(checkSession.ElementAt(0));
                }
                else
                {
                    for (int i = 0; i < listSession.Count; i++)
                    {
                        if (listSession.ElementAt(i).Id == checkSession.ElementAt(0).Id)
                            break;
                        if (checkSession.ElementAt(0).Status == (int)SessionStatusEnum.HaventTranferYet)
                            if (i == listSession.Count - 1)
                                listSession.Add(checkSession.ElementAt(0));
                    }
                }
            }
            return listSession;
        }

        public async Task<ICollection<Session>> GetSessionNeedToPayByUser(Guid id)
        {
            var listSession = await GetSessionHaventTranferByAuctioneer(id);

            for (int i = 0; i < listSession.Count; i++)
            {
                var winner = new Users();
                var checkIncrease = await CheckSessionIncrease(listSession.ElementAt(i).Id);
                if (checkIncrease)
                {
                    winner = await GetUserWinning(listSession.ElementAt(i).Id);
                }
                else
                {
                    winner = await GetUserWinningByJoining(listSession.ElementAt(i).Id);
                }
                if (winner.Id != id)
                {
                    listSession.Remove(listSession.ElementAt(i));
                    i--;
                }
                if (listSession.ElementAt(i).Status != (int)SessionStatusEnum.HaventTranferYet)
                {
                    listSession.Remove(listSession.ElementAt(i));
                    i--;
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
            var userNoti = await _UserNotificationDetailService.AddNewUserNotificationDetail(CreateUserNotification);
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

        public async Task<bool> CheckSessionJoining(Guid id)
        {
            var SessionDetail = await _SessionDetailService.GetSessionDetailBySession(id);
            if (SessionDetail.Count() == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CheckSessionIncrease(Guid id)
        {
            var SessionDetail = await _SessionDetailService.GetSessionDetailBySession(id);
            var sort = SessionDetail.OrderByDescending(s => s.Price);
            var Session = await _SessionService.GetSessionByID(id);
            var item = await _ItemService.GetItemByID(Session.ElementAt(0).ItemId);
            if (item.ElementAt(0).FirstPrice == sort.ElementAt(0).Price)
            {
                return false;
            }
            return true;
        }

        public async Task<Users> GetUserWinningByJoining(Guid id)
        {
            if (id == null)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var sessionDetailJoining = await _SessionDetailService.GetSessionDetailBySession(id);
            var sortList = sessionDetailJoining.OrderByDescending(s => s.CreateDate);
            var Winner = await _UserService.GetUserByID(sortList.ElementAt(sortList.Count() - 1).UserId);
            return Winner;
        }

        public async Task<BookingItem> ReAuction(UpdateItemRequest updateItemRequest)
        {
            try
            {
                var item = await _ItemService.UpdateItem(updateItemRequest);

                var bookingItem = await _BookingItemService.GetBookingItemByItem(item.Id);

                var bookingRequest = new UpdateBookingItemRequest()
                {
                    Id = bookingItem.ElementAt(0).Id,
                    Status = (int)BookingItemEnum.Waitting
                };

                var result = await _BookingItemService.UpdateStatusBookingItem(bookingRequest);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<UTCCode> ConfirmEmail(string email)
        {
            try
            {

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = email;
                string subject = "BIDs - Nâng Cấp Tài Khoản";

                UTCCode = RandomString(6);

                string UTCCODE = "Mã kích hoạt email của bạn là " + UTCCode + ".";

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = UTCCODE;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                var response = new UTCCode()
                {
                    email = email,
                    code = UTCCode,
                };
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckUTCCode(string codeInput, string codeCheck)
        {
            if (codeInput == codeCheck)
            {
                return true;
            }
            return false;
        }

        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }

        public async Task<double> Exchange()
        {
            string apiKey = "b686ea94a7a54287af0bfadc71cf1103"; // Thay bằng API Key của bạn

            using (HttpClient httpClient = new HttpClient())
            {
                string url = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";
                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    // Parse content to get the exchange rates
                    // Rates are usually stored in JSON format

                    string jsonResponse = content;

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var exchangeRates = JsonSerializer.Deserialize<ExchangeRates>(jsonResponse, options);

                    if (exchangeRates != null && exchangeRates.Rates.TryGetValue("VND", out var vndRate))
                    {
                        return vndRate;
                    }
                    else
                    {
                        return 0;
                    }

                    //content = "{\"rates\":{\"VND\":23275.00,...}}";
                    // You can use a JSON library to parse the content and extract the rate for VND.
                    // After extracting the rate for VND, you can use it for conversion.
                }
                else
                {
                    Console.WriteLine("Error: Unable to fetch exchange rates.");
                }
            }
            return 0;
        }
        class ExchangeRates
        {
            public Dictionary<string, double> Rates { get; set; }
        }

        public async Task<ICollection<Session>> GetSessionFailHadJoin()
        {
            var listSessionFail = await _SessionService.GetSessionsIsFail();
            foreach (var session in listSessionFail)
            {
                var check = await CheckSessionJoining(session.Id);
                if (!check)
                    listSessionFail.Remove(session);
            }
            return listSessionFail;
        }

        public async Task<ICollection<Users>> GetUserJoinSession(Guid sessionId)
        {
            var listSession = await _SessionService.GetSessionByID(sessionId);
            var session = listSession.ElementAt(0);
            var listSessionDetail = await _SessionDetailService.GetSessionDetailBySession(sessionId);
            var listUser = new List<Users>();
            foreach (var detail in listSessionDetail)
            {
                var user = await _UserService.GetUserByID(detail.UserId);
                if (listUser.Count == 0)
                    listUser.Add(user);
                for (int i = 0; i < listUser.Count; i++)
                {
                    if (listUser.ElementAt(i).Id == user.Id)
                    {
                        break;
                    }
                    if (i == listUser.Count - 1)
                        listUser.Add(user);
                }
            }
            var checkIncrease = await CheckSessionIncrease(sessionId);
            var winner = new Users();
            if (checkIncrease)
            {
                winner = await GetUserWinning(sessionId);
            }
            else
            {
                winner = await GetUserWinningByJoining(sessionId);
            }
            listUser.Remove(winner);
            return listUser;
        }

        public async Task<ReportTotalSessionPayment> ReportSessionTotal()
        {
            try
            {
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalCount = 0;
                var totalPrice = 0.00;
                var totalReceive = 0.00;
                var totalSend = 0.00;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Session ";
                    SqlCommand command = new SqlCommand(query, connection);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        totalCount++;
                        totalPrice += Convert.ToDouble(row["FinalPrice"]);
                    }

                    string queryPaymentUser = "SELECT * FROM PaymentUser WHERE Status = @StatusUser";
                    SqlCommand commandPaymentUser = new SqlCommand(queryPaymentUser, connection);

                    commandPaymentUser.Parameters.Add("@StatusUser", SqlDbType.NVarChar).Value = "APPROVED";

                    SqlDataAdapter adapterPaymentUser = new SqlDataAdapter(commandPaymentUser);
                    DataTable dataTablePaymentUser = new DataTable();
                    adapterPaymentUser.Fill(dataTablePaymentUser);
                    foreach (DataRow row in dataTablePaymentUser.Rows)
                    {
                        totalReceive += Convert.ToDouble(row["Amount"]);
                    }

                    string queryPaymentStaff = "SELECT * FROM PaymentStaff WHERE Status = @StatusStaff";
                    SqlCommand commandPaymentStaff = new SqlCommand(queryPaymentStaff, connection);

                    commandPaymentStaff.Parameters.Add("@StatusStaff", SqlDbType.NVarChar).Value = "OK";

                    SqlDataAdapter adapterPaymentStaff = new SqlDataAdapter(commandPaymentStaff);
                    DataTable dataTablePaymentStaff = new DataTable();
                    adapterPaymentStaff.Fill(dataTablePaymentStaff);
                    foreach (DataRow row in dataTablePaymentStaff.Rows)
                    {
                        totalSend += Convert.ToDouble(row["Amount"]);
                    }
                }
                var responseReport = new ReportTotalSessionPayment()
                {
                    TotalCount = totalCount,
                    TotalPrice = totalPrice,
                    TotalReceive = totalReceive,
                    TotalSend = totalSend
                };
                return responseReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportSessionTotal> ReportSessionTotalByDate(DateTime Start, DateTime End)
        {
            try
            {
                if (Start >= End)
                {
                    throw new Exception(ErrorMessage.CommonError.ERROR_DATE_TIME);
                }
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalCount = 0;
                var totalPayment = 0.0;
                var totalCountNotStart = 0;
                var totalPaymentNotStart = 0.0;
                var totalCountInStage = 0;
                var totalPaymentInStage = 0.0;
                var totalCountHaventTranfer = 0;
                var totalPaymentHaventTranfer = 0.0;
                var totalCountComplete = 0;
                var totalPaymentComplete = 0.0;
                var totalCountFail = 0;
                var totalPaymentFail = 0.0;
                var totalCountReceived = 0;
                var totalPaymentReceived = 0.0;
                var totalCountErrorItem = 0;
                var totalPaymentErrorItem = 0.0;
                var totalCountDelete = 0;
                var totalPaymentDelete = 0.0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Session WHERE CreateDate >= @Start AND CreateDate <= @End";
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.Add("@Start", SqlDbType.DateTime).Value = Start;
                    command.Parameters.Add("@End", SqlDbType.DateTime).Value = End;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        totalCount++;
                        totalPayment += Convert.ToDouble(row["FinalPrice"]);
                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.NotStart)
                        {
                            totalCountNotStart ++;
                            totalPaymentNotStart += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.InStage)
                        {
                            totalCountInStage++;
                            totalPaymentInStage += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.HaventTranferYet)
                        {
                            totalCountHaventTranfer++;
                            totalPaymentHaventTranfer += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Complete)
                        {
                            totalCountComplete++;
                            totalPaymentComplete += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Fail)
                        {
                            totalCountFail++;
                            totalPaymentFail += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Received)
                        {
                            totalCountReceived++;
                            totalPaymentReceived += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.ErrorItem)
                        {
                            totalCountErrorItem++;
                            totalPaymentErrorItem += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Delete)
                        {
                            totalCountDelete++;
                            totalPaymentDelete += Convert.ToDouble(row["FinalPrice"]);
                        }
                    }
                }
                var responseReport = new ReportSessionTotal()
                {
                    TotalCount = totalCount,
                    TotalPayment = totalPayment,
                    TotalCountComplete = totalCountComplete,
                    TotalCountErrorItem = totalCountErrorItem,
                    TotalCountFail = totalCountFail,
                    TotalCountHaventTranfer = totalCountHaventTranfer,
                    TotalCountInStage = totalCountInStage,
                    TotalCountNotStart = totalCountNotStart,
                    TotalCountReceived = totalCountReceived,
                    TotalPaymentComplete = totalPaymentComplete,
                    TotalPaymentErrorItem = totalPaymentErrorItem,
                    TotalPaymentFail = totalPaymentFail,
                    TotalPaymentHaventTranfer = totalPaymentHaventTranfer,
                    TotalPaymentInStage = totalPaymentInStage,
                    TotalPaymentNotStart = totalPaymentNotStart,
                    TotalPaymentReceived = totalPaymentReceived,
                    TotalCountDelete = totalCountDelete,
                    TotalPaymentDelete = totalPaymentDelete
                };
                return responseReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportPaymentUser> ReportPaymentUser(Guid UserId, DateTime Start, DateTime End)
        {
            try
            {
                if (Start >= End)
                {
                    throw new Exception(ErrorMessage.CommonError.ERROR_DATE_TIME);
                }
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalSend = 0.00;
                var totalReceive = 0.00;
                var PaymentReport = new List<PaymentReport>();
                var reportPaymentUser = new ReportPaymentUser()
                {
                    TotalReceive = totalReceive,
                    TotalSend = totalSend,
                    PaymentReport = PaymentReport
                };
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentUser Where UserID = @UserId AND PaymentDate >= @StartDate AND PaymentDate <= @EndDate AND Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    // Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = UserId;
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = Start;
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = End;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "APPROVED";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalSend += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = false,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentStaff Where UserID = @Id AND PaymentDate >= @StartDate AND PaymentDate <= @EndDate AND Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    //    Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = UserId;
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = Start;
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = End;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "OK";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalReceive += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = true,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalRecieveAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }
                return reportPaymentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportPaymentUser> ReportPaymentUserToTal(Guid UserId)
        {
            try
            {
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalSend = 0.00;
                var totalReceive = 0.00;
                var PaymentReport = new List<PaymentReport>();
                var reportPaymentUser = new ReportPaymentUser()
                {
                    TotalReceive = totalReceive,
                    TotalSend = totalSend,
                    PaymentReport = PaymentReport
                };
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentUser Where UserID = @UserId AND Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    // Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@UserID", SqlDbType.UniqueIdentifier).Value = UserId;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "APPROVED";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalSend += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = false,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentStaff Where UserID = @Id AND Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    //    Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@ID", SqlDbType.UniqueIdentifier).Value = UserId;
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "OK";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalReceive += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = true,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalRecieveAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }
                return reportPaymentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportPaymentUser> ReportPaymentToTal()
        {
            try
            {
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalSend = 0.00;
                var totalReceive = 0.00;
                var PaymentReport = new List<PaymentReport>();
                var reportPaymentUser = new ReportPaymentUser()
                {
                    TotalReceive = totalReceive,
                    TotalSend = totalSend,
                    PaymentReport = PaymentReport
                };
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentUser Where Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    // Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "APPROVED";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalSend += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = false,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM PaymentStaff Where Status = @Status";
                    SqlCommand command = new SqlCommand(query, connection);
                    //    Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@Status", SqlDbType.NVarChar).Value = "OK";

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        reportPaymentUser.TotalReceive += Convert.ToDouble(row["Amount"]);
                        var session = await _SessionService.GetSessionByID(Guid.Parse(row["SessionID"].ToString()));
                        var Payment = new PaymentReport()
                        {
                            IsReceive = true,
                            PaymentID = row["PayPalTransactionId"].ToString(),
                            PaymentTime = Convert.ToDateTime(row["PaymentDate"]),
                            PaymentTotal = Convert.ToDouble(row["Amount"]),
                            PaymentContent = Convert.ToString(row["PaymentDetails"]),
                            PayPalAccount = Convert.ToString(row["PayPalRecieveAccount"]),
                            SessionName = session.ElementAt(0).Name
                        };
                        reportPaymentUser.PaymentReport.Add(Payment);
                    }
                }
                return reportPaymentUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportSessionTotal> ReportCategoryDetail(Guid CategoryId, DateTime startDate, DateTime endDate)
        {
            try
            {
                if (CategoryId == Guid.Empty)
                {
                    throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
                }
                if (startDate >= endDate)
                {
                    throw new Exception(ErrorMessage.CommonError.ERROR_DATE_TIME);
                }
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalCount = 0;
                var totalPayment = 0.0;
                var totalCountNotStart = 0;
                var totalPaymentNotStart = 0.0;
                var totalCountInStage = 0;
                var totalPaymentInStage = 0.0;
                var totalCountHaventTranfer = 0;
                var totalPaymentHaventTranfer = 0.0;
                var totalCountComplete = 0;
                var totalPaymentComplete = 0.0;
                var totalCountFail = 0;
                var totalPaymentFail = 0.0;
                var totalCountReceived = 0;
                var totalPaymentReceived = 0.0;
                var totalCountErrorItem = 0;
                var totalPaymentErrorItem = 0.0;
                var totalCountDelete = 0;
                var totalPaymentDelete = 0.0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT s.* " +
                        "FROM Session AS s " +
                        "INNER JOIN Item AS i ON s.ItemID = i.ID " +
                        "WHERE i.CategoryID = @CategoryId " +
                        "AND s.CreateDate >= @StartDate AND s.CreateDate <= @EndDate;";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate;
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate;
                    command.Parameters.Add("@CategoryId", SqlDbType.UniqueIdentifier).Value = CategoryId;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        totalCount++;
                        totalPayment += Convert.ToDouble(row["FinalPrice"]);
                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.NotStart)
                        {
                            totalCountNotStart++;
                            totalPaymentNotStart += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.InStage)
                        {
                            totalCountInStage++;
                            totalPaymentInStage += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.HaventTranferYet)
                        {
                            totalCountHaventTranfer++;
                            totalPaymentHaventTranfer += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Complete)
                        {
                            totalCountComplete++;
                            totalPaymentComplete += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Fail)
                        {
                            totalCountFail++;
                            totalPaymentFail += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Received)
                        {
                            totalCountReceived++;
                            totalPaymentReceived += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.ErrorItem)
                        {
                            totalCountErrorItem++;
                            totalPaymentErrorItem += Convert.ToDouble(row["FinalPrice"]);
                        }

                        if (Convert.ToInt32(row["Status"]) == (int)SessionStatusEnum.Delete)
                        {
                            totalCountDelete++;
                            totalPaymentDelete += Convert.ToDouble(row["FinalPrice"]);
                        }
                    }
                }
                var responseReport = new ReportSessionTotal()
                {
                    TotalCount = totalCount,
                    TotalPayment = totalPayment,
                    TotalCountComplete = totalCountComplete,
                    TotalCountErrorItem = totalCountErrorItem,
                    TotalCountFail = totalCountFail,
                    TotalCountHaventTranfer = totalCountHaventTranfer,
                    TotalCountInStage = totalCountInStage,
                    TotalCountNotStart = totalCountNotStart,
                    TotalCountReceived = totalCountReceived,
                    TotalPaymentComplete = totalPaymentComplete,
                    TotalPaymentErrorItem = totalPaymentErrorItem,
                    TotalPaymentFail = totalPaymentFail,
                    TotalPaymentHaventTranfer = totalPaymentHaventTranfer,
                    TotalPaymentInStage = totalPaymentInStage,
                    TotalPaymentNotStart = totalPaymentNotStart,
                    TotalPaymentReceived = totalPaymentReceived,
                    TotalCountDelete = totalCountDelete,
                    TotalPaymentDelete = totalPaymentDelete
                };
                return responseReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<ReportUser> ReportUser(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate >= endDate)
                {
                    throw new Exception(ErrorMessage.CommonError.ERROR_DATE_TIME);
                }
                string connectionString = "server =DESKTOP-ARAK6K1\\SQLEXPRESS;database=BIDsLocal;uid=sa;pwd=05072001;Trusted_Connection=True;TrustServerCertificate=True;";
                //string connectionString = "Server = tcp:bidonlinetesting.database.windows.net,1433; Initial Catalog = bidtest; Persist Security Info = False; User ID = bid - admin; Password = 123Helloall!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;\r<br>";
                var totalCount = 0;
                var totalAccepted = 0;
                var totalRejected = 0;
                var totalBanned = 0;
                var totalWaiting = 0;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM Users WHERE CreateDate >= @StartDate AND CreateDate <= @EndDate";
                    SqlCommand command = new SqlCommand(query, connection);

                    // Thay đổi giá trị của tham số ngày tháng tương ứng
                    command.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate;
                    command.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    foreach (DataRow row in dataTable.Rows)
                    {
                        totalCount++;
                        if (Convert.ToInt32(row["Status"]) == (int)UserStatusEnum.Acctive)
                        {
                            totalAccepted++;
                        }
                        if (Convert.ToInt32(row["Status"]) == (int)UserStatusEnum.Waitting)
                        {
                            totalWaiting++;
                        }
                        if (Convert.ToInt32(row["Status"]) == (int)UserStatusEnum.Ban)
                        {
                            totalBanned++;
                        }
                        if (Convert.ToInt32(row["Status"]) == (int)UserStatusEnum.Deny)
                        {
                            totalRejected++;
                        }
                    }
                }
                var responseReport = new ReportUser()
                {
                    TotalCount = totalCount,
                    TotalAccountAccepted = totalAccepted,
                    TotalAccountBanned = totalBanned,
                    TotalAccountRejected = totalRejected,
                    TotalAccountWaiting = totalWaiting
                };
                return responseReport;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
