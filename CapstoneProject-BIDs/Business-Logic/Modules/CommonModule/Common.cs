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

namespace Business_Logic.Modules.CommonModule
{
    public class Common : ICommon
    {
        private readonly IUserService _UserService;
        private readonly IItemService _ItemService;
        private readonly IBanHistoryService _BanHistoryService;
        private readonly ISessionDetailService _SessionDetailService;
        private readonly ISessionService _SessionService;
        public Common(IUserService UserService
            , IItemService ItemService
            , IBanHistoryService BanHistoryService
            , ISessionDetailService SessionDetailService
            , ISessionService SessionService)
        {
            _UserService = UserService;
            _ItemService = ItemService;
            _SessionDetailService = SessionDetailService;
            _SessionService = SessionService;
            _BanHistoryService = BanHistoryService;
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
                    + " đã bắt đầu và sẽ diễn ra trong thời gian "
                    + session.AuctionTime.Hours + " giờ "
                    + session.AuctionTime.Minutes + " phút"
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
            var SessionWinner = await _SessionDetailService.Getwinner();
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
            var SessionWinner = await _SessionDetailService.Getwinner();
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

        public async Task<ICollection<Session>> GetSessionInStageByUser(Guid id)
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
                foreach(var y in listSession)
                {
                    if (y.Id == checkSession.ElementAt(0).Id)
                        continue;
                    listSession.Add(checkSession.ElementAt(0));
                }
            }
            return listSession;
        }
    }
}
