using Business_Logic.Modules.BookingItemModule.Interface;
using Business_Logic.Modules.BookingItemModule.Request;
using Business_Logic.Modules.ItemModule.Interface;
using Business_Logic.Modules.ItemModule.Request;
using Business_Logic.Modules.StaffModule.Interface;
using Business_Logic.Modules.UserModule;
using Business_Logic.Modules.UserModule.Interface;
using Data_Access.Constant;
using Data_Access.Entities;
using Data_Access.Enum;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;

namespace Business_Logic.Modules.BookingItemModule
{
    public class BookingItemService : IBookingItemService
    {
        private readonly IBookingItemRepository _BookingItemRepository;
        private readonly IStaffRepository _StaffRepository;
        public BookingItemService(IBookingItemRepository BookingItemRepository
            , IStaffRepository staffRepository)
        {
            _BookingItemRepository = BookingItemRepository;
            _StaffRepository = staffRepository;
        }

        public async Task<ICollection<BookingItem>> GetAll()
        {
            return await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.OrderByDescending(x => x.UpdateDate).ToList());
        }


        public async Task<ICollection<BookingItem>> GetBookingItemByID(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Id == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByStaff(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.StaffId == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByStaffIsWatting(string email)
        {

            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_NULL);
            }
            var staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.StaffId == staff.Id && x.Status == (int) BookingItemEnum.Waitting).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByStaffIsBeginWatting(string email)
        {

            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_NULL);
            }
            var staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.StaffId == staff.Id && x.Status == (int)BookingItemEnum.SessionWaitting).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByUserIsWaiting(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)BookingItemEnum.Waitting).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByUserIsBeginWaiting(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)BookingItemEnum.SessionWaitting).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByUserIsNotCreateSession(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)BookingItemEnum.NotCreateSessionYet).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByUserIsAccepted(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)BookingItemEnum.Accepted).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByUserIsDenied(Guid id)
        {

            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }

            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Item.UserId == id && x.Status == (int)BookingItemEnum.Denied).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByStaffToCreateSession(string email)
        {

            if (email == null)
            {
                throw new Exception(ErrorMessage.CommonError.EMAIL_IS_NULL);
            }
            var staff = await _StaffRepository.GetFirstOrDefaultAsync(x => x.Email == email);
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.StaffId == staff.Id && x.Status == (int)BookingItemEnum.NotCreateSessionYet).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task<ICollection<BookingItem>> GetBookingItemByItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new Exception(ErrorMessage.CommonError.ID_IS_NULL);
            }
            var BookingItem = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.ItemId == id).ToList());
            if (BookingItem == null)
            {
                throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
            }
            return BookingItem;
        }

        public async Task AddNewBookingItem(CreateBookingItemRequest BookingItemRequest)
        {

            ValidationResult result = new CreateBookingItemRequestValidator().Validate(BookingItemRequest);
            if (!result.IsValid)
            {
                throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
            }

            ICollection<Staff> listStaffActive = await _StaffRepository.GetStaffsBy(x => x.Status == true);
            Random random = new Random();

            var newBookingItem = new BookingItem();

            newBookingItem.Id = Guid.NewGuid();
            newBookingItem.ItemId = BookingItemRequest.ItemId;
            newBookingItem.StaffId = listStaffActive.ElementAt(random.Next(0,listStaffActive.Count-1)).Id;
            DateTime dateTime = DateTime.UtcNow;
            newBookingItem.CreateDate = dateTime.AddHours(7);
            newBookingItem.UpdateDate = dateTime.AddHours(7);
            newBookingItem.Status = BookingItemRequest.Status;

            await _BookingItemRepository.AddAsync(newBookingItem);
        }

        public async Task<BookingItem> UpdateStatusBookingItem(UpdateBookingItemRequest BookingItemRequest)
        {
            try
            {
                var BookingItemUpdate = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Id == BookingItemRequest.Id).ToList());

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }

                ValidationResult result = new UpdateBookingItemRequestValidator().Validate(BookingItemRequest);
                if (!result.IsValid)
                {
                    throw new Exception(ErrorMessage.CommonError.INVALID_REQUEST);
                }

                var bookingAccept = BookingItemUpdate.ElementAt(0);

                DateTime dateTime = DateTime.UtcNow;
                bookingAccept.UpdateDate = dateTime.AddHours(7);
                bookingAccept.Status = BookingItemRequest.Status;

                await _BookingItemRepository.UpdateAsync(bookingAccept);
                return bookingAccept;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<BookingItem> AcceptStatusBookingItem(Guid id)
        {
            try
            {
                var BookingItemUpdate = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Id == id).ToList());

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }

                var bookingAccept = BookingItemUpdate.ElementAt(0);
                DateTime dateTime = DateTime.UtcNow;
                bookingAccept.UpdateDate = dateTime.AddHours(7);
                bookingAccept.Status = (int)BookingItemEnum.NotCreateSessionYet;

                await _BookingItemRepository.UpdateAsync(bookingAccept);

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = BookingItemUpdate.ElementAt(0).Item.User.Email;
                string subject = "[BIDs] - Vật Phẩm Đấu Giá";
                string content = "Vật phẩm " + BookingItemUpdate.ElementAt(0).Item.Name + " đã được chấp thuận và sẽ được đưa lên hệ thống. Xin chân thành cảm ơn vì đã sử dụng hệ thống của chúng tôi.";


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return bookingAccept;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }

        public async Task<BookingItem> DenyStatusBookingItem(Guid id, string reason)
        {
            try
            {
                var BookingItemUpdate = await _BookingItemRepository.GetAll(includeProperties: "Staff,Item,Item.User,Item.Category,Item.Images,Item.Category.Descriptions,Item.ItemDescriptions"
                , options: o => o.Where(x => x.Id == id).ToList());

                if (BookingItemUpdate == null)
                {
                    throw new Exception(ErrorMessage.BookingItemError.BOOKING_ITEM_NOT_FOUND);
                }

                var bookingAccept = BookingItemUpdate.ElementAt(0);
                DateTime dateTime = DateTime.UtcNow;
                bookingAccept.UpdateDate = dateTime.AddHours(7);
                bookingAccept.Status = (int)BookingItemEnum.Denied;

                await _BookingItemRepository.UpdateAsync(bookingAccept);

                string _gmail = "bidauctionfloor@gmail.com";
                string _password = "gnauvhbfubtgxjow";

                string sendto = BookingItemUpdate.ElementAt(0).Item.User.Email;
                string subject = "[BIDs] - Vật Phẩm Đấu Giá";
                string content = "Vật phẩm " + BookingItemUpdate.ElementAt(0).Item.Name + " khởi tạo không thành công vì thông tin bạn cung cấp không chính xác!"
                    + " Cụ thể lỗi ở việc "
                    + reason
                    + ". Bạn hãy cung cấp đúng thông tin hơn trong lần tiếp theo.";


                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(_gmail);
                mail.To.Add(sendto);
                mail.Subject = subject;
                mail.IsBodyHtml = true;
                mail.Body = content;

                mail.Priority = MailPriority.High;

                SmtpServer.Port = 587;
                SmtpServer.UseDefaultCredentials = false;
                SmtpServer.Credentials = new NetworkCredential(_gmail, _password);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);

                return bookingAccept;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error at update type: " + ex.Message);
                throw new Exception(ex.Message);
            }

        }
    }
}
