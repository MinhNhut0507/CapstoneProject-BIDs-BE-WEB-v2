using Business_Logic.Modules.BookingItemModule.Request;
using Data_Access.Entities;

namespace Business_Logic.Modules.BookingItemModule.Interface
{
    public interface IBookingItemService
    {
        public Task AddNewBookingItem(CreateBookingItemRequest BookingItemCreate);

        public Task<BookingItem> UpdateStatusBookingItem(UpdateBookingItemRequest BookingItemUpdate);

        public Task<BookingItem> AcceptStatusBookingItem(Guid id);

        public Task<BookingItem> DenyStatusBookingItem(Guid id, string reason);

        public Task<ICollection<BookingItem>> GetAll();
        public Task<ICollection<BookingItem>> GetBookingItemByID(Guid id);

        public Task<ICollection<BookingItem>> GetBookingItemByItem(Guid id);

        public Task<ICollection<BookingItem>> GetBookingItemByStaff(Guid id);

        public Task<ICollection<BookingItem>> GetBookingItemByStaffIsWatting(string email);

        public Task<ICollection<BookingItem>> GetBookingItemByStaffToCreateSession(string email);
        public Task<ICollection<BookingItem>> GetBookingItemByUserIsWaiting(Guid id);
        public Task<ICollection<BookingItem>> GetBookingItemByUserIsNotCreateSession(Guid id);
        public Task<ICollection<BookingItem>> GetBookingItemByUserIsAccepted(Guid id);
        public Task<ICollection<BookingItem>> GetBookingItemByUserIsDenied(Guid id);
    }
}
